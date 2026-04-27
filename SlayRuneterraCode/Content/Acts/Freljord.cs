using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Ancients;
using SlayRuneterra.Content.Encounters;
using SlayRuneterra.Content.Events.DemaciaAct;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Acts;

public class Freljord : SlayRuneterraActModel
{
    protected override string CustomBackgroundScenePath => "res://SlayRuneterra/scenes/acts/demacia/demacia_background.tscn";
 protected override string CustomMapTopBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_top_demacia.png";
 protected override string CustomMapMidBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_middle_demacia.png";
 protected override string CustomMapBotBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_bottom_demacia.png";
 protected override string CustomRestSiteBackgroundPath => "res://SlayRuneterra/scenes/acts/demacia/demacia_rest_site.tscn";

    public List<List<string>> BackgroundLayerPaths => 
    [
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_00_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_00_b.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_01_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_01_b.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_02_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_02_b.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_03_a.tscn", "res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_03_b.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_04_a.tscn"],
                ["res://SlayRuneterra/scenes/acts/demacia/layers/demacia_bg_05_a.tscn"],
    ];
    

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
    
    public override IEnumerable<EncounterModel> BossDiscoveryOrder => 
    [
                ModelDb.Encounter<LuxBoss>(),
                ModelDb.Encounter<GarenBoss>(),
                ModelDb.Encounter<JarvanTheFourthBoss>(),
    ];
    
    public override IEnumerable<AncientEventModel> AllAncients =>
    [
                ModelDb.AncientEvent<Zoe>(),
                ModelDb.AncientEvent<Kayle>()
    ];

    public override IEnumerable<EventModel> AllEvents =>
    [
                ModelDb.Event<Amalgamator>(),
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

                    ModelDb.Encounter<BowlbugsWeak>(),
                    ModelDb.Encounter<ThievingHopperWeak>(),
                    ModelDb.Encounter<ExoskeletonsWeak>(),
                    
                    
                    ModelDb.Encounter<BowlbugsNormal>(),
                    ModelDb.Encounter<ChompersNormal>(),
                    ModelDb.Encounter<ExoskeletonsNormal>(),
                    ModelDb.Encounter<HunterKillerNormal>(),
                    ModelDb.Encounter<LouseProgenitorNormal>(),
                    ModelDb.Encounter<MytesNormal>(),
                    ModelDb.Encounter<OvicopterNormal>(),
                    ModelDb.Encounter<SlumberingBeetleNormal>(),
                    ModelDb.Encounter<SpinyToadNormal>(),
                    ModelDb.Encounter<TheObscuraNormal>(),
                    ModelDb.Encounter<TunnelerNormal>(),

                    
                    ModelDb.Encounter<DecimillipedeElite>(),
                    ModelDb.Encounter<EntomancerElite>(),
                    ModelDb.Encounter<InfestedPrismsElite>(),
                    

                    ModelDb.Encounter<KnowledgeDemonBoss>(),
                    ModelDb.Encounter<TheInsatiableBoss>(),
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
    

    // public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    // {
    //     if (combatState.RunState.Act is not Freljord)
    //         return;
    //     
    //     if (side != CombatSide.Player || combatState.RoundNumber > 1)
    //         return;
    //
    //     foreach (var creature in combatState.Creatures)
    //     {
    //         WeakPower? power = await PowerCmd.Apply<WeakPower>(creature, 1, null,  null);
    //         if(creature.IsPlayer)
    //             power!.SkipNextDurationTick = false;
    //     }
    // }
    
    public override Task BeforeCombatStart()
    {
        MainFile.Logger.Warn("======= Freljord - Before Combat Start =======");
        return Task.CompletedTask;
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, ICombatState combatState)
    {
        MainFile.Logger.Warn("======= Freljord - Before Side Turn Start =======");
        return Task.CompletedTask;
    }
    
}