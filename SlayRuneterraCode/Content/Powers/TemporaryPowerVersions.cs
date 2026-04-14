using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models.Powers;
using SlayRuneterra.Content.Cards;

namespace SlayRuneterra.Content.Powers;



public class DragonsFuryTemporaryStrengthPower : CustomTemporaryPowerModelWrapper<DragonsFury, StrengthPower>
{
    protected override int LastForXExtraTurns => 1;
}