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

public class Wasp : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 12, 8);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 14, 12);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/wasp.tscn";

    #region MoveStateMachine

    private const string STING = "STING";

    private int StingDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState stingState = new MoveState(STING, PerformSting, new SingleAttackIntent(StingDamage));
        stingState.FollowUpState = stingState;
        states.Add(stingState);
        return new MonsterMoveStateMachine(states, stingState);
    }

    #region State Methods

    private async Task PerformSting(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(StingDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }


    #endregion State Methods

    #endregion MoveStateMachine

    public override async Task AfterAddedToRoom()
    {
        await PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), Creature, 1m, Creature, null);
        await PowerCmd.Apply<WaspEnragePower>(new ThrowingPlayerChoiceContext(), Creature, 1m, Creature, null);
    }
}