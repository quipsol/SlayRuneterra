using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;

namespace SlayRuneterra.Content.Encounters.Elite;

public class QuinnElite(): CustomEncounterModel(RoomType.Monster)
{
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