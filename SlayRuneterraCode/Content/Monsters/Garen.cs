using MegaCrit.Sts2.Core.Combat;
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
using SlayRuneterra.Utils;

namespace SlayRuneterra.Content.Monsters;

public class Garen : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 255, 245);
    public override int MaxInitialHp => MinInitialHp;

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/garen.tscn";

    #region MoveStateMachine

    private const string DETERMINE_NEXT_MOVE = "DETERMINE_NEXT_MOVE";
    private const string JUSTICE = "JUSTICE";
    private const string JUSTICE_RECOVERY = "JUSTICE_RECOVERY";
    private const string JUDGEMENT = "JUDGEMENT";
    private const string DECISIVE_STRIKE = "DECISIVE_STRIKE";
    private const string COURAGE = "COURAGE";

    private int JusticeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 35, 30);
    private int JudgementDamage => 2;
    private int JudgementHits => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
    private int DecisiveStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 8);
    private int DecisiveStrikeWeak => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 1);
    private int CourageBlock => 10;
    private int CourageStrength => 1;

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState justiceState = new MoveState(JUSTICE, PerformJustice, new SingleAttackIntent(JusticeDamage));
        MoveState stunState = new MoveState(JUSTICE_RECOVERY, PerformStun, new StunIntent());
        MoveState judgementState = new MoveState(JUDGEMENT, PerformJudgement, new MultiAttackIntent(JudgementDamage, JudgementHits));
        MoveState decisiveStrikeState = new MoveState(DECISIVE_STRIKE, PerformDecisiveStrike, new SingleAttackIntent(DecisiveStrikeDamage), new DebuffIntent());
        MoveState courageState = new MoveState(COURAGE, PerformCourage, new DefendIntent(), new BuffIntent());
        
        OrderInterceptMoveState determineNextMove = new(DETERMINE_NEXT_MOVE);
        determineNextMove.CanInterceptMultipleTimes = false;
        determineNextMove.SetInterceptMove(justiceState, ShouldPerformJustice);
        determineNextMove.AddState(judgementState);
        determineNextMove.AddState(decisiveStrikeState);
        determineNextMove.AddState(courageState);
        
        justiceState.FollowUpState = stunState;
        stunState.FollowUpState = determineNextMove;
        judgementState.FollowUpState = determineNextMove;
        decisiveStrikeState.FollowUpState = determineNextMove;
        courageState.FollowUpState = determineNextMove;

        states.Add(justiceState);
        states.Add(stunState);
        states.Add(judgementState);
        states.Add(decisiveStrikeState);
        states.Add(courageState);
        states.Add(determineNextMove);
        
        return new MonsterMoveStateMachine(states, determineNextMove);
    }

    #region State Methods

    private async Task PerformJustice(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(JusticeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task PerformJudgement(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(JudgementDamage)
                    .FromMonster(this)
                    .WithAttackerAnim("Attack", 0.3f)
                    .WithHitCount(JudgementHits)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task PerformDecisiveStrike(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(DecisiveStrikeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, DecisiveStrikeWeak, this.Creature, null);
    }
    
    private async Task PerformCourage(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.GainBlock(this.Creature, new BlockVar(CourageBlock, ValueProp.Move), null);
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), this.Creature, CourageStrength, this.Creature, null);
    }
    
    private async Task PerformStun(IReadOnlyList<Creature> targets)
    {
        await Cmd.CustomScaledWait(0.33f, 1f);
    }


    #endregion State Methods

    private bool ShouldPerformJustice()
    {
        return this.Creature.CombatState!.GetCreaturesOnSide(CombatSide.Player).Any(creature => creature is { IsAlive: true, IsPet: false } && creature.CurrentHp <= creature.MaxHp * 0.3f);
    }
    
    #endregion MoveStateMachine
    
}