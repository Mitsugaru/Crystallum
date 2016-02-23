/// <summary>
/// Interface for all battle systems
/// </summary>
public interface IBattleSystem
{

    /// <summary>
    /// Current turn count.
    /// </summary>
    int Turn { get; }

    /// <summary>
    /// Current action count.
    /// </summary>
    int ActionCount { get; }

    /// <summary>
    /// The victory condition for the battle.
    /// </summary>
    ICondition VictoryCondition { get; set; }

    /// <summary>
    /// The loss condition for the battle.
    /// </summary>
    ICondition LossCondition { get; set; }

    /// <summary>
    /// Battle formula that calculates damage values
    /// </summary>
    IBattleFormula<int> Formula { get; set; }

    /// <summary>
    /// Step through a battle phase.
    /// Any entities that were to act next get to take their action.
    /// Any incremental / per turn events occur.
    /// Turn will increment by 1.
    /// </summary>
    void BattleStep();

    /// <summary>
    /// Get the entities that will act next battle step.
    /// </summary>
    /// <returns>Array of entites that have actions when BattleStep is called</returns>
    Entity[] GetNextActingEntities();

    /// <summary>
    /// Wrap around call to increment turn.
    /// We will wrap if we go over, since battle formulas take the turn data.
    /// </summary>
    void IncrementTurn();

    /// <summary>
    /// Wrap around call to increment the action count.
    /// We will wrap if we go over, since battle formulas take the action count data.
    /// </summary>
    void IncrementAction();

    /// <summary>
    /// Check the conditions of the battle.
    /// </summary>
    /// <returns>True if any conditions have been fulfilled</returns>
    bool ConditionCheck();

    /// <summary>
    /// Get result of condition check.
    /// </summary>
    /// <returns>True if any conditions have been fulfilled</returns>
    bool ConditionResult();

    /// <summary>
    /// Set the seed for the random hash generator.
    /// </summary>
    /// <param name="seed">Initial seed</param>
    void SetSeed(int seed);
}
