using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;

namespace SlayRuneterra.Content.Encounters.Weak;

public class PluckyPoroWeak(): CustomEncounterModel(RoomType.Monster)
{
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => true;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<PluckyPoro>() 
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return
        [
                    (ModelDb.Monster<PluckyPoro>().ToMutable(), null)
        ];
    }

    
}