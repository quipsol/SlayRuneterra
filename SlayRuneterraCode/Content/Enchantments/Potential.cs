using BaseLib.Abstracts;
using SlayRuneterra.Utils.Content;

namespace SlayRuneterra.Content.Enchantments;

public class Potential : CustomEnchantmentModel
{
    protected override string CustomIconPath => "res://SlayRuneterra/images/placeholder/100_100/blue.png";
    public override bool HasExtraCardText => false;
    public override bool ShowAmount => true;
    
    protected override void OnEnchant()
    {
        ModifyMaxUpgradeLevelHandler.ModifyCustomMaxUpgradeLevel(Card,  Amount);
    }
}