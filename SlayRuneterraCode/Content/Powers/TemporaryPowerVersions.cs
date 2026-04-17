using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models.Powers;
using SlayRuneterra.Content.Cards;
using SlayRuneterra.Content.Relics.Kayle;

namespace SlayRuneterra.Content.Powers;



public class DragonsFuryStrengthPower : CustomTemporaryPowerModelWrapper<DragonsFury, StrengthPower>
{
    protected override int LastForXExtraTurns => 1;
}

public class SwordOfJusticeStrengthPower : CustomTemporaryPowerModelWrapper<SwordOfJustice, StrengthPower>
{
}