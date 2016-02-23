using strange.extensions.mediation.impl;
using System.Collections.Generic;

public abstract class AbstractBattleSystem : View, IBattleSystem
{

    [Inject]
    public IBattleManager BattleManager { get; set; }

    protected int turn = 1;
    public int Turn
    {
        get
        {
            return turn;
        }
    }
    protected int actionCount = 1;
    public int ActionCount
    {
        get
        {
            return actionCount;
        }
    }
    public ICondition VictoryCondition { get; set; }

    public ICondition LossCondition { get; set; }

    public IBattleFormula<int> Formula { get; set; }

    /// <summary>
    /// A mapping of entites to their temporary battle stats.
    /// We only write to an entity's base stat for hp / xp gain / loss.
    /// </summary>
    protected Dictionary<Entity, BattleStats> stats = new Dictionary<Entity, BattleStats>();

    /// <summary>
    /// Random hash function
    /// </summary>
    protected HashFunction random;

    public abstract void BattleStep();
    public abstract Entity[] GetNextActingEntities();

    public void IncrementTurn()
    {
        if (turn == int.MaxValue)
        {
            turn = int.MinValue;
        }
        else
        {
            turn++;
        }
    }

    public void IncrementAction()
    {
        if (actionCount == int.MaxValue)
        {
            actionCount = int.MinValue;
        }
        else
        {
            actionCount++;
        }
    }

    public void SetSeed(int seed)
    {
        if (random == null)
        {
            random = new XXHash(seed);
        }
    }

    protected bool CanAct(Entity entity)
    {
        bool act = true;
        //Check health
        if (entity.HP <= 0)
        {
            act = false;
        }
        return act;
    }

    public bool ConditionCheck()
    {
        VictoryCondition.CheckCondition();
        LossCondition.CheckCondition();
        return ConditionResult();
    }

    public bool ConditionResult()
    {
        return VictoryCondition.Fulfilled || LossCondition.Fulfilled;
    }
}
