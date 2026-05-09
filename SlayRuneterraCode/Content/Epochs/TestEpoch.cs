using MegaCrit.Sts2.Core.Timeline;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Epochs;

public class TestEpoch : SlayRuneterraEpochModel
{
    public override string Id => "SLAYRUNETERRA-TEST_EPOCH";
    public override EpochEra Era => EpochEra.Blight0;
    public override int EraPosition => 1;
    public override string StoryId => "Magnum_Opus";

    public override string CustomPackedPortraitPath => "res://SlayRuneterra/scenes/tests/test_epoch_portrait.tres";
    public override string CustomBigPortraitPath => "res://SlayRuneterra/images/placeholder/150_150/white.png";
}