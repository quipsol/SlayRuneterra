using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Soraka;


[Pool(typeof(EventRelicPool))]
public class Rejuvenate : SlayRuneterraRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private bool _hasTriggered;
    
    [SavedProperty]
    public bool HasTriggered
    {
        get =>  _hasTriggered;
        set
        {
            AssertMutable();
            _hasTriggered = value;
            if(_hasTriggered)
                Status = RelicStatus.Disabled;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (HasTriggered || room is not RestSiteRoom)
            return;
        Flash();
        await CreatureCmd.Heal(Owner.Creature, Owner.Creature.MaxHp);
        Status = RelicStatus.Disabled;
        HasTriggered = true;
    }
}