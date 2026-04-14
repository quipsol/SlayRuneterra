using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Events.Demacia;

public class AncientOak : SlayRuneterraEventModel
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
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
                    Option(CloseYourEyes, [], "INITIAL"),
                    Option(Withstand, [], "INITIAL"),
        ];
    }


    private async Task CloseYourEyes()
    {
        List<CardModel?> chosenCards = (await CardSelectCmd.FromDeckForTransformation(Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 2))).ToList();
        foreach (CardModel? chosenCard in chosenCards)
        {
            if (chosenCard == null)
                MainFile.Logger.Warn("AncientOak event was unable to find upgradeable card.");
            else
                await CardCmd.TransformToRandom(chosenCard, Rng, CardPreviewStyle.EventLayout);
        }
        
        
        LocString finalDescription = GetDescription("CLOSE_YOUR_EYES");
        SetEventFinished(finalDescription);
    }
    private async Task Withstand()
    {
        MainFile.Logger.Info("TODO");
        List<CardModel?> chosenCards = Owner!.Deck.Cards.Where(card => card is { IsUpgradable: true, IsUpgraded: false }).TakeRandom(2, Rng).ToList()!;
        foreach (CardModel? chosenCard in chosenCards)
        {
            if (chosenCard == null)
                MainFile.Logger.Warn("AncientOak event was unable to find upgradeable card.");
            else
                CardCmd.Upgrade(chosenCard);
        }

        LocString finalDescription = GetDescription("WITHSTAND");
        SetEventFinished(finalDescription);
    }


}