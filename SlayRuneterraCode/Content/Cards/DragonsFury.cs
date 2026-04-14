using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Cards;


[Pool(typeof(EventCardPool))]
public class DragonsFury() : SlayRuneterraCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<DragonsFuryTemporaryStrengthPower>(4),
        new PowerVar<ArtifactPower>(2)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<DragonsFuryTemporaryStrengthPower>(Owner.Creature, DynamicVars[nameof(DragonsFuryTemporaryStrengthPower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(DragonsFuryTemporaryStrengthPower)].UpgradeValueBy(2);
    }
}