using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters;

public class WaspNestNormal : SlayRuneterraEncounterModel
{
    
    public override string CustomScenePath => "res://SlayRuneterra/scenes/encounters/wasp_nest_normal.tscn";

    
    public override bool IsValidForAct(ActModel act) => act is Demacia;

    // this order is also attack order!
    public override IReadOnlyList<string> Slots => ["wasp3", "wasp2", "wasp1", "wasp_nest"];

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<WaspNest>(), 
                ModelDb.Monster<Wasp>(), 
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return 
        [
                    (ModelDb.Monster<WaspNest>().ToMutable(), "wasp_nest"),
                    (ModelDb.Monster<Wasp>().ToMutable(), "wasp1"),
                    (ModelDb.Monster<Wasp>().ToMutable(), "wasp2"),
        ];
    }

 
}