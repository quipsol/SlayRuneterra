using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Events.Demacia;

public class InjuredVanguard : SlayRuneterraEventModel
{
    public override string? CustomInitialPortraitPath => "res://SlayRuneterra/images/events/amalgamator.png";
    public override string? CustomBackgroundScenePath => null;
    public override string? CustomVfxPath => "";

    public override bool IsAllowed(RunState runState)
    {
        return  SlayRuneterraConfig.IsEnabled && runState.Players.All(player => player.Creature.CurrentHp > this.DynamicVars.HpLoss.BaseValue || player.Deck.Cards.Any(card => card is { IsUpgradable: true, IsUpgraded: false }));
    }

    private bool HasEnoughHealth()
    {
        return this.Owner!.Creature.CurrentHp > this.DynamicVars.HpLoss.BaseValue;
    }

    
    private bool HasUnupgradedUpgradeableCard()
    {
        return this.Owner!.Deck.Cards.Any(card => card is {IsUpgraded:false, IsUpgradable: true});
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
                new HpLossVar(8),
                new GoldVar(77),
    ];
    
    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
                    AllowOrLockOption(HasEnoughHealth, KillVanguard, []),
                    AllowOrLockOption(HasUnupgradedUpgradeableCard, HelpVanguard, []),
        ];
    }
    


    private async Task KillVanguard()
    {
        InjuredVanguard injuredVanguard = this;
        Control? container = NEventRoom.Instance?.VfxContainer;
        if (LocalContext.IsMe(injuredVanguard.Owner) && container != null)
        {
            for (int i = 0; i < 3; ++i)
            {
                Vector2 vector2_1 = new Vector2(container.Size.X * 0.25f, container.Size.Y * 0.6f);
                Vector2 vector2_2 = new Vector2(Rng.Chaotic.NextFloat(-100f, 100f), Rng.Chaotic.NextFloat(-200f, 200f));
                Node2D node2D1 = VfxCmd.PlayNonCombatVfx((Godot.Node) container, vector2_1 + vector2_2, "vfx/vfx_attack_slash")!;
                Node2D node2D2 = VfxCmd.PlayNonCombatVfx((Godot.Node) container, vector2_1 + vector2_2, "vfx/events/dense_vegetation_slice_vfx")!;
                NDebugAudioManager.Instance!.Play("slash_attack.mp3", 0.8f, PitchVariance.Medium);
                node2D1.RotationDegrees = (float) (-(double) Rng.Chaotic.NextFloat() * 180.0);
                node2D2.RotationDegrees = (float) (-(double) Rng.Chaotic.NextFloat() * 180.0);
                await Cmd.CustomScaledWait(0.2f, 0.4f);
            }
        }
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(),Owner!.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        
        LocString finalDescription = GetDescription("KILL_VANGUARD");
        SetEventFinished(finalDescription);
    }
    private Task HelpVanguard()
    {
        CardModel? chosenCard = Owner!.Deck.Cards.Where(card => card is { IsUpgradable: true, IsUpgraded: false }).TakeRandom(1, Rng).FirstOrDefault();
        if (chosenCard == null)
            MainFile.Logger.Error("InjuredVanguard event was unable to find upgradeable card. Option should have been Locked.");
        else
            CardCmd.Upgrade(chosenCard);
        
        LocString finalDescription = GetDescription("HELP_VANGUARD");
        SetEventFinished(finalDescription);
        return Task.CompletedTask;
    }

}