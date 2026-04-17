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
using SlayRuneterra.Content.Relics;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Events.DemaciaAct;

public class PetriciteValley : SlayRuneterraEventModel
{
    public override string? CustomInitialPortraitPath => "res://SlayRuneterra/images/events/amalgamator.png";
    public override string? CustomBackgroundScenePath => null;
    public override string? CustomVfxPath => "";

    public override bool IsAllowed(IRunState runState)
    {
        return SlayRuneterraConfig.IsEnabled;
    }
    
    private bool HasEnoughHealth()
    {
        return this.Owner!.Creature.CurrentHp > this.DynamicVars.HpLoss.BaseValue;
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
                new HpLossVar(15),
    ];
    
    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return 
        [
                    Option(MarchOn, [], "INITIAL"),
                    AllowOrLockOption(HasEnoughHealth, WalkAround, [])
        ];
    }
    
    
    private async Task MarchOn()
    {
        await RelicCmd.Obtain<PetriciteClump>(this.Owner!);
        LocString finalDescription = GetDescription("MARCH_ON");
        SetEventFinished(finalDescription);
    }
    
    private async Task WalkAround()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(),Owner!.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        LocString finalDescription = GetDescription("WALK_AROUND");
        SetEventFinished(finalDescription);
    }
 

}