using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class VanguardLancer : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 34, 32);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 36, 34);
    
    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/vanguard_lancer.tscn";
    
    #region MoveStateMachine
    
    private const string SINGLE = "SINGLE";
    private const string MULTI = "MULTI";
    private const string BLOCK = "BLOCK";
    
    private int SingleDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
    private int MultiDamage => 3;
    private int MultiHit => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
    private int BlockAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState singleState = new MoveState(SINGLE, Single, new SingleAttackIntent(SingleDamage));
        MoveState doubleState = new MoveState(MULTI, Multi, new MultiAttackIntent(MultiDamage, MultiHit));
        MoveState blockState = new MoveState(BLOCK, Block, new DefendIntent());
        
        singleState.FollowUpState = blockState;
        doubleState.FollowUpState = singleState;
        blockState.FollowUpState = doubleState;
        states.Add(singleState);
        states.Add(doubleState);
        states.Add(blockState);
        
        return new MonsterMoveStateMachine(states, doubleState);
    }
    
    #region State Methods
    
    private async Task Single(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(SingleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task Multi(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(MultiDamage)
                    .FromMonster(this)
                    .WithAttackerAnim("Attack", 0.3f)
                    .WithHitCount(MultiHit)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task Block(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.GainBlock(this.Creature, new BlockVar(BlockAmount, ValueProp.Move), null);
    }
    
    #endregion State Methods
    
    #endregion MoveStateMachine
}