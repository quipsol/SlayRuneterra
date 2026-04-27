using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class PetriciteGolem : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 74, 71);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 75, 72);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/vanguard_ranger.tscn";

    #region MoveStateMachine

    private const string SLAM = "SLAM";
    private const string ROCK_SOLID = "ROCK_SOLID";

    private int SlamDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
    private int RockSolidBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState slamState = new MoveState(SLAM, PerformSlam, new SingleAttackIntent(SlamDamage));
        MoveState rockSolidState = new MoveState(ROCK_SOLID, PerformRockSolid, new DefendIntent());
        slamState.FollowUpState = rockSolidState;
        rockSolidState.FollowUpState = slamState;
        states.Add(slamState);
        states.Add(rockSolidState);
        
        return new MonsterMoveStateMachine(states, slamState);
    }

    #region State Methods

    private async Task PerformSlam(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(SlamDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    private async Task PerformRockSolid(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.GainBlock(Creature, new BlockVar(RockSolidBlock, ValueProp.Move), null);
    }


    #endregion State Methods

    #endregion MoveStateMachine

    public override async Task AfterAddedToRoom()
    {
        await PowerCmd.Apply<PetricitePower>(new ThrowingPlayerChoiceContext(), Creature, 1, Creature, null);
    }
}