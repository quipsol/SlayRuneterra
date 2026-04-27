using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Models;
using SlayRuneterra.Utils.Content;

namespace SlayRuneterra.Content.Enchantments;

public class Potential : CustomEnchantmentModel, IModifyMaxUpgradeLevel
{
    protected override string? CustomIconPath => "res://SlayRuneterra/images/placeholder/100_100/blue.png";
    public override bool HasExtraCardText => false;
    public override bool ShowAmount => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Upgradeability", 1)];

    public int ModifyMaxUpgradeLevel(int amount)
    {
        MainFile.Logger.Info($"Modifying MaxUpgardeLevel by {Amount}");
        return amount + Amount;
    }
    
    public int ModifyMaxUpgradeLevelHook(CardModel card, int amount)
    {
        MainFile.Logger.Info($"Modifying MaxUpgardeLevel by {Amount}");
        return amount + Amount;
    }

    protected override void OnEnchant()
    {
       this.ModifyMaxUpgradeLevelOfCard(Card); 
        // if(Card is SlayRuneterraCardModel srModel)
        //     srModel.IncreaseMaxUpgradeLevel(Amount);
        
    }

}