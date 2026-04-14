using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Afflictions;

public class Blind : CustomAfflictionModel
{
    public override bool HasExtraCardText => true;
    
    public override void AfterApplied()
    { }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if(cardPlay.Card.Affliction is Blind)
            CardCmd.ClearAffliction(cardPlay.Card);
        return Task.CompletedTask;
    }

    public override Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if(card.Affliction is Blind)
            CardCmd.ClearAffliction(card);
        return Task.CompletedTask;
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        foreach (Player player in CombatState.Players)
        {
            foreach (CardPile pile in player.Piles)
            {
                foreach (CardModel card in pile.Cards)
                {
                    if(card.Affliction is Blind)
                        CardCmd.ClearAffliction(card);
                }
            } 
        }
           
        return Task.CompletedTask;
    }
}