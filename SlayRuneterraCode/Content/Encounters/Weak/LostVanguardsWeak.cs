using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Encounters.Weak;

public class LostVanguardsWeak(): SlayRuneterraEncounterModel()
{
    protected override bool HasCustomBackground => false;
    public override bool IsValidForAct(ActModel act) => act is Demacia;
    public override RoomType RoomType => RoomType.Monster;
    public override bool IsWeak => true;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<VanguardCharger>(), 
                ModelDb.Monster<VanguardLancer>(),
                ModelDb.Monster<VanguardRanger>(), 
                ModelDb.Monster<SilverwingVanguard>(), 
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return
        [
                    (this.Rng.NextItem<MonsterModel>(AllPossibleMonsters)!.ToMutable(), null),
                    (this.Rng.NextItem<MonsterModel>(AllPossibleMonsters)!.ToMutable(), null)
        ];
    }

    
}