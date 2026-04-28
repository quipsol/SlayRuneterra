using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using SlayRuneterra.Content.Cards.Ancient;
using SlayRuneterra.Content.Enchantments;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Soraka;


[Pool(typeof(EventRelicPool))]
public class TimeCapsule() : SlayRuneterraRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    private List<SerializableCard> _serializableCards = new ();
    public override bool ShowCounter => IsMutable && _serializableCards.Count > 0;
    public override int DisplayAmount => IsMutable ? _serializableCards.Count : 0;
    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4), new StringVar("CardTitles")];
    
    [SavedProperty]
    public List<SerializableCard> SerializableCards
    {
        get
        {
            return _serializableCards;
        }
        private set
        {
            AssertMutable();
            _serializableCards.Clear();
            _serializableCards.AddRange(value);
            UpdateCardList();
        }
    }
    
    protected override void AfterCloned()
    {
        base.AfterCloned();
        _serializableCards = new List<SerializableCard>();
    }
    
    public override async Task AfterObtained()
    {
        // IEnumerable<CardModel> enumerable = (await CardSelectCmd.FromDeckForRemoval(
        //             prefs: new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, DynamicVars.Cards.IntValue),
        //             player: Owner, 
        //             filter: c => c.IsUpgradable)).OrderBy<CardModel, string>(c => c.Id.Entry, StringComparer.Ordinal);
        //
        // IEnumerable<CardModel> enumerable2 = PileType.Deck.GetPile(Owner).Cards.Where((c) => c.IsUpgradable).ToList().StableShuffle(Owner.RunState.Rng.Niche)
        //             .Take(DynamicVars.Cards.IntValue);
        //
        List<CardModel> source = PileType.Deck.GetPile(Owner).Cards.Where(c => c.Rarity == CardRarity.Basic).ToList();
        IEnumerable<CardModel> strikes = source.Where(c => c.Tags.Contains(CardTag.Strike)).Take((int)Math.Floor(DynamicVars.Cards.IntValue / 2m));
        IEnumerable<CardModel> defends = source.Where(c => c.Tags.Contains(CardTag.Defend)).Take((int)Math.Ceiling(DynamicVars.Cards.IntValue / 2m));
        
        foreach (CardModel item in strikes.Concat(defends))
        {
            CardModel cardModel = (CardModel)item.MutableClone();
            SerializableCards.Add(cardModel.ToSerializable());
            await CardPileCmd.RemoveFromDeck(item);
        }
        UpdateCardList();
    }
    
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (!base.Owner.Creature.IsDead && SerializableCards.Count != 0 && room.RoomType is RoomType.Boss or RoomType.Elite)
        {
            Flash();
            await Cmd.CustomScaledWait(0.1f, 1f);
            SerializableCard serializableCard = Owner.PlayerRng.Rewards.NextItem(SerializableCards)!;
            CardModel cardModel = CardModel.FromSerializable(serializableCard);
            if (!Owner.RunState.ContainsCard(cardModel))
            {
                Owner.RunState.AddCard(cardModel, Owner);
            }
            // if (cardModel.IsUpgradable)
            // {
            //     CardCmd.Upgrade(cardModel, CardPreviewStyle.MessyLayout);
            // }
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck));
            Status = ((SerializableCards.Count <= 0) ? RelicStatus.Disabled : RelicStatus.Normal);
            SerializableCards.Remove(serializableCard);
            UpdateCardList();
        }
    }
    
    
    
    private void UpdateCardList()
    {
        base.Status = ((SerializableCards.Count <= 0) ? RelicStatus.Disabled : RelicStatus.Normal);
        StringVar stringVar = (StringVar)base.DynamicVars["CardTitles"];
        if (SerializableCards.Count == 0)
        {
            stringVar.StringValue = string.Empty;
        }
        else
        {
            stringVar.StringValue = string.Join('\n', SerializableCards.Select(c => "- " + SaveUtil.CardOrDeprecated(c.Id!).Title));
        }
        InvokeDisplayAmountChanged();
    }

    public void DebugAddCard(SerializableCard card)
    {
        SerializableCards.Add(card);
    }
}