using System.Reflection;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Cards;


[Pool(typeof(EventCardPool))]
public class DragonsFury() : SlayRuneterraCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<DragonsFuryStrengthPower>(3),
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    

    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<DragonsFuryStrengthPower>(choiceContext, Owner.Creature, DynamicVars[nameof(DragonsFuryStrengthPower)].BaseValue, Owner.Creature, this);

        var firstOrb = Owner.PlayerCombatState!.OrbQueue.Orbs.FirstOrDefault();
        if (firstOrb == null) return;
        var newOrb = ModelDb.GetById<OrbModel>(ModelDb.GetId(firstOrb.GetType())).ToMutable();
        FieldInfo? evokeValField = AccessTools.Field(newOrb.GetType(), "_evokeVal");
        if(evokeValField != null)  evokeValField.SetValue(newOrb, evokeValField.GetValue(firstOrb));
        FieldInfo? passiveValField = AccessTools.Field(newOrb.GetType(), "_passiveVal");
        if(passiveValField != null)  passiveValField.SetValue(newOrb, passiveValField.GetValue(firstOrb));
        await OrbCmd.Channel(choiceContext, newOrb, Owner);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars[nameof(DragonsFuryStrengthPower)].UpgradeValueBy(2);
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        return base.AfterCardEnteredCombat(card);
    }
}