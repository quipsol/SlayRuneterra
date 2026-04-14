using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class VanguardRanger : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 20);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 22);
    
    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/vanguard_ranger.tscn";
    
    #region MoveStateMachine
    
    private const string NORMAL_SHOT = "NORMAL_SHOT";
    private const string CHARGE_SHOT = "CHARGE_SHOT";
    private const string CHARGE = "CHARGE";

    private int NormalShotDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
    private int ChargeShotDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 22, 20);
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState normalShotState = new MoveState(NORMAL_SHOT, NormalShot, new SingleAttackIntent(NormalShotDamage));
        MoveState chargeShotState = new MoveState(CHARGE_SHOT, ChargeShot, new SingleAttackIntent(ChargeShotDamage));
        MoveState chargeState = new MoveState(CHARGE, Charge, new BuffIntent());
        normalShotState.FollowUpState = chargeState;
        chargeState.FollowUpState = chargeShotState;
        chargeShotState.FollowUpState = chargeState;
        states.Add(normalShotState);
        states.Add(chargeState);
        states.Add(chargeShotState);
        
        return new MonsterMoveStateMachine(states, normalShotState);
    }
    
    #region State Methods
    
    private async Task NormalShot(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(NormalShotDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task ChargeShot(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(ChargeShotDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private Task Charge(IReadOnlyList<Creature> targets)
    {
        // noop
        return Task.CompletedTask;
    }
    
    #endregion State Methods
    
    #endregion MoveStateMachine
}