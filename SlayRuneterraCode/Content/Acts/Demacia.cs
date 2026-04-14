using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using SlayRuneterra.Content.Encounters.Elite;
using SlayRuneterra.Content.Ancients;
using SlayRuneterra.Content.Encounters.Boss;
using SlayRuneterra.Content.Encounters.Normal;
using SlayRuneterra.Content.Encounters.Weak;
using SlayRuneterra.Content.Events.Demacia;
using SlayRuneterra.Models;
using LostWisp = MegaCrit.Sts2.Core.Models.Events.LostWisp;

namespace SlayRuneterra.Content.Acts;

public sealed class Demacia : CustomActModel
{
    public override string? CustomBackgroundScenePath => "res://SlayRuneterra/scenes/acts/demacia/demacia_background.tscn";
    public override string? CustomMapTopBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_top_demacia.png";
    public override string? CustomMapMidBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_middle_demacia.png";
    public override string? CustomMapBotBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_bottom_demacia.png";
    public override string? CustomRestSiteBackgroundPath => "res://SlayRuneterra/scenes/acts/demacia/demacia_rest_site.tscn";

    public override Dictionary<string, List<string?>> LayerPaths => new() 
    { 
                ["background"] = ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_00_a.tscn"], 
                ["one"] = ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_01_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_01_b.tscn"],
                ["two"] = ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_02_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_02_b.tscn"],
                ["three"] = ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_03_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_03_b.tscn"],
                ["four"] = ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_04_a.tscn"],
                ["five"] = ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_05_a.tscn"],
                ["foreground"] = ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_fg_a.tscn"],
    };
    

    public override Color MapTraveledColor  => new Color("27221C");
    public override Color MapUntraveledColor => new Color("6E7750");
    public override Color MapBgColor => new Color("9B9562");
    
    public override string AmbientSfx => "event:/sfx/ambience/act2_ambience";
    public override string ChestOpenSfx  => "event:/sfx/ui/treasure/treasure_act2";
    public override string[] BgMusicOptions  => ["event:/music/act2_a1_v2", "event:/music/act2_a2_v2"];
    public override string[] MusicBankPaths => ["res://banks/desktop/act2_a1.bank", "res://banks/desktop/act2_a2.bank"];
    
    protected override int BaseNumberOfRooms  => 14;
    protected override int NumberOfWeakEncounters => 3; // optional override

    
    
    public override string ChestSpineResourcePath => "res://animations/backgrounds/treasure_room/chest_room_act_2_skel_data.tres";
    
    // No idea how to handle animations yet
    //public override string ChestSpineResourcePath => "res://SlayRuneterra/animations/backgrounds/treasure_room/chest_room_demacia_skel_data.tres";
    
    public override string ChestSpineSkinNameNormal => "act2";
    public override string ChestSpineSkinNameStroke => "act2_stroke";
    
    public override IEnumerable<EncounterModel> BossDiscoveryOrder => 
    [
                ModelDb.Encounter<LuxBoss>(),
                ModelDb.Encounter<GarenBoss>(),
                ModelDb.Encounter<JarvanTheFourthBoss>(),
    ];
    
    public override IEnumerable<AncientEventModel> AllAncients =>
    [
                ModelDb.AncientEvent<Soraka>()
    ];

    public override IEnumerable<EventModel> AllEvents =>
    [
                ModelDb.Event<PetriciteValley>(),
                ModelDb.Event<InjuredVanguard>(),
                ModelDb.Event<AncientOak>(),
                
                
                //ModelDb.Event<Amalgamator>(),
                ModelDb.Event<Bugslayer>(),
                ModelDb.Event<ColorfulPhilosophers>(),
                ModelDb.Event<ColossalFlower>(),
                ModelDb.Event<FieldOfManSizedHoles>(),
                ModelDb.Event<InfestedAutomaton>(),
                ModelDb.Event<LostWisp>(),
                ModelDb.Event<SpiritGrafter>(),
                ModelDb.Event<TheLanternKey>(),
                ModelDb.Event<ZenWeaver>()
    ];
    
    
    public override IEnumerable<EncounterModel> GenerateAllEncounters()
    {
        return [ 
                    ModelDb.Encounter<LostVanguardsWeak>(),
                    ModelDb.Encounter<PluckyPoroWeak>(),
                    // ModelDb.Encounter<BowlbugsWeak>(),
                    // ModelDb.Encounter<ThievingHopperWeak>(),
                    // ModelDb.Encounter<ExoskeletonsWeak>(),
                    
                    ModelDb.Encounter<VanguardScoutsNormal>(),
                    ModelDb.Encounter<VanguardPatrolNormal>(),
                    ModelDb.Encounter<VanguardGuardsNormal>(),
                    ModelDb.Encounter<GreathornElkNormal>(),
                    // ModelDb.Encounter<BowlbugsNormal>(),
                    // ModelDb.Encounter<ChompersNormal>(),
                    // ModelDb.Encounter<ExoskeletonsNormal>(),
                    // ModelDb.Encounter<HunterKillerNormal>(),
                    // ModelDb.Encounter<LouseProgenitorNormal>(),
                    // ModelDb.Encounter<MytesNormal>(),
                    // ModelDb.Encounter<OvicopterNormal>(),
                    // ModelDb.Encounter<SlumberingBeetleNormal>(),
                    // ModelDb.Encounter<SpinyToadNormal>(),
                    // ModelDb.Encounter<TheObscuraNormal>(),
                    // ModelDb.Encounter<TunnelerNormal>(),
                    
                    ModelDb.Encounter<QuinnElite>(),
                    ModelDb.Encounter<PoppyElite>(),
                    // ModelDb.Encounter<DecimillipedeElite>(),
                    // ModelDb.Encounter<EntomancerElite>(),
                    // ModelDb.Encounter<InfestedPrismsElite>(),
                    
                    ModelDb.Encounter<GarenBoss>(),
                    ModelDb.Encounter<JarvanTheFourthBoss>(),
                    ModelDb.Encounter<LuxBoss>(),
                    // ModelDb.Encounter<KnowledgeDemonBoss>(),
                    // ModelDb.Encounter<TheInsatiableBoss>(),
        ];
    }

    public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
    {
        List<AncientEventModel> list = AllAncients.ToList();
        if (!unlockState.IsEpochRevealed<OrobasEpoch>())
        {
            list.Remove(ModelDb.AncientEvent<Orobas>());
        }
        return list;
    }

    protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState) { }

    public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
    {
        int restCount = mapRng.NextGaussianInt(6, 1, 6, 7);
        int unknownCount = MapPointTypeCounts.StandardRandomUnknownCount(mapRng) - 1;
        return new MapPointTypeCounts(unknownCount, restCount);
    }
    
}