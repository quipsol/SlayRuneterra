using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;
using SlayRuneterra.Utils;

namespace SlayRuneterra.Content.Monsters;

public class Lux : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 184, 174);
    public override int MaxInitialHp => MinInitialHp;

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/lux.tscn";

    #region MoveStateMachine

    private const string BEACON_OF_HOPE = "BEACON_OF_HOPE";
    private const string PRISMATIC_BARRIER = "PRISMATIC_BARRIER";
    private const string LIGHT_BINDING = "LIGHT_BINDING";
    private const string LUCENT_SINGULARITY = "LUCENT_SINGULARITY";
    private const string FINALES_FUNKELN = "FINALES_FUNKELN";
    private const string ILLUMINATION = "ILLUMINATION";
    private const string DETERMINE_NEXT_MOVE = "DETERMINE_NEXT_MOVE";

    private int PrismaticBarrierBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 15);
    private int LightBindingDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
    private int LightBindingVulnerable => 2;
    private int LucentSingularityDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 6);
    private int FinalesFunkelnDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
    private int IlluminationIncrease => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
    private int BaseIllumination => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);

    private MoveState? _beaconOfHopeState = null;
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        _beaconOfHopeState = new MoveState(BEACON_OF_HOPE, PerformBeaconOfHope, new SummonIntent());
        MoveState prismaticBarrierState = new MoveState(PRISMATIC_BARRIER, PerformPrismaticBarrier, new DefendIntent());
        MoveState lightBindingState = new MoveState(LIGHT_BINDING, PerformLightBinding, new SingleAttackIntent(LightBindingDamage), new DebuffIntent());
        MoveState lucentSingularityState = new MoveState(LUCENT_SINGULARITY, PerformLucentSingularity, new SingleAttackIntent(LucentSingularityDamage));
        MoveState finalesFunkelnState = new MoveState(FINALES_FUNKELN, PerformFinalesFunkeln, new SingleAttackIntent(FinalesFunkelnDamage));
        MoveState  illuminationState = new MoveState(ILLUMINATION, PerformIllumination, new BuffIntent());
        
        OrderInterceptMoveState determineNextMove = new(DETERMINE_NEXT_MOVE);
        determineNextMove.CanInterceptMultipleTimes = true;
        determineNextMove.SetInterceptMove(prismaticBarrierState, IsGalioAlive);
        determineNextMove.AddState(lightBindingState);
        
        _beaconOfHopeState.FollowUpState = prismaticBarrierState;
        prismaticBarrierState.FollowUpState = lightBindingState;
        lightBindingState.FollowUpState = lucentSingularityState;
        lucentSingularityState.FollowUpState = finalesFunkelnState;
        finalesFunkelnState.FollowUpState = illuminationState;
        illuminationState.FollowUpState = determineNextMove;
        
        states.Add(_beaconOfHopeState);
        states.Add(prismaticBarrierState);
        states.Add(lightBindingState);
        states.Add(lucentSingularityState);
        states.Add(finalesFunkelnState);
        states.Add(illuminationState);
        states.Add(determineNextMove);

        return new MonsterMoveStateMachine(states, illuminationState);
    }

    #region State Methods
    
    private async Task PerformLightBinding(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(LightBindingDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), targets, LightBindingVulnerable, this.Creature, null);
        await PowerCmd.Apply<IlluminationPower>(new ThrowingPlayerChoiceContext(), targets, this.Creature.GetPowerAmount<LuxIlluminationPower>(), this.Creature, null);
    }
    
    private async Task PerformLucentSingularity(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(LucentSingularityDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await PowerCmd.Apply<IlluminationPower>(new ThrowingPlayerChoiceContext(), targets, this.Creature.GetPowerAmount<LuxIlluminationPower>(), this.Creature, null);
    }
    
    private async Task PerformFinalesFunkeln(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(FinalesFunkelnDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await PowerCmd.Apply<IlluminationPower>(new ThrowingPlayerChoiceContext(), targets, this.Creature.GetPowerAmount<LuxIlluminationPower>(), this.Creature, null);
    }
    
    private async Task PerformPrismaticBarrier(IReadOnlyList<Creature> targets)
    {
        foreach (Creature creature in  Creature.CombatState!.GetTeammatesOf(Creature).Where(c => c.IsAlive))
        {
            await CreatureCmd.GainBlock(creature, new BlockVar(PrismaticBarrierBlock, ValueProp.Move), null);
        }
    }

    
    private async Task PerformBeaconOfHope(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.Add(ModelDb.Monster<Galio>().ToMutable(), this.CombatState, this.Creature.Side, "galio");
    }
    
    private async Task PerformIllumination(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<LuxIlluminationPower>(new ThrowingPlayerChoiceContext(), Creature, IlluminationIncrease, this.Creature, null);
    }

    #endregion State Methods

    private bool IsGalioAlive()
    {
        return this.Creature.CombatState!.GetTeammatesOf(this.Creature).Any( c => c.IsAlive && c.Monster is Galio);
    }
    
    #endregion MoveStateMachine

    public void SetStateToBeaconOfHope()
    {
        SetMoveImmediate(_beaconOfHopeState!);
    }
    
    public override async Task AfterAddedToRoom()
    {
        await PowerCmd.Apply<LuxIlluminationPower>(new ThrowingPlayerChoiceContext(), this.Creature, BaseIllumination, this.Creature, null);
        await PowerCmd.Apply<SummonGalioPower>(new ThrowingPlayerChoiceContext(), this.Creature, (decimal)(this.Creature.MaxHp * 0.66), this.Creature, null);
        
    }
}