using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class VanguardDefender : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 44, 42);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 46, 44);
    
    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/vanguard_defender.tscn";
    
    #region MoveStateMachine
    
    private const string SINGLE = "SINGLE";
    private const string BLOCK = "BLOCK";
    private const string SINGLE_OR_BLOCK_BRANCH = "SINGLE_OR_BLOCK_BRANCH";
    
    private int SingleDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
    private int BlockAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState singleState = new MoveState(SINGLE, Single, new SingleAttackIntent(SingleDamage));
        MoveState blockState = new MoveState(BLOCK, Block, new DefendIntent());
        
        // What if both are true? What determines which state has priority? Is it the AddState order?
        ConditionalBranchState singleOrBlockState = new ConditionalBranchState(SINGLE_OR_BLOCK_BRANCH);
        singleOrBlockState.AddState(singleState, () => !HasAlly() || AreAllAlliesVanguardDefender());
        singleOrBlockState.AddState(blockState, () => HasAlly() && !AreAllAlliesVanguardDefender());
        
        blockState.FollowUpState = singleOrBlockState;
        singleState.FollowUpState = singleState;
        states.Add(singleState);
        states.Add(blockState);
        states.Add(singleOrBlockState);
        
        return new MonsterMoveStateMachine(states, blockState);
    }
    
    #region State Methods
    

    
    private async Task Single(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(SingleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task Block(IReadOnlyList<Creature> targets)
    {
        
        var target = GetRandomAlly();
        if(target != null)
            await CreatureCmd.GainBlock(target, new BlockVar(BlockAmount, ValueProp.Move), null);
        else
            await CreatureCmd.GainBlock(this.Creature, new BlockVar(BlockAmount, ValueProp.Move), null);
    }
    
    #endregion State Methods
    
    private bool AreAllAlliesVanguardDefender()
    {
        return !this.Creature.CombatState!.GetTeammatesOf(this.Creature)
                    .Any((Creature c) => c.IsAlive && c != this.Creature && c.ModelId.Entry != this.Creature.ModelId.Entry);
    }
    
    private bool HasAlly()
    {
        return this.Creature.CombatState!.GetTeammatesOf(this.Creature).Any((Creature c) => c.IsAlive && c != this.Creature);
    }
    
    private int GetAllyCount()
    {
        return this.Creature.CombatState!.GetTeammatesOf(this.Creature).Count((Creature c) => c.IsAlive && c != this.Creature);
    }
    
    private Creature? GetRandomAlly()
    {
        foreach (var VARIABLE in this.Creature.CombatState!.GetTeammatesOf(this.Creature))
        {
            MainFile.Logger.Warn(VARIABLE.ModelId.Entry);
        }
        
        return Creature.CombatState.GetTeammatesOf(Creature).Where(c => c.IsAlive && c != this.Creature).TakeRandom(1, RunRng.MonsterAi).FirstOrDefault();
    }
    
    #endregion MoveStateMachine
}