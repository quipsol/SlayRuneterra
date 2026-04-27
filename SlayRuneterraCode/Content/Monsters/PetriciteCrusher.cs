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
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class PetriciteCrusher : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 124, 121);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 125, 122);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/vanguard_ranger.tscn";

    #region MoveStateMachine

    private const string CRUSH = "CRUSH";
    private const string STAMPEDE = "STAMPEDE";
    private const string CONVERT = "CONVERT";

    private int CrushDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 10);
    private int StampedeDamage => 1;
    private int StampedeHits => 2;
    private int ConvertHpLoss => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 10);
    private int ConvertStrength => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
    private int ConvertBlock => 10;

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState crushState = new MoveState(CRUSH, PerformCrush, new SingleAttackIntent(CrushDamage));
        MoveState stampedeState = new MoveState(STAMPEDE, PerformStampede, new MultiAttackIntent(StampedeDamage, StampedeHits));
        MoveState convertState = new MoveState(CONVERT, PerformConvert, new BuffIntent(), new DefendIntent());
        crushState.FollowUpState = stampedeState;
        stampedeState.FollowUpState = convertState;
        convertState.FollowUpState = crushState;
        states.Add(crushState);
        states.Add(stampedeState);
        states.Add(convertState);
        return new MonsterMoveStateMachine(states, crushState);
    }

    #region State Methods

    private async Task PerformCrush(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(CrushDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task PerformStampede(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(StampedeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithHitCount(StampedeHits)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task PerformConvert(IReadOnlyList<Creature> targets)
    {
        if (Creature.CurrentHp > ConvertHpLoss * 3)
        {
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Creature, ConvertHpLoss, ValueProp.Unpowered, Creature);
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, ConvertStrength, Creature, null);
            await CreatureCmd.GainBlock(Creature, new BlockVar(ConvertBlock, ValueProp.Move), null);
        }
        else
        {
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, 1, Creature, null);
            await CreatureCmd.GainBlock(Creature, new BlockVar(ConvertBlock*2, ValueProp.Move), null);
        }
    }


    #endregion State Methods

    #endregion MoveStateMachine
}