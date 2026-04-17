using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Rooms;

namespace SlayRuneterra.Models;

public abstract class SlayRuneterraEncounterModel(RoomType roomType = RoomType.Monster) : CustomEncounterModel(roomType)
{
    protected override bool HasCustomBackground => false;
}