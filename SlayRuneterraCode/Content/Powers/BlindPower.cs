using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using SlayRuneterra.Content.Afflictions;
using SlayRuneterra.Models;
using SlayRuneterra.Utils;

namespace SlayRuneterra.Content.Powers;


// TODO:
// Change to: Blind ALL cards until the end of your turn
public class BlindPower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override void BeforeCardDrawnEarly(CardModel card, PileType pileType, bool fromHandDraw)
    {
        CardCmd.Afflict<Blind>(card, 1);
        PowerCmd.Decrement(this);
    }
}