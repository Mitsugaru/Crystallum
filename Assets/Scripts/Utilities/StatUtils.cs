using UnityEngine;

/// <summary>
/// Has methods and values that help with stat calculation
/// </summary>
public class StatUtils
{
    /// <summary>
    /// Theoretical maximum experience value
    /// </summary>
    public static readonly int MAX_EXP = 10000;

    /// <summary>
    /// The minimum value range for a stat
    /// </summary>
    public static readonly int MIN_STAT = 1;

    /// <summary>
    /// The maximum value range for a stat
    /// </summary>
    public static readonly int MAX_STAT = 10;

    /// <summary>
    /// The maximum value for the HP stat
    /// </summary>
    public static readonly int MAX_HP = 5;

    /// <summary>
    /// Calculate the E-Value for a given level.
    /// If level is out of bounds, returns 0.
    /// </summary>
    /// <param name="level">Level to calculate</param>
    /// <returns>E-Value for level</returns>
    public static float CalcEValue(int level)
    {
        float eVal = 0;

        if (level >= 1 && level <= 100)
        {
            //(EXP(current level × 3 ÷ 101) − 1 ÷ (EXP(3) − 1)) ÷ 20
            eVal = (Mathf.Exp(level * 3 / 101) - 1 / (Mathf.Exp(3) - 1)) / 20;
        }

        return eVal;
    }

    /// <summary>
    /// Calculate the base experience value.
    /// </summary>
    /// <returns>Base XP value</returns>
    public static float CalcBaseExp()
    {
        return CalcEValue(1) * MAX_EXP;
    }

    public static int CalcMaxStat(int level)
    {
        int max = 0;
        if (level >= 1 && level <= 100)
        {
            max = level * MAX_STAT;
        }
        return max;
    }
}
