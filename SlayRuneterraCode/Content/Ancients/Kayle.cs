using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using SlayRuneterra.Content.Relics.Kayle;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Ancients;


public class Kayle : SlayRuneterraAncientModel
{
    public override string? CustomMapIconPath => "res://SlayRuneterra/images/placeholder/100_100/purple.png";
    public override string? CustomMapIconOutlinePath => "res://SlayRuneterra/images/placeholder/150_150/black.png";
    public override string? CustomRunHistoryIconPath => "res://SlayRuneterra/images/placeholder/100_100/white.png";
    public override string? CustomRunHistoryIconOutlinePath => "res://SlayRuneterra/images/placeholder/150_150/black.png";

    public override string? CustomBackgroundScenePath => "res://SlayRuneterra/scenes/events/background_scenes/kayle.tscn";
    
    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
    public override Color DialogueColor => new Color("3C1931");

    public override bool IsValidForAct(ActModel act) => SlayRuneterraConfig.IsEnabled;

    
    protected override OptionPools MakeOptionPools => new(
                // Divine Feather, Divine Helmet, Divine Idol
                MakePool(
                            AncientOption<DivineFeather>(),
                            AncientOption<DivineHelmet>(),
                            AncientOption<DivineIdol>() // Singeplayer
                            //AncientOption<DivineIdol>() // Multiplayer (intangible to all, no damage)
                ),
                // Judgement, Sword of Judgement, 
                MakePool(
                            AncientOption<Judgement>(weight: 20),
                            AncientOption<RingOfCarnage>(),
                            AncientOption<EssenceOfAnAngel>()
                ),
                // Essence of an Angel, Ring of Carnage,
                MakePool(
                            AncientOption<DivineIdol>()
                ));
}