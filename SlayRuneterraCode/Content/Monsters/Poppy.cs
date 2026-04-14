using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class Poppy : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 121, 115);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 121, 119);

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/poppy.tscn";

    #region MoveStateMachine

    private const string HAMMER_SHOCK = "HAMMER_SHOCK";
    private const string IRON_AMBASSADOR = "IRON_AMBASSADOR";
    private const string STEADFAST_PRESENCE = "STEADFAST_PRESENCE";
    private const string KEEPERS_VERDICT_CHARGE = "KEEPERS_VERDICT_CHARGE";
    private const string KEEPERS_VERDICT = "KEEPERS_VERDICT";

    private int HammerShockDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
    private int HammerShockWeakAmount => 1;
    private int IronAmbassadorDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
    private int IronAmbassadorBlock => 10;
    private int SteadfastPresenceStrength => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
    private int SteadfastPresenceBlock => 10;
    private int  KeepersVerdictDazedCount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
    private int KeepersVerdictDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 25, 22);

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState hammerShockState = new MoveState(HAMMER_SHOCK, PerformHammerShock, new SingleAttackIntent(HammerShockDamage), new DebuffIntent());
        MoveState ironAmbassadorState = new MoveState(IRON_AMBASSADOR, PerformIronAmbassador, new SingleAttackIntent(IronAmbassadorDamage), new DefendIntent());
        MoveState steadfastPresenceState = new MoveState(STEADFAST_PRESENCE, PerformSteadfastPresence, new BuffIntent(), new DefendIntent());
        MoveState keepersVerdictChargeState = new MoveState(KEEPERS_VERDICT_CHARGE, PerformKeepersVerdictCharge, new BuffIntent());
        MoveState keepersVerdictState = new MoveState(KEEPERS_VERDICT, PerformKeepersVerdict, new SingleAttackIntent(KeepersVerdictDamage), new DebuffIntent());

        hammerShockState.FollowUpState = ironAmbassadorState;
        ironAmbassadorState.FollowUpState = steadfastPresenceState;
        steadfastPresenceState.FollowUpState = keepersVerdictChargeState;
        keepersVerdictChargeState.FollowUpState = keepersVerdictState;
        keepersVerdictState.FollowUpState = hammerShockState;

        states.Add(hammerShockState);
        states.Add(ironAmbassadorState);
        states.Add(steadfastPresenceState);
        states.Add(keepersVerdictChargeState);
        states.Add(keepersVerdictState);
        
        return new MonsterMoveStateMachine(states, hammerShockState);
    }

    #region State Methods

    private async Task PerformHammerShock(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(HammerShockDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await PowerCmd.Apply<WeakPower>(targets, HammerShockWeakAmount, this.Creature, null);
    }
    
    private async Task PerformIronAmbassador(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(IronAmbassadorDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await CreatureCmd.GainBlock(this.Creature, new BlockVar(IronAmbassadorBlock, ValueProp.Move), null);
    }
    
    private async Task PerformSteadfastPresence(IReadOnlyList<Creature> targets)
    {
        //Poppy poppy = this;
        //await CreatureCmd.TriggerAnim(poppy.Creature, "Cast", 0.5f);
        //SfxCmd.Play(poppy.AttackSfx);
        //VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) targets, "vfx/vfx_slime_impact");
        await PowerCmd.Apply<StrengthPower>(Creature, SteadfastPresenceStrength, Creature, null);
        await CreatureCmd.GainBlock(this.Creature, new BlockVar(SteadfastPresenceBlock, ValueProp.Move), null);
    }
    
    private async Task PerformKeepersVerdictCharge(IReadOnlyList<Creature> targets)
    {
        await Cmd.Wait(0.33f);
    }
    
    private async Task PerformKeepersVerdict(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(KeepersVerdictDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        await CardPileCmd.AddToCombatAndPreview<Dazed>(targets, PileType.Draw, KeepersVerdictDazedCount, false);
    }


    #endregion State Methods

    #endregion MoveStateMachine

    public override async Task AfterAddedToRoom()
    {
        await PowerCmd.Apply<HardenedShellPower>(Creature, 20m, Creature, null);
    }
}