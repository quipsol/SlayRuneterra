using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Cards.Ancient;

[Pool(typeof(ColorlessCardPool))]
public class CelestialBlessing() : SlayRuneterraCardModel(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    public override string BetaPortraitPath => "res://SlayRuneterra/images/card_portraits/colorless_ancient_placeholder.png";
    public override string PortraitPath => "res://SlayRuneterra/images/card_portraits/colorless_ancient_placeholder.png";
    public override string CustomPortraitPath => "res://SlayRuneterra/images/card_portraits/colorless_ancient_placeholder.png";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
                new HealVar(5),
                new CardsVar(2)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}