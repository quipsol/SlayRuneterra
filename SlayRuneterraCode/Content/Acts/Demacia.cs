using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using SlayRuneterra.Content.Ancients;
using SlayRuneterra.Content.Encounters;
using SlayRuneterra.Content.Events.DemaciaAct;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;
using SlayRuneterra.Utils.Tests;
using ManualBackgroundAssets = SlayRuneterra.Utils.ManualBackgroundAssets;

namespace SlayRuneterra.Content.Acts;

public sealed class Demacia : SlayRuneterraActModel
{


    #region Assets
    
    public override string? CustomChestScene => "res://SlayRuneterra/animations/backgrounds/treasure_room/test_anim.tscn";


    protected override BackgroundAssets CustomGenerateBackgroundAssets(Rng rng)
    {
        return  new ManualBackgroundAssets(GetBackgroundAssetPaths(rng));
    }
    
    protected override string CustomBackgroundScenePath => "res://SlayRuneterra/scenes/acts/demacia/demacia_background.tscn";
    //protected override string CustomBackgroundScenePath => "res://BaseLib/scenes/dynamic_background.tscn";
    protected override string CustomMapTopBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_top_demacia.png";
    protected override string CustomMapMidBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_middle_demacia.png";
    protected override string CustomMapBotBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_bottom_demacia.png";
    protected override string CustomRestSiteBackgroundPath => "res://SlayRuneterra/scenes/acts/demacia/demacia_rest_site.tscn";


    public List<string> ForegroundLayerPaths =>
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_fg_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_fg_b.tscn"];
    
    public List<List<string>> BackgroundLayerPaths => 
    [
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_00_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_00_b.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_01_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_01_b.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_02_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_02_b.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_03_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_03_b.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_04_a.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_05_a.tscn"],
    ];

    public (string, List<string>, string?) GetBackgroundAssetPaths(Rng rng)
    {
        bool daytime = rng.NextBool();
        string foreground = ForegroundLayerPaths[daytime ? 0 : 1];
        List<string> layers = new();
        bool isBackground = true;
        foreach (var layerPaths in BackgroundLayerPaths)
        {
            if (isBackground)
            {
                layers.Add(layerPaths[daytime ? 0 : 1]);
                isBackground = false;
                continue;
            }
            layers.Add(rng.NextItem(layerPaths) ?? "");
        }
        return (CustomBackgroundScenePath, layers, foreground);
    }
    
    #endregion Assets

    public override Color MapTraveledColor => new Color("27221C");
    public override Color MapUntraveledColor => new Color("6E7750");
    public override Color MapBgColor => new Color("9B9562");
    
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
        return
        [
                    ModelDb.Encounter<LostVanguardsWeak>(),
                    ModelDb.Encounter<PluckyPoroWeak>(),
                    // ModelDb.Encounter<BowlbugsWeak>(),
                    // ModelDb.Encounter<ThievingHopperWeak>(),
                    // ModelDb.Encounter<ExoskeletonsWeak>(),

                    ModelDb.Encounter<VanguardScoutsNormal>(),
                    ModelDb.Encounter<VanguardPatrolNormal>(),
                    ModelDb.Encounter<VanguardGuardsNormal>(),
                    ModelDb.Encounter<GreathornElkNormal>(),
                    ModelDb.Encounter<PetriciteGolemNormal>(),
                    ModelDb.Encounter<PetriciteCrusherNormal>(),
                    ModelDb.Encounter<WaspNestNormal>(),
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
                    ModelDb.Encounter<XinZhaoElite>(),

                    ModelDb.Encounter<GarenBoss>(),
                    ModelDb.Encounter<JarvanTheFourthBoss>(),
                    ModelDb.Encounter<LuxBoss>(),
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