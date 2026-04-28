using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters;

public class XinZhaoElite : SlayRuneterraEncounterModel
{
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override bool IsWeak => false;
    public override RoomType RoomType => RoomType.Elite;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<XinZhao>()
    ];

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() =>
    [
                (ModelDb.Monster<XinZhao>().ToMutable(), null)
    ];
}
