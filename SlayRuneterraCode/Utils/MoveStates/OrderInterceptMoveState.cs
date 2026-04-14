using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Random;

namespace SlayRuneterra.Utils;

public class OrderInterceptMoveState : MonsterState
{
    public override bool ShouldAppearInLogs => true;
    public string BranchId { get; }
    public override string Id => this.BranchId;
    
    public OrderInterceptMoveState(string stateId) => this.BranchId = stateId;

    
    private ConditionalBranch InterceptMove { get; set; } = new(new MoveState(), () => false);
    
    private int _lastStateIndex = -1;
    private List<MoveState> States { get; } = [];

    public bool CanInterceptMultipleTimes { get; set; } = false;
    private bool _hasIntercepted = false;
    
    public void SetInterceptMove(MonsterState move, Func<bool> condition)
    {
        this.InterceptMove = new ConditionalBranch(move, condition);
    }
    
    public void AddState(MoveState state)
    {
        States.Add(state);
    }
    
    public override string GetNextState(Creature _, Rng __)
    {
        if ( (CanInterceptMultipleTimes || !_hasIntercepted) && InterceptMove.Evaluate() > 0.0)
        {
            _hasIntercepted = true;
            return InterceptMove.id;
        }
        
        if(States.Count == 0)
            throw new InvalidOperationException("No valid next state found.");
        _lastStateIndex++;
        return States[_lastStateIndex % States.Count].Id;
    }

    public override void RegisterStates(Dictionary<string, MonsterState> monsterStates)
    {
        monsterStates.Add(this.Id, (MonsterState) this);
    }
    
    
    private readonly struct ConditionalBranch
    {
        public readonly string id;
        private readonly Func<bool> _conditionalLambda;

        public ConditionalBranch(MonsterState state, Func<bool> condition)
        {
            this.id = state.Id;
            this._conditionalLambda = condition;
        }

        public float Evaluate()
        {
            return this._conditionalLambda != null ? (float) (this._conditionalLambda() ? 1 : 0) : 1f;
        }
    }

}