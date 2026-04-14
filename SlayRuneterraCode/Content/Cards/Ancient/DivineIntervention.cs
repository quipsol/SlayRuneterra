using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Cards.Ancient;

[Pool(typeof(ColorlessCardPool))]
public class DivineIntervention() : SlayRuneterraCard(3, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    public override string BetaPortraitPath => "res://SlayRuneterra/images/card_portraits/colorless_ancient_placeholder.png";
    public override string PortraitPath => "res://SlayRuneterra/images/card_portraits/colorless_ancient_placeholder.png";
    public override string CustomPortraitPath => "res://SlayRuneterra/images/card_portraits/colorless_ancient_placeholder.png";

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.SingleplayerOnly;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
                new PowerVar<IntangiblePower>(1), 
                new PowerVar<DivineInterventionPower>(20)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IntangiblePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<IntangiblePower>(Owner.Creature, DynamicVars[nameof(IntangiblePower)].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<DivineInterventionPower>(Owner.Creature, DynamicVars[nameof(DivineInterventionPower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(DivineInterventionPower)].UpgradeValueBy(10);
    }
}