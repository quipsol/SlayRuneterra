using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Acts;
using SlayRuneterra.Content.Monsters;

namespace SlayRuneterra.Content.Encounters.Boss;

public class JarvanTheFourthBoss() : CustomEncounterModel(RoomType.Boss)
{
    public override BackgroundAssets? CustomEncounterBackground(ActModel parentAct, Rng rng)
    {
        return null;
    }
     
    public override bool IsValidForAct(ActModel act) => act is Demacia;

    public override string BossNodePath =>"res://images/map/placeholder/waterfall_giant_boss_icon";
    public override MegaSkeletonDataResource? BossNodeSpineResource => null;
    
    protected override bool HasCustomBackground => false;

    // this order is also attack order!
    public override IReadOnlyList<string> Slots => ["vanguard3", "vanguard2", "vanguard1", "jarvan"];

    public override RoomType RoomType => RoomType.Boss;
    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
    [
                ModelDb.Monster<JarvanTheFourth>(), 
                ModelDb.Monster<VanguardCharger>(), 
                ModelDb.Monster<VanguardLancer>(), 
                ModelDb.Monster<VanguardRanger>(), 
                ModelDb.Monster<VanguardDefender>(), 
                ModelDb.Monster<SilverwingVanguard>(),
    ];
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return 
        [
                    (ModelDb.Monster<JarvanTheFourth>().ToMutable(), "jarvan")
        ];
    }
    
    public override float GetCameraScaling()
    {
        return 0.9f;
    }
    
    
}