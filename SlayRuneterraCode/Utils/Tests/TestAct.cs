using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace SlayRuneterra.Utils.Tests;
//
//
// public class TestAct() : BaseLib.Abstracts.CustomActModel(actNumber: 2)
// {
//     public override IEnumerable<EncounterModel> GenerateAllEncounters()
//     {
//         return
//         [
//                     ModelDb.Encounter<LostVanguardsWeak>(),
//                     ModelDb.Encounter<PluckyPoroWeak>(),
//                     
//                     ModelDb.Encounter<BowlbugsNormal>(),
//                     ModelDb.Encounter<ChompersNormal>(),
//                     ModelDb.Encounter<ExoskeletonsNormal>(),
//                     ModelDb.Encounter<HunterKillerNormal>(),
//                     ModelDb.Encounter<LouseProgenitorNormal>(),
//                     ModelDb.Encounter<MytesNormal>(),
//                     ModelDb.Encounter<OvicopterNormal>(),
//                     ModelDb.Encounter<SlumberingBeetleNormal>(),
//                     ModelDb.Encounter<SpinyToadNormal>(),
//                     ModelDb.Encounter<TheObscuraNormal>(),
//                     ModelDb.Encounter<TunnelerNormal>(),
//
//                     ModelDb.Encounter<QuinnElite>(),
//                     ModelDb.Encounter<PoppyElite>(),
//                     ModelDb.Encounter<XinZhaoElite>(),
//
//                     ModelDb.Encounter<GarenBoss>(),
//                     ModelDb.Encounter<JarvanTheFourthBoss>(),
//                     ModelDb.Encounter<LuxBoss>(),
//         ];
//     }
//     public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState state)
//     {
//         List<AncientEventModel> list = AllAncients.ToList();
//         if (!state.IsEpochRevealed<OrobasEpoch>())
//         {
//             list.Remove(ModelDb.AncientEvent<Orobas>());
//         }
//         return list;
//     }
//
//     public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
//     {
//         int restCount = mapRng.NextGaussianInt(6, 1, 6, 7);
//         int unknownCount = MapPointTypeCounts.StandardRandomUnknownCount(mapRng) - 1;
//         return new MapPointTypeCounts(unknownCount, restCount);
//     }
//
//     public override IEnumerable<EncounterModel> BossDiscoveryOrder =>
//     [
//                 ModelDb.Encounter<TheInsatiableBoss>(),
//                 ModelDb.Encounter<KnowledgeDemonBoss>(),
//                 ModelDb.Encounter<KaiserCrabBoss>()
//     ];
//
//     public override IEnumerable<AncientEventModel> AllAncients =>
//     [
//                 ModelDb.AncientEvent<Neow>()
//     ];
//     public override IEnumerable<EventModel> AllEvents =>
//     [
//                 ModelDb.Event<Amalgamator>(),
//                 ModelDb.Event<Bugslayer>(),
//                 ModelDb.Event<ColorfulPhilosophers>(),
//                 ModelDb.Event<ColossalFlower>(),
//                 ModelDb.Event<FieldOfManSizedHoles>(),
//                 ModelDb.Event<InfestedAutomaton>(),
//                 ModelDb.Event<LostWisp>(),
//                 ModelDb.Event<SpiritGrafter>(),
//                 ModelDb.Event<TheLanternKey>(),
//                 ModelDb.Event<ZenWeaver>()
//     ];
//     protected override string CustomMapTopBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_top_demacia.png";
//     protected override string CustomMapMidBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_middle_demacia.png";
//     protected override string CustomMapBotBgPath => "res://SlayRuneterra/images/acts/demacia/map/map_bottom_demacia.png";
//     protected override string CustomRestSiteBackgroundPath => "res://SlayRuneterra/scenes/acts/demacia/demacia_rest_site.tscn";
// }