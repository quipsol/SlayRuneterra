using MegaCrit.Sts2.Core.Combat;
using SlayRuneterra.Utils.HookInterface;

namespace SlayRuneterra.Models;

public abstract class SlayRuneterraActModel : CustomActModel ,IHookReceiver
{
    public override bool ShouldReceiveCombatHooks => true;
    
    public bool ReceiveHooks(CombatState combatState)
    {
        return combatState.RunState.Act.Id.Entry == this.Id.Entry;
    }
    
    protected override string CustomBackgroundScenePath => $"res://{MainFile.ModId}/scenes/backgrounds/{Id.Entry.ToLowerInvariant()}/{Id.Entry.ToLowerInvariant()}_background.tscn";
    protected override string CustomMapTopBgPath => $"res://{MainFile.ModId}/images/packed/map/map_bgs/{Id.Entry.ToLowerInvariant()}/map_bottom_{Id.Entry.ToLowerInvariant()}.png";
    protected override string CustomMapMidBgPath => $"res://{MainFile.ModId}/images/packed/map/map_bgs/{Id.Entry.ToLowerInvariant()}/map_middle_{Id.Entry.ToLowerInvariant()}.png";
    protected override string CustomMapBotBgPath => $"res://{MainFile.ModId}/images/packed/map/map_bgs/{Id.Entry.ToLowerInvariant()}/map_top_{Id.Entry.ToLowerInvariant()}.png";
    protected override string CustomRestSiteBackgroundPath => $"res://{MainFile.ModId}/scenes/rest_site/{Id.Entry.ToLowerInvariant()}_rest_site.tscn";

}