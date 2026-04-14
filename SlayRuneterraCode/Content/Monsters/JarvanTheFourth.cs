using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class JarvanTheFourth : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 180, 170);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 190, 180);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/jarvan.tscn";

    #region MoveStateMachine

    private const string RALLY = "RALLY";
    private const string RAISE_MORALE = "RAISE_MORALE";
    private const string CUTTHROAT = "CUTTHROAT";
    private const string SLAM = "SLAM";
    private const string MOVE_BRANCHES = "MOVE_BRANCHES";

    private IReadOnlyList<MonsterModel> SummonableVanguards =>
    [
                ModelDb.Monster<VanguardCharger>(), 
                ModelDb.Monster<VanguardLancer>(),
                ModelDb.Monster<VanguardRanger>(), 
                ModelDb.Monster<VanguardDefender>(), 
                ModelDb.Monster<SilverwingVanguard>(),
    ];
    
    private int SlamDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 8);
    private int CutthroatDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
    private int CutthroatVulnerable => 2;
    private int RallySummonCount => 1;
    private int RallyBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 8);
    private int RaiseMoraleStrength => 2;

    private bool _lastMoveRally = false;
    private bool _lastMoveRaiseMorale = false;
    private bool _lastMoveCutthroat = false;
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState rallyState = new MoveState(RALLY, PerformRally, new SummonIntent());
        MoveState raiseMoraleState = new MoveState(RAISE_MORALE, PerformRaiseMorale, new BuffIntent());
        MoveState cutthroatState = new MoveState(CUTTHROAT, PerformCutthroat, new SingleAttackIntent(CutthroatDamage), new DebuffIntent());
        MoveState slamState = new MoveState(SLAM, PerformSlam, new SingleAttackIntent(SlamDamage));
        
        ConditionalBranchState determineNextMoveState = new ConditionalBranchState(MOVE_BRANCHES);
        determineNextMoveState.AddState(rallyState, ShouldRally);
        determineNextMoveState.AddState(raiseMoraleState, ShouldRaiseMorale);
        determineNextMoveState.AddState(cutthroatState, ShouldCutthroat);
        determineNextMoveState.AddState(slamState, ShouldSlam);

        
        rallyState.FollowUpState = determineNextMoveState;
        raiseMoraleState.FollowUpState = determineNextMoveState;
        cutthroatState.FollowUpState = determineNextMoveState;
        slamState.FollowUpState = determineNextMoveState;

        states.Add(rallyState);
        states.Add(raiseMoraleState);
        states.Add(cutthroatState);
        states.Add(slamState);
        states.Add(determineNextMoveState);
        
        return new MonsterMoveStateMachine(states, rallyState);
    }

    #region State Methods

    private async Task PerformRally(IReadOnlyList<Creature> targets)
    {
        for (int i = 0; i < RallySummonCount; i++)
        {
            string slotName = this.CombatState.Encounter!.Slots.LastOrDefault((string s) => base.CombatState.Enemies.All((Creature c) => c.SlotName != s), string.Empty);
            var creature = await CreatureCmd.Add(this.RunRng.MonsterAi.NextItem(SummonableVanguards)!.ToMutable(), this.CombatState, this.Creature.Side, slotName);
            await PowerCmd.Apply<MinionPower>(creature, 1m, this.Creature, null);
        }
        await CreatureCmd.GainBlock(this.Creature, new BlockVar(RallyBlock, ValueProp.Move), null);
        _lastMoveRally = true;
        _lastMoveRaiseMorale = false;
        _lastMoveCutthroat = false;
    }

    private async Task PerformRaiseMorale(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<StrengthPower>(this.Creature.CombatState!.GetTeammatesOf(this.Creature).Where(creature => creature.IsAlive ), 
                    RaiseMoraleStrength, this.Creature, null);
        _lastMoveRally = false;
        _lastMoveRaiseMorale = true;
        _lastMoveCutthroat = false;
    }
    
    private async Task PerformCutthroat(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(CutthroatDamage)
                    .FromMonster(this)
                    .WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await PowerCmd.Apply<VulnerablePower>(targets, CutthroatVulnerable, this.Creature, null);
        _lastMoveRally = false;
        _lastMoveRaiseMorale = false;
        _lastMoveCutthroat = true;
    }
    
    
    private async Task PerformSlam(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(SlamDamage)
                    .FromMonster(this)
                    .WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        _lastMoveRally = false;
        _lastMoveRaiseMorale = false;
        _lastMoveCutthroat = false;
    }

    #endregion State Methods

    private bool ShouldRally()
    {
        int allyCount = GetAllyCount();
        if (_lastMoveRally || allyCount >= 3)
            return false;
        if (allyCount == 0)
            return RunRng.MonsterAi.NextInt(100) <= 66;
        if(_lastMoveCutthroat || _lastMoveRaiseMorale)
            return RunRng.MonsterAi.NextInt(100) <= 40;
        return RunRng.MonsterAi.NextInt(100) <= 33;
    }

    private bool ShouldRaiseMorale()
    {
        int allyCount = GetAllyCount();
        if (_lastMoveRaiseMorale || allyCount == 0)
            return false;
        if (allyCount == 3)
            if(_lastMoveCutthroat)
                return RunRng.MonsterAi.NextInt(100) <= 50;
            else
                return RunRng.MonsterAi.NextInt(100) <= 33;
            
        if(_lastMoveCutthroat)
            return RunRng.MonsterAi.NextInt(100) <= 20;
        if(_lastMoveRally)
            return RunRng.MonsterAi.NextInt(100) <= 25;
        
        return RunRng.MonsterAi.NextInt(100) <= 16;
    }

    private bool ShouldCutthroat()
    {
        int allyCount = GetAllyCount();
        if (_lastMoveCutthroat || allyCount == 0)
            return false;
        if (allyCount == 3)
            return true; // when you have 3 allies, never slam
            
        if(_lastMoveRaiseMorale)
            return RunRng.MonsterAi.NextInt(100) <= 20;
        if(_lastMoveRally)
            return RunRng.MonsterAi.NextInt(100) <= 25;
        
        return RunRng.MonsterAi.NextInt(100) <= 16;
    }

    private bool ShouldSlam()
    {
        return true;
    }
    
    
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
        foreach (var creature in this.Creature.CombatState!.GetTeammatesOf(this.Creature))
        {
            MainFile.Logger.Warn(creature.ModelId.Entry);
        }
        
        return this.Creature.CombatState.GetTeammatesOf(this.Creature).Where((Creature c) => c.IsAlive && c != this.Creature).TakeRandom(1, this.RunRng.MonsterAi).FirstOrDefault();
    }
    
    #endregion MoveStateMachine
}