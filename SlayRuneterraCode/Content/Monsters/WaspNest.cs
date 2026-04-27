using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class WaspNest : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 121, 91);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 122, 92);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/wasp_nest.tscn";

    #region MoveStateMachine

    private const string SUMMON_WASP = "SUMMON_WASP";
    private const string BUFF_WASPS = "BUFF_WASPS";
    private const string SUMMON_OR_BUFF = "SUMMON_OR_BUFF";

    private int WaspAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 1, 1);
    private int WaspsBuff => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState summonWaspState = new MoveState(SUMMON_WASP, PerformSummonWasp, new SummonIntent());
        MoveState buffWaspState = new MoveState(BUFF_WASPS, PerformBuffWasps, new BuffIntent());
        
        ConditionalBranchState summonOrBuffState = new ConditionalBranchState(SUMMON_OR_BUFF);
        summonOrBuffState.AddState(summonWaspState, AnyFreeWaspSlot);
        summonOrBuffState.AddState(buffWaspState, () => !AnyFreeWaspSlot());
        
        summonWaspState.FollowUpState = summonOrBuffState;
        buffWaspState.FollowUpState = summonOrBuffState;
        states.Add(summonWaspState);
        states.Add(buffWaspState);
        states.Add(summonOrBuffState);
        return new MonsterMoveStateMachine(states, summonOrBuffState);
    }

    #region State Methods

    private async Task PerformSummonWasp(IReadOnlyList<Creature> targets)
    {
        for (int i = 0; i < WaspAmount; i++)
        {
            string slotName = CombatState.Encounter!.Slots.LastOrDefault(s => CombatState.Enemies.All(creature => creature.SlotName != s), string.Empty);
            if (slotName == "") break;
            await CreatureCmd.Add( ModelDb.Monster<Wasp>().ToMutable(), this.CombatState, this.Creature.Side, slotName);
        }
    }

    private async Task PerformBuffWasps(IReadOnlyList<Creature> targets)
    {
        foreach (var creature in CombatState.Enemies)
        {
            if (creature.Monster is Wasp)
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), creature, WaspsBuff, Creature, null);
        }
    }
    
    #endregion State Methods

    private bool AnyFreeWaspSlot()
    {
        return !CombatState.Encounter!.Slots.All(s => CombatState.Enemies.Any(creature => creature.SlotName == s));
    }
    
    #endregion MoveStateMachine
}