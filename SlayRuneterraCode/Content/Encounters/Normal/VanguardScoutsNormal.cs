using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters.Normal;

public class VanguardScoutsNormal(): SlayRuneterraEncounterModel(RoomType.Monster)
{
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => false;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<VanguardRanger>(), 
                ModelDb.Monster<SilverwingVanguard>()
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return
        [
                    (ModelDb.Monster<VanguardLancer>().ToMutable(), null),
                    (this.Rng.NextItem(AllPossibleMonsters)!.ToMutable(), null),
                    (ModelDb.Monster<VanguardRanger>().ToMutable(), null)
        ];
    }

    
}