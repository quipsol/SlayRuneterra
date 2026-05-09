using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using SlayRuneterra.Content.Acts;
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
