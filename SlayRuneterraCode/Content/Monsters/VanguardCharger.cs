using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class VanguardCharger : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 30, 28);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 32, 30);
 
    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/vanguard_charger.tscn";
    
    
    #region MoveStateMachine
    
    private const string SINGLE = "SINGLE";
    private int SingleDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState placeholderState = new MoveState(SINGLE, Placeholder, new SingleAttackIntent(SingleDamage));
        placeholderState.FollowUpState = placeholderState;
        states.Add(placeholderState);
        
        return new MonsterMoveStateMachine(states, placeholderState);
    }
    
    #region State Methods
    
    private async Task Placeholder(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(SingleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    #endregion State Methods
    
    #endregion MoveStateMachine

    public override async Task AfterAddedToRoom()
    {
        await PowerCmd.Apply<VanguardChargerPower>(new ThrowingPlayerChoiceContext(), this.Creature, 1, this.Creature, null);
    }
}