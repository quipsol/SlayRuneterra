using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;
using SlayRuneterra.Nodes;

namespace SlayRuneterra.Content.Cards;


[Pool(typeof(EventCardPool))]
public class MyGrandFinale() : SlayRuneterraCardModel(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    public override string CustomPortraitPath => "res://SlayRuneterra/images/card_portraits/card.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
                new DamageVar(4, ValueProp.Move),
    ];


    private const string _daggerSpraySfx = "event:/sfx/characters/silent/silent_dagger_spray";
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Func<Node2D> func = (() => NGrandFinaleVfx.Create(base.Owner.Creature, new Color("#ff0000"), goingRight: true)!);
        NCombatRoom? instance = NCombatRoom.Instance;
        if (instance is not null)
            instance.CombatVfxContainer.AddChildSafely((Godot.Node) func());
        return;
        SfxCmd.Play("event:/sfx/characters/silent/silent_dagger_spray");
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .WithHitCount(2)
                    .FromCard(this)
                    .TargetingAllOpponents(CombatState)
                    .WithAttackerFx(() => NGrandFinaleVfx.Create(base.Owner.Creature, new Color("#ff0000"), goingRight: true))
                    .BeforeDamage(delegate
                                {
                                    IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
                                    foreach (Creature item in hittableEnemies)
                                    {
                                        NDaggerSprayImpactVfx child = NDaggerSprayImpactVfx.Create(item, new Color("#000000"), goingRight: true)!;
                                        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
                                    }
                                    return Task.CompletedTask;
                                })
                    .Execute(choiceContext);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}