using UnityEngine;
using System.Collections;
using System;

public interface IBattleFormula<T> : IFormula<T> {

    /// <summary>
    /// The range of the defense multiplier
    /// </summary>
    MultiplierValues DefenseMultiplier { get; set; }

    /// <summary>
    ///  Calculate the damage the attacker does based on the given primary stats.
    /// </summary>
    /// <param name="attacker">Attacker info</param>
    /// <param name="defender">Defender info</param>
    /// <param name="turn">The battle system turn</param>
    /// <param name="actionCount">The current battle system action count</param>
    /// <returns>The total damage incurred</returns>
    T Generate(BattleActorInfo attacker, BattleActorInfo defender, int turn, int actionCount);
}
