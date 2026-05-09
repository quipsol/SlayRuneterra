using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Content.Enchantments;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Events.General;

public class WanderingMage : SlayRuneterraEventModel
{
    public override string CustomInitialPortraitPath => "res://SlayRuneterra/images/events/amalgamator.png";
    public override string? CustomBackgroundScenePath => null;
    public override string CustomVfxPath => "";

    public override bool IsAllowed(IRunState runState)
    {
        return SlayRuneterraConfig.IsEnabled && runState.Players.All((p => CardPile.Get(PileType.Deck, p)!.Cards.Any(ModelDb.Enchantment<Potential>().CanEnchant)));
    }

    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
                new ("LearnPotential", 1),
                new ("PrivateLessonPotential", 2),
                new GoldVar(90)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
                    Option(Learn, HoverTipFactory.FromEnchantment<Potential>(DynamicVars["LearnPotential"].IntValue)),
                    AllowOrLockOption(() => Owner!.Gold >= DynamicVars.Gold.BaseValue, PrivateLesson, 
                                HoverTipFactory.FromEnchantment<Potential>(DynamicVars["PrivateLessonPotential"].IntValue))
        ];
    }


    private async Task Learn()
    {
        await EnchantCard(DynamicVars["LearnPotential"].IntValue);
        SetEventFinished(GetDescription("LEAVE"));
    }

    private async Task PrivateLesson()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner!);
        await EnchantCard(DynamicVars["PrivateLessonPotential"].IntValue);
        SetEventFinished(GetDescription("LEAVE"));
    }

    private async Task EnchantCard(int enchantAmount)
    {
        CardModel? cardModel = (await CardSelectCmd.FromDeckForEnchantment(Owner!, ModelDb.Enchantment<Potential>(), enchantAmount,
                    new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1))).FirstOrDefault();
        if (cardModel != null)
        {
            CardCmd.Enchant<Potential>(cardModel, DynamicVars["PrivateLessonPotential"].BaseValue);
            NCardEnchantVfx? nCardEnchantVfx = NCardEnchantVfx.Create(cardModel);
            if (nCardEnchantVfx != null)
            {
                NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(nCardEnchantVfx);
            }
        }
    }


}