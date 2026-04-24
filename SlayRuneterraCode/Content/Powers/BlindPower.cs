using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Afflictions;
using SlayRuneterra.Models;
using SlayRuneterra.Utils;

namespace SlayRuneterra.Content.Powers;


// TODO:
// Change to: Blind ALL cards until the end of your turn
public class BlindPower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (!Owner.IsPlayer) return;
        foreach (CardModel item in Owner.Player!.PlayerCombatState!.AllCards)
        {
            if (item.Affliction == null)
            {
                await CardCmd.Afflict<Blind>(item, 1m);
            }
        }
    }
    
    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner == Owner.Player && card.Affliction == null)
        {
            await CardCmd.Afflict<Blind>(card, 1m);
        }
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
        {
            await PowerCmd.Decrement(this);
        }
    }
    
    public override Task AfterRemoved(Creature oldOwner)
    {
        if (!Owner.IsPlayer) return Task.CompletedTask;
        foreach (CardModel item in oldOwner.Player!.PlayerCombatState!.AllCards)
        {
            if (item.Affliction is Blind)
            {
                CardCmd.ClearAffliction(item);
            }
        }
        return Task.CompletedTask;
    }
}