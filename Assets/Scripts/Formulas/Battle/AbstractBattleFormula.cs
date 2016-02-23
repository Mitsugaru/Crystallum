using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract battle formula class with basic XXHash random function and multiplier values.
/// </summary>
public abstract class AbstractBattleFormula<T> : IBattleFormula<T>
{

    /// <summary>
    /// Multiplier value that formulas use.
    /// </summary>
    /// <returns>Multipliers</returns>
    public MultiplierValues Multiplier { get; set; }

    /// <summary>
    /// XXHash based hash function
    /// </summary>
    protected HashFunction random;

    public abstract T Generate(int attackerStat, int defenderStat, int turn, int actionCount);

    public void SetSeed(int seed)
    {
        random = new XXHash(seed);
    }

}
