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

public class PluckyPoro : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 64, 62);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 66, 64);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/plucky_poro.tscn";

    #region MoveStateMachine

    private const string ATTACK = "ATTACK";
    private const string BUFF = "BUFF";

    private int AttackDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
    private int BuffStrengthAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState attackState = new MoveState(ATTACK, PerformAttack, new SingleAttackIntent(AttackDamage));
        MoveState buffState = new MoveState(BUFF, PerformBuff, new BuffIntent());
        attackState.FollowUpState = buffState;
        buffState.FollowUpState = attackState;

        states.Add(attackState);
        states.Add(buffState);
        return new MonsterMoveStateMachine(states, buffState);
    }

    #region State Methods

    private async Task PerformAttack(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(AttackDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task PerformBuff(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), this.Creature, BuffStrengthAmount, this.Creature, null);
    }


    #endregion State Methods

    #endregion MoveStateMachine
}