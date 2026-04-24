using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class XinZhao : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 116, 110);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 120, 114);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/vanguard_ranger.tscn";

    #region MoveStateMachine

    private const string DETERMINATION = "DETERMINATION";
    private const string THREE_TALON_STRIKE = "THREE_TALON_STRIKE";
    private const string AUDACIOUS_CHARGE = "AUDACIOUS_CHARGE";

    private int HealAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 30, 20);
    private int ThreeTalonStrikeCount => 3;
    private int ThreeTalonStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
    private int AudaciousChargeStrength => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 1);
    private int AudaciousChargeDamage => 13;
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState threeTalonStrikeState = new MoveState(THREE_TALON_STRIKE, PerformThreeTalonSrike, new MultiAttackIntent(ThreeTalonStrikeDamage, ThreeTalonStrikeCount));
        MoveState audaciousChargeState = new MoveState(AUDACIOUS_CHARGE, PerformAudaciousStrike, new SingleAttackIntent(AudaciousChargeDamage), new BuffIntent());
        MoveState determinationState = new MoveState(DETERMINATION, PerformDetermination, new HealIntent());
        threeTalonStrikeState.FollowUpState = audaciousChargeState;
        audaciousChargeState.FollowUpState = determinationState;
        determinationState.FollowUpState = threeTalonStrikeState;
        states.Add(threeTalonStrikeState);
        states.Add(audaciousChargeState);
        states.Add(determinationState);
        return new MonsterMoveStateMachine(states, threeTalonStrikeState);
    }

    #region State Methods

    private async Task PerformAudaciousStrike(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(AudaciousChargeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, AudaciousChargeStrength, Creature, null);
    }
    
    private async Task PerformDetermination(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.Heal(Creature, HealAmount);
    }
    
    private async Task PerformThreeTalonSrike(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(ThreeTalonStrikeDamage)
                    .FromMonster(this)
                    .WithAttackerAnim("Attack", 0.3f)
                    .WithHitCount(ThreeTalonStrikeCount)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }


    #endregion State Methods

    #endregion MoveStateMachine
}