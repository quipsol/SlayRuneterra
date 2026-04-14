using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;

namespace SlayRuneterra.Content.Encounters.Elite;

public class PoppyElite(): CustomEncounterModel(RoomType.Monster)
{
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => false;

    
    public override RoomType RoomType => RoomType.Elite;
    
    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<Poppy>()
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => 
    [
                (ModelDb.Monster<Poppy>().ToMutable(), null)
    ];
    

    
}