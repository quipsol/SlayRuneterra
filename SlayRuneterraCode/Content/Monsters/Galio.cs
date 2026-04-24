using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Monsters;

public class Galio : SlayRuneterraMonsterModel
{
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 91, 81);
    public override int MaxInitialHp => MinInitialHp;

    protected override string VisualsPath => "res://SlayRuneterra/scenes/creature_visuals/demacia_act/galio.tscn";

    public override bool HasDeathSfx => false;
    
    private const string TACKLE = "TACKLE";
    private const string PROTECT = "PROTECT";
    
    private const int StrengthenBlock = 12;
    private int TackleDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
    private int ProtectAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 12);
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        
        MoveState tackleState = new MoveState(TACKLE, PerformTackle, new SingleAttackIntent(TackleDamage));
        MoveState protectState = new MoveState(PROTECT, PerformProtect, new DefendIntent(), new BuffIntent());
        
        tackleState.FollowUpState = protectState;
        protectState.FollowUpState = tackleState;

        states.Add(tackleState);
        states.Add(protectState);

        // Move selection branch

        return new MonsterMoveStateMachine(states, protectState);
    }

    private async Task PerformTackle(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(TackleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f)
                    .WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick")
                    .WithHitFx("vfx/vfx_attack_blunt")
                    .Execute(null);
    }
    
    private async Task PerformProtect(IReadOnlyList<Creature> targets)
    {
        NPowerUpVfx.CreateGhostly(Creature);
        IEnumerable<Creature> allLux = this.Creature.CombatState!.GetTeammatesOf(this.Creature).Where((Creature c) => c.IsAlive && c.Monster is Lux);
        List<Creature> enumerable = allLux.ToList();
        if (!enumerable.Any())
            await CreatureCmd.GainBlock(this.Creature, new BlockVar(ProtectAmount + 5, ValueProp.Move), null);
        else
            foreach (Creature creature in enumerable)
            {
                await CreatureCmd.GainBlock(creature, new BlockVar(ProtectAmount, ValueProp.Move), null);
            }
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), this.Creature, 1, this.Creature, null);
    }
    
}