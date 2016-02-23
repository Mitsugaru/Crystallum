using UnityEngine;
using System.Collections;
using System;

public interface IBattleFormula<T> : IFormula<T> {

    /// <summary>
    /// Calculate the damage the attacker does based on the given primary stats.
    /// </summary>
    /// <param name="attackerStat">Primary attacker stat</param>
    /// <param name="defenderStat">Primary defending stat</param>
    /// <param name="turn">The battle system turn</param>
    /// <param name="actionCount">The current battle system action count</param>
    /// <returns>The total damage incurred</returns>
    T Generate(int attackerStat, int defenderStat, int turn, int actionCount);
}
