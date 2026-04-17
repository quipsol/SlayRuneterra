using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Content.Cards;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Events.General;

public class DragonsDen : SlayRuneterraEventModel
{
    public override string? CustomInitialPortraitPath => "res://SlayRuneterra/images/events/amalgamator.png";
    public override string? CustomBackgroundScenePath => null;
    public override string? CustomVfxPath => "";

    public override bool IsAllowed(IRunState runState)
    {
        return SlayRuneterraConfig.IsEnabled;
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
                new StringVar("Relic", ModelDb.Relic<TungstenRod>().Title.GetFormattedText()),
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
                    Option(ObserveTeeth, HoverTipFactory.FromCardWithCardHoverTips<TwinBite>(), "INITIAL"),
                    Option(ObserveTemper, HoverTipFactory.FromCardWithCardHoverTips<DragonsFury>(), "INITIAL"),
        ];
    }


    private async Task ObserveTeeth()
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<TwinBite>(Owner), PileType.Deck), 2f);
        SetEventFinished(GetDescription("LEAVE"));
    }

    private async Task ObserveTemper()
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<DragonsFury>(Owner), PileType.Deck), 2f);
        SetEventFinished(GetDescription("LEAVE"));
    }

}