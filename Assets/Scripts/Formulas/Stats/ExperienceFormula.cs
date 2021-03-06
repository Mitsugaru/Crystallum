﻿using UnityEngine;

/// <summary>
/// Formula that calculates the XP values
/// </summary>
public class ExperienceFormula : AbstractStatFormula<int>
{
    /// <summary>
    /// Experience stats reference
    /// </summary>
    private Stats experience;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="xp">Experience stats reference</param>
    public ExperienceFormula(Stats xp)
    {
        this.experience = xp;
    }

    public override int Generate(int level)
    {
        int expReq = 0;

        int expVal = Mathf.FloorToInt(StatUtils.CalcEValue(level, experience.Limit) * Multiplier.Maximum - StatUtils.CalcBaseExp(experience.Limit, Multiplier.Maximum));
        if (level == 2)
        {
            expReq = expVal;
        }
        else if (level > 2)
        {
            int prevExp = experience.GetValue(level - 1);
            int backExp = experience.GetValue(level - 2);
            int modifier = backExp;
            if (backExp != prevExp)
            {
                modifier = random.Range(backExp, prevExp, level);
            }
            expReq = Mathf.FloorToInt(Mathf.Max(prevExp, expVal) + (prevExp - modifier));
        }

        return expReq;
    }
}
