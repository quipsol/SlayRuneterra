using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Utils.Content;

namespace SlayRuneterra.Content.Enchantments;

public class Potential : CustomEnchantmentModel, IModifyMaxUpgradeLevel
{
    protected override string CustomIconPath => "res://SlayRuneterra/images/placeholder/100_100/blue.png";
    public override bool HasExtraCardText => false;
    public override bool ShowAmount => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Upgradeability", 1)];
    
    public int ModifyMaxUpgradeLevelHook(CardModel card, int amount)
    {
        return amount;//card == Card ? amount + Amount : amount;
    }

    protected override void OnEnchant()
    {
        ModifyMaxUpgradeLevelHandler.ModifyCustomMaxUpgradeLevel(Card,  Amount);
    }
}