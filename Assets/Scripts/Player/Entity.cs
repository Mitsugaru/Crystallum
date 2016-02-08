using UnityEngine;
using System.Collections;

public class Entity
{

    protected int level = 1;
    /// <summary>
    /// Current level
    /// </summary>
    /// <returns>Current level</returns>
    public int Level
    {
        get
        {
            return level;
        }
    }

    protected int xp = 0;
    /// <summary>
    /// The current xp
    /// </summary>
    /// <returns></returns>
    public int XP
    {
        get
        {
            return xp;
        }
    }

    /// <summary>
    /// The XP required to achieve the next level
    /// </summary>
    /// <returns></returns>
    public int XPToNextLevel
    {
        get
        {
            int req = xpStat.GetValue(level + 1) - xp;
            if(req < 0) {
                req = 0;
            }
            return req;
        }
    }

    protected Stats virtueStat = new Stats();

    protected Stats resolveStat = new Stats();

    protected Stats spiritStat = new Stats();

    protected Stats deftStat = new Stats();

    protected Stats vitalityStat = new Stats();

    protected Stats hpStat = new Stats();

    protected Stats xpStat = new Stats();

    /// <summary>
    /// Hash function
    /// </summary>
    protected HashFunction random;

    /// <summary>
    /// Step counter for hash function
    /// </summary>
    protected int rCount = 0;

    public Entity(int seed)
    {
        random = new XXHash(seed);
        GenerateFormulas();
        GenerateStats();
    }

    protected void GenerateFormulas()
    {
        /*
         * Generate multipliers
         */
        // Virtue
        MultiplierValues virtueMultipliers = GenerateMultiplier(StatUtils.MIN_STAT, StatUtils.MAX_STAT, rCount);
        rCount += 2;

        // Resolve
        MultiplierValues resolveMultipliers = GenerateMultiplier(StatUtils.MIN_STAT, StatUtils.MAX_STAT, rCount);
        rCount += 2;

        // Spirit
        MultiplierValues spiritMultipliers = GenerateMultiplier(StatUtils.MIN_STAT, StatUtils.MAX_STAT, rCount);
        rCount += 2;

        // Deftness
        MultiplierValues deftMultipliers = GenerateMultiplier(StatUtils.MIN_STAT, StatUtils.MAX_STAT, rCount);
        rCount += 2;

        // Vitality
        MultiplierValues vitalityMultipliers = GenerateMultiplier(StatUtils.MIN_STAT, StatUtils.MAX_STAT, rCount);
        rCount += 2;

        // HP
        MultiplierValues hpMultipliers = GenerateMultiplier(StatUtils.MIN_STAT, StatUtils.MAX_HP, rCount);
        rCount += 2;

        /*
         * Genereate formulas
         */
        // Virtue
        BaseStatFormula virtueFormula = new BaseStatFormula(virtueStat);
        virtueFormula.SetSeed(unchecked((int)random.GetHash(rCount++)));
        virtueFormula.Multiplier = virtueMultipliers;
        virtueStat.SetFormula(virtueFormula);

        // Resolve
        BaseStatFormula resolveFormula = new BaseStatFormula(resolveStat);
        resolveFormula.SetSeed(unchecked((int)random.GetHash(rCount++)));
        resolveFormula.Multiplier = resolveMultipliers;
        resolveStat.SetFormula(resolveFormula);

        // Spirit
        BaseStatFormula spiritFormula = new BaseStatFormula(spiritStat);
        spiritFormula.SetSeed(unchecked((int)random.GetHash(rCount++)));
        spiritFormula.Multiplier = spiritMultipliers;
        spiritStat.SetFormula(spiritFormula);

        // Deftness
        BaseStatFormula deftFormula = new BaseStatFormula(deftStat);
        deftFormula.SetSeed(unchecked((int)random.GetHash(rCount++)));
        deftFormula.Multiplier = deftMultipliers;
        deftStat.SetFormula(deftFormula);

        // Vitality
        BaseStatFormula vitalityFormula = new BaseStatFormula(virtueStat);
        vitalityFormula.SetSeed(unchecked((int)random.GetHash(rCount++)));
        vitalityFormula.Multiplier = vitalityMultipliers;
        vitalityStat.SetFormula(vitalityFormula);

        // Health
        HealthFormula hpFormula = new HealthFormula(hpStat, vitalityStat);
        hpFormula.SetSeed(unchecked((int)random.GetHash(rCount++)));
        hpFormula.Multiplier = hpMultipliers;
        hpStat.SetFormula(hpFormula);

        // Experience
        ExperienceFormula xpFormula = new ExperienceFormula(xpStat);
        xpStat.SetFormula(xpFormula);
    }

    protected void GenerateStats()
    {
        virtueStat.Generate();
        resolveStat.Generate();
        spiritStat.Generate();
        deftStat.Generate();
        vitalityStat.Generate();
        hpStat.Generate();
        xpStat.Generate();
    }

    /// <summary>
    /// Generate a multiplier value
    /// </summary>
    /// <param name="min">minimum value</param>
    /// <param name="max">maximum value</param>
    /// <param name="count">current step count</param>
    /// <returns></returns>
    protected MultiplierValues GenerateMultiplier(int min, int max, int count)
    {
        return new MultiplierValues(random.Range(min, max, count), random.Range(min, max, count + 1));
    }
}
