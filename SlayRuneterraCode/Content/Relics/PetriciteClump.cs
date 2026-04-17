using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics;

[Pool(typeof(EventRelicPool))]
public class PetriciteClump() : SlayRuneterraRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Event;
    public override bool ShowCounter => true;
    public override bool IsUsedUp => !Active;
    public override int DisplayAmount => Active ? 1 : 0;
    // this.InvokeDisplayAmountChanged();
    
    private bool _active = true;
    [SavedProperty]
    public bool Active
    {
        get => this._active;
        set
        {
            if(value == this._active) return;
            this.AssertMutable();
            this._active = value;
            this.DynamicVars["IsActive"].BaseValue = DisplayAmount;
            this.InvokeDisplayAmountChanged();
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("IsActive", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Energy)];

    private bool _usedThisRound = false;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (!Active || _usedThisRound || card.Owner.Creature != this.Owner.Creature || card.Type is not CardType.Skill)
            return false;
        modifiedCost = originalCost + 1M;
        return false;
    }
    
    public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        _usedThisRound = false;
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if(cardPlay.Card.Type is CardType.Skill)
            _usedThisRound = true;
        return Task.CompletedTask;
    }

    public override Task AfterCombatVictory(CombatRoom room)
    {
        Active = false;
        return Task.CompletedTask;
    }
}