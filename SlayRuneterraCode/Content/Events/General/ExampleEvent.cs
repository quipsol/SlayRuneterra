using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Events.General;

public class ExampleEvent : SlayRuneterraEventModel
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
                    // Uses method name for option key (CombineDefends => COMBINE_DEFENDS), INITIAL is also default value for the page key
                    Option(EnterByrdonisNest, [], "INITIAL"),
                    Option(EnterFakeMerchant, [], "INITIAL"),
                    Option(EnterRandomEvent, [], "INITIAL"),
        ];
    }


    private async Task EnterByrdonisNest()
    {
        // Do Stuff
        await RunManager.Instance.EnterRoom(new EventRoom(ModelDb.Event<ByrdonisNest>()));
    }

    private async Task EnterFakeMerchant()
    {
        // Do Stuff
        await RunManager.Instance.EnterRoom(new EventRoom(ModelDb.Event<FakeMerchant>()));
    }

    private async Task EnterRandomEvent()
    {
        // Do Stuff
        if (this.Owner!.RunState is not RunState runState)
        {
            MainFile.Logger.Error("RunSate was null where it shouldn't have been");
            return;
        }
        await RunManager.Instance.EnterRoom(new EventRoom(runState.Act.PullNextEvent(runState)));
    }

}