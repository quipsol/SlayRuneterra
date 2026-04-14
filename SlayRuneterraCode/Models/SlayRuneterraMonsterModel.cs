using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SlayRuneterra.Models;

public abstract class SlayRuneterraMonsterModel : CustomMonsterModel
{
    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/galio.tscn";
    public override bool HasDeathSfx => false;
    
    public virtual async Task Enrage(){}
    
}