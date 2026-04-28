using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters;

public class QuinnElite : SlayRuneterraEncounterModel
{
    
    public override string CustomScenePath => "res://SlayRuneterra/scenes/encounters/quinn_elite.tscn";
    
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => false;

    
    public override RoomType RoomType => RoomType.Elite;
    
    public override IReadOnlyList<string> Slots => ["valor", "quinn"];
    
    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<Quinn>(),
                ModelDb.Monster<Valor>(),
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => 
    [
                (ModelDb.Monster<Quinn>().ToMutable(), "quinn"),
                (ModelDb.Monster<Valor>().ToMutable(), "valor"),
    ];
    

    
}