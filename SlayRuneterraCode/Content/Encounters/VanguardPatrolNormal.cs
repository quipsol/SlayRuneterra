using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters;

public class VanguardPatrolNormal : SlayRuneterraEncounterModel
{
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => false;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<VanguardCharger>(),
                ModelDb.Monster<VanguardLancer>(),
                ModelDb.Monster<VanguardRanger>() 
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return
        [
                    (ModelDb.Monster<VanguardCharger>().ToMutable(), null),
                    (ModelDb.Monster<VanguardLancer>().ToMutable(), null),
                    (ModelDb.Monster<VanguardRanger>().ToMutable(), null)
        ];
    }

    
}