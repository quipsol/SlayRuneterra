using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class SilverwingVanguard: SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 34, 32);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 36, 34);
    
    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/silverwing_vanguard.tscn";
    
    #region MoveStateMachine
    
    private const string WEAK = "WEAK";
    private const string ATTACK = "ATTACK";
    private int AttackDamage => 2;
    private int AttackHits => 5;
    private int FlutterStacks => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 2);
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState weakState = new MoveState(WEAK, Weak, new DebuffIntent());
        MoveState attackState = new MoveState(ATTACK, Attack, new MultiAttackIntent(AttackDamage, AttackHits));
        
        
        attackState.FollowUpState = weakState;
        weakState.FollowUpState = attackState;
        states.Add(attackState);
        states.Add(weakState);
        
        return new MonsterMoveStateMachine(states, weakState);
    }
    
    #region State Methods
    
    private async Task Attack(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(AttackDamage)
                    .FromMonster(this)
                    .WithAttackerAnim("Attack", 0.3f)
                    .WithHitCount(AttackHits)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }

    private async Task Weak(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, 2, this.Creature, null);
    }
    
    #endregion State Methods
    
    #endregion MoveStateMachine
    
    public override async Task AfterAddedToRoom()
    {
        await PowerCmd.Apply<DefinitelyNotFlutterPower>(new ThrowingPlayerChoiceContext(), this.Creature, FlutterStacks, this.Creature, null);
    }
}