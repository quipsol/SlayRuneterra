using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class Quinn : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 94, 92);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 96, 94);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/quinn.tscn";

    #region MoveStateMachine

    private const string VOLLEY = "VOLLEY";
    private const string FOCUS_SHOT = "FOCUS_SHOT";
    private const string PREPARE = "PREPARE";

    private int VolleyDamage => 5;
    private int VolleyHits => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
    private int FocusShotDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 10);
    private int PrepareBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 10);
    private int PrepareVulnerable => 2;

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState volleyState = new MoveState(VOLLEY, PerformVolley, new MultiAttackIntent(VolleyDamage,VolleyHits));
        MoveState focusShotState = new MoveState(FOCUS_SHOT, PerformFocusShot, new SingleAttackIntent(FocusShotDamage));
        MoveState prepareState = new MoveState(PREPARE, PerformPrepare, new DefendIntent(), new DebuffIntent());
        volleyState.FollowUpState = prepareState;
        focusShotState.FollowUpState = volleyState;
        prepareState.FollowUpState = focusShotState;

        states.Add(volleyState);
        states.Add(focusShotState);
        states.Add(prepareState);
        
        return new MonsterMoveStateMachine(states, focusShotState);
    }

    #region State Methods

    private async Task PerformVolley(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(VolleyDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithHitCount(VolleyHits)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task PerformFocusShot(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(FocusShotDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task PerformPrepare(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.GainBlock(this.Creature, new BlockVar(PrepareBlock, ValueProp.Move), null);
        await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, PrepareVulnerable, this.Creature, null);
    }


    #endregion State Methods

    #endregion MoveStateMachine
    
    public override async Task AfterAddedToRoom()
    {
        await PowerCmd.Apply<QuinnEnragePower>(new ThrowingPlayerChoiceContext(), this.Creature, 1, this.Creature, null);
    }
    
    
}