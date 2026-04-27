using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters;

public class PetriciteCrusherNormal(): SlayRuneterraEncounterModel()
{
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => false;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<PetriciteCrusher>()
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => 
    [
                (ModelDb.Monster<PetriciteCrusher>().ToMutable(), null)
    ];
    

    
}