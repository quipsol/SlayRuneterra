using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Ancients;


// Gives options from all other ancients (and maybe also rare relics from other characters?)
// Preferably has 4 or 5 options to choose from but that's kinda hawd :(
// Add custom ancient card that turns into a random ancient card at the start of combat (and enchants them with something, idk)

//  Total pools: 5
// Pool 1 - 3: A random relic from other ancients (in full 3-act mod only from other Targonian ancients)
// Pool 4: A random character specific relic from the Rare category
// Pool 5: A few Zoe specific options
public class Zoe : SlayRuneterraAncientModel
{
    public override string? CustomMapIconPath => "res://SlayRuneterra/images/placeholder/100_100/purple.png";
    public override string? CustomMapIconOutlinePath => "res://SlayRuneterra/images/placeholder/150_150/black.png";
    public override string? CustomRunHistoryIconPath => "res://SlayRuneterra/images/placeholder/100_100/white.png";
    public override string? CustomRunHistoryIconOutlinePath => "res://SlayRuneterra/images/placeholder/150_150/black.png";

    public override string? CustomBackgroundScenePath => "res://SlayRuneterra/scenes/events/background_scenes/zoe.tscn";

    public override bool IsValidForAct(ActModel act) => false;//SlayRuneterraConfig.IsEnabled;


    
    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);

    public override Color DialogueColor => new Color("3C1931");
    
    private IEnumerable<EventOption> Pool1 => 
    [
                RelicOption<BloodSoakedRose>(),
                RelicOption<WhisperingEarring>(),
                RelicOption<Fiddle>()
    ];

    private IEnumerable<EventOption> Pool2 =>
    [
                RelicOption<PreservedFog>(),
                RelicOption<SereTalon>(),
                RelicOption<DistinguishedCape>().ThatDecreasesMaxHp(9m)
    ];

    private IEnumerable<EventOption> Pool3 =>
    [
                RelicOption<ChoicesParadox>(),
                RelicOption<MusicBox>(),
                RelicOption<LordsParasol>(),
                RelicOption<JeweledMask>()
    ];
    
 
    protected override OptionPools MakeOptionPools => new(
                MakePool(
                            AncientOption<Orrery>(),
                            AncientOption<PrismaticGem>(),
                            AncientOption<SeaGlass>(relicPrep: (glass) => //An example of a relic that requires setup.
                            {
                                if (Owner == null) return glass;
                                var character = Owner.Character;
                                var characterModel = Rng.NextItem(Owner.UnlockState.Characters.Where((Func<CharacterModel, bool>) (c => c.Id != character.Id))) ?? character;
                                glass.CharacterId = characterModel.Id;
                                return glass;
                            })
                ),
                MakePool( //Options can be assigned weight to change their appearance rates. Clone is a bit rarer, for example.
                            AncientOption<Astrolabe>(weight: 10),
                            AncientOption<Driftwood>(weight: 90)
                ),
                MakePool(
                            AncientOption<CharonsAshes>(),
                            AncientOption<OrangeDough>(),
                            AncientOption<PowerCell>(),
                            AncientOption<PaperKrane>(),
                            AncientOption<BigHat>()
                ));
    
    
}