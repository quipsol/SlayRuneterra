using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Utils.Extensions;

namespace SlayRuneterra.Models;

public abstract class SlayRuneterraPowerModel : CustomPowerModel
{
    //Loads from DemaciaAct/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
    
    // public virtual Task BeforeCardDrawnEarly(CardModel card, PileType pileType, bool fromHandDraw)
    // {
    //     return Task.CompletedTask;
    // }
    //
    // public virtual Task BeforeCardDrawn(CardModel card, PileType pileType, bool fromHandDraw)
    // {
    //     return Task.CompletedTask;
    // }
    
    public virtual void BeforeCardDrawnEarly(CardModel card, PileType pileType, bool fromHandDraw)
    {
    }
    
    public virtual void BeforeCardDrawn(CardModel card, PileType pileType, bool fromHandDraw)
    {
    }
}