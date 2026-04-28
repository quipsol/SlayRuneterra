using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters;

public class VanguardGuardsNormal : SlayRuneterraEncounterModel
{
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => false;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<VanguardCharger>(), 
                ModelDb.Monster<VanguardDefender>()
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return
        [
                    (ModelDb.Monster<VanguardCharger>().ToMutable(), null),
                    (Rng.NextItem(AllPossibleMonsters)!.ToMutable(), null),
                    (ModelDb.Monster<VanguardDefender>().ToMutable(), null)
        ];
    }

    
}