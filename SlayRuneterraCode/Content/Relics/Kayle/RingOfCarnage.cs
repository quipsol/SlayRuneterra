using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Kayle;


[Pool(typeof(EventRelicPool))]
public class RingOfCarnage : SlayRuneterraRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private bool _extraEnergy = false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != this.Owner || cardPlay.Card.Type == CardType.Attack)
            return Task.CompletedTask;
        _extraEnergy = false;
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;
        if (!_extraEnergy)
        {
            _extraEnergy = true;
            return;
        }
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        _extraEnergy = false;
        return Task.CompletedTask;
    }
}