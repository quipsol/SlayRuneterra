using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class Valor : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 44, 42);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 46, 44);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/valor.tscn";

    #region MoveStateMachine

    private const string GOUGE = "GOUGE";
    private const string BLIND = "BLIND";
    private const string PECK = "PECK";
    private const string GAUGE_OR_BLIND = "GAUGE_OR_BLIND";

    private int GougeWeak => 2;
    private int GougeVulnerable => 2;
    private int BlindAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 5);
    private int PeckDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
    private int PeckHits => 4;

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState gougeState = new MoveState(GOUGE, PerformGouge, new DebuffIntent());
        MoveState blindState = new MoveState(BLIND, PerformBlind, new DebuffIntent());
        MoveState peckState = new MoveState(PECK, PerformPeck, new MultiAttackIntent(PeckDamage, PeckHits));

        ConditionalBranchState gaugeOrBlindState = new ConditionalBranchState(GAUGE_OR_BLIND);
        gaugeOrBlindState.AddState(gougeState, () => !HasQuinnAlly());
        gaugeOrBlindState.AddState(blindState, HasQuinnAlly);
        
        gougeState.FollowUpState = peckState;
        blindState.FollowUpState = peckState;
        peckState.FollowUpState = gaugeOrBlindState;

        states.Add(gougeState);
        states.Add(blindState);
        states.Add(peckState);
        states.Add(gaugeOrBlindState);
        
        return new MonsterMoveStateMachine(states, gaugeOrBlindState);
    }

    #region State Methods

    private async Task PerformGouge(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<WeakPower>(targets, GougeWeak, this.Creature, null);
        await PowerCmd.Apply<VulnerablePower>(targets, GougeVulnerable, this.Creature, null);
    }
    private async Task PerformBlind(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<BlindPower>(targets, BlindAmount, this.Creature, null);
    }

    private async Task PerformPeck(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(PeckDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithHitCount(PeckHits)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
  


    #endregion State Methods

    private bool HasQuinnAlly()
    {
        return this.Creature.CombatState!.GetTeammatesOf(this.Creature).Any(c => c is { Monster: Quinn, IsAlive: true });
    }
    
    #endregion MoveStateMachine
    

    public override async Task AfterAddedToRoom()
    {
         await PowerCmd.Apply<ValorEnragePower>(this.Creature, 1, this.Creature, null);
 
    }

}