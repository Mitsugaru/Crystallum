using UnityEngine;

/// <summary>
/// Has methods and values that help with stat calculation
/// </summary>
public class StatUtils
{
    public static readonly int MIN_EXP = 2000;
    /// <summary>
    /// Maximum experience factor
    /// </summary>
    public static readonly int MAX_EXP = 100000;

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

    public static readonly int MAX_LEVEL = 100;

    public static readonly int MAX_ENEMY_LEVEL = 1000;

    /// <summary>
    /// Calculate the E-Value for a given level.
    /// If level is out of bounds, returns 0.
    /// </summary>
    /// <param name="level">Level to calculate</param>
    /// <param name="maxLevel">The max level</param>
    /// <returns>E-Value for level</returns>
    public static float CalcEValue(int level, int maxLevel)
    {
        float eVal = 0;

        if (level >= 1 && level <= 100)
        {
            //(EXP(current level × 3 ÷ 101) − 1 ÷ (EXP(3) − 1)) ÷ 20
            eVal = (Mathf.Exp(level * 3f / (maxLevel + 1f)) - 1f / (Mathf.Exp(3f) - 1f)) / 20f;
        }

        return eVal;
    }

    /// <summary>
    /// Calculate the base experience value.
    /// </summary>
    /// <param name="maxLevel">The max level for the entity</param>
    /// <param name="maxExp">The max experience for the entity</param>
    /// <returns>Base XP value</returns>
    public static float CalcBaseExp(int maxLevel, int maxExp)
    {
        return CalcEValue(1, maxLevel) * maxExp;
    }

    public static int CalcMaxStat(int level)
    {
        int max = 0;
        if (level >= 1)
        {
            max = level * MAX_STAT;
        }
        return max;
    }
}
