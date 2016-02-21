using UnityEngine;
using System.Collections;

public interface IBattleSystem
{

    /// <summary>
    /// Current turn count
    /// </summary>
    int Turn { get; }

    /// <summary>
    /// Step through a battle phase
    /// </summary>
    void BattleStep();

    /// <summary>
    /// Get the entities that will act next battle step.
    /// </summary>
    /// <returns>Array of entites that have actions when BattleStep is called</returns>
    Entity[] GetNextActingEntities();

}
