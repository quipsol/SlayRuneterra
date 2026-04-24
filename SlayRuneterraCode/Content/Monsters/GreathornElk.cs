using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class GreathornElk : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 64, 61);
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 66, 63);
    
    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/greathorn_elk.tscn";
    
    #region MoveStateMachine
    
    private const string STAMPEDE = "STAMPEDE";
    private const string WAIL = "WAIL";
    private const string WAIL_OR_STAMPEDE = "WAIL_OR_STAMPEDE";

    private int StampedeDamage => 5;
    private int StampedeHits => 3;
    private int WailBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
    private int WailWeak => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);

    private int _wailCooldownCounter = 0;
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        MoveState stampedeState = new MoveState(STAMPEDE, Stampede, new MultiAttackIntent(StampedeDamage, StampedeHits));
        MoveState wailState = new MoveState(WAIL, Wail, new DebuffIntent(), new DefendIntent());
        
        ConditionalBranchState weakOrStampedeState = new ConditionalBranchState(WAIL_OR_STAMPEDE);
        weakOrStampedeState.AddState(stampedeState, () => _wailCooldownCounter < WailWeak);
        weakOrStampedeState.AddState(wailState, () => _wailCooldownCounter >= WailWeak);
        
        stampedeState.FollowUpState = weakOrStampedeState;
        wailState.FollowUpState = stampedeState;
        
        states.Add(stampedeState);
        states.Add(wailState);
        states.Add(weakOrStampedeState);
        
        return new MonsterMoveStateMachine(states, wailState);
    }
    
    #region State Methods
    
    private async Task Stampede(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(StampedeDamage)
                    .FromMonster(this)
                    .WithAttackerAnim("Attack", 0.3f)
                    .WithHitCount(StampedeHits)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
        _wailCooldownCounter++;
    }
    private async Task Wail(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), targets, WailWeak, this.Creature, null);
        await CreatureCmd.GainBlock(this.Creature, new BlockVar(WailBlock, ValueProp.Move), null);
        _wailCooldownCounter = 0;
    }
    
    
    #endregion State Methods
    
    #endregion MoveStateMachine
}