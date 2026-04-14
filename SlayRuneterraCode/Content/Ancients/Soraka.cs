using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Relics.Soraka;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Ancients;

public class Soraka : SlayRuneterraAncientModel
{
    public override string? CustomMapIconPath => "res://SlayRuneterra/images/placeholder/100_100/purple.png";
    public override string? CustomMapIconOutlinePath => "res://SlayRuneterra/images/placeholder/150_150/black.png";
    public override string? CustomRunHistoryIconPath => "res://SlayRuneterra/images/placeholder/100_100/white.png";
    public override string? CustomRunHistoryIconOutlinePath => "res://SlayRuneterra/images/placeholder/150_150/black.png";

    public override string? CustomBackgroundScenePath => "res://SlayRuneterra/scenes/events/background_scenes/soraka.tscn";
    
    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
    public override Color DialogueColor => new Color("3C1931");

    public override bool IsValidForAct(ActModel act) => SlayRuneterraConfig.IsEnabled;

    // On the first campfire you enter, heal yourself to full hp
    protected override OptionPools MakeOptionPools => new(
                MakePool(
                            AncientOption<NeowsTorment>(),
                            AncientOption<NeowsTalisman>(),
                            AncientOption<SmallCapsule>(),
                            AncientOption<LargeCapsule>()
                ),
                MakePool(
                            AncientOption<ArcaneScroll>(weight: 10),
                            AncientOption<LeafyPoultice>(weight: 10),
                            AncientOption<PrecariousShears>(weight: 10),
                            AncientOption<GoldenPearl>(weight: 20)
                ),
                MakePool(
                            AncientOption<Rejuvenate>()
                ));
    
    
}