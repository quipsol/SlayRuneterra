using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;

namespace SlayRuneterra.Content.Encounters.Boss;

public class GarenBoss() : CustomEncounterModel(RoomType.Boss)
{
    public override BackgroundAssets? CustomEncounterBackground(ActModel parentAct, Rng rng)
    {
        return null;
    }
     
    public override bool IsValidForAct(ActModel act) => act is Demacia;

    public override string BossNodePath =>"res://images/map/placeholder/waterfall_giant_boss_icon";
    public override MegaSkeletonDataResource? BossNodeSpineResource => null;
    
    protected override bool HasCustomBackground => false;


    public override RoomType RoomType => RoomType.Boss;
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Garen>()];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() =>
    [
                (ModelDb.Monster<Garen>().ToMutable(), null)
    ];
    
    public override float GetCameraScaling()
    {
        return 0.9f;
    }
    

}