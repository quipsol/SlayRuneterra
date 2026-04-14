using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;

namespace SlayRuneterra.Content.Encounters.Normal;

public class GreathornElkNormal(): CustomEncounterModel(RoomType.Monster)
{
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => false;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<GreathornElk>()
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => 
        [
                    (ModelDb.Monster<GreathornElk>().ToMutable(), null)
        ];
    

    
}