using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters;

public class PoppyElite(): SlayRuneterraEncounterModel(RoomType.Monster)
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