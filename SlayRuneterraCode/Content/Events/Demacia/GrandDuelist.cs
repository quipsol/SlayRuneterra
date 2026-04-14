using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Events.Demacia;

public class GrandDuelist : SlayRuneterraEventModel
{
    public override string? CustomInitialPortraitPath => "res://SlayRuneterra/images/events/amalgamator.png";
    public override string? CustomBackgroundScenePath => null;
    public override string? CustomVfxPath => "";

    public override bool IsAllowed(RunState runState)
    {
        return SlayRuneterraConfig.IsEnabled;
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
                new GoldVar("Duelist", 20),
                new GoldVar("Challenger", 45),
                new GoldVar("Yourself", 67),
                new HpLossVar(7),
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
                    Option(BetOnDuelist, []),
                    Option(BetOnChallenger, []),
                    AllowOrLockOption(() => Owner!.Creature.CurrentHp > DynamicVars.HpLoss.BaseValue, ChallengeYourself, [])
        ];
    }


    private async Task BetOnDuelist()
    {
        LocString finalDescription;
        int a = Rng.NextInt(100);
        MainFile.Logger.Warn(a.ToString());
        if (a < 70)
        {
            await PlayerCmd.GainGold(DynamicVars["Duelist"].BaseValue, Owner!);
            finalDescription = GetDescription("BET_ON_DUELIST", "WON");
        }
        else
        {
            finalDescription = GetDescription("BET_ON_DUELIST", "LOST");
        }
        SetEventFinished(finalDescription);
    }
    private async Task BetOnChallenger()
    {
        LocString finalDescription;
        if (Rng.NextInt(100) < 30)
        {
            await PlayerCmd.GainGold(DynamicVars["Challenger"].BaseValue, Owner!);
            finalDescription = GetDescription("BET_ON_CHALLENGER", "WON");
        }
        else
        {
            finalDescription = GetDescription("BET_ON_CHALLENGER", "LOST");
        }
        SetEventFinished(finalDescription);
    }
    private async Task ChallengeYourself()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(),Owner!.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        await PlayerCmd.GainGold(DynamicVars["Yourself"].BaseValue, Owner!);
        LocString finalDescription = GetDescription("CHALLENGE_YOURSELF");
        SetEventFinished(finalDescription);
    }


}