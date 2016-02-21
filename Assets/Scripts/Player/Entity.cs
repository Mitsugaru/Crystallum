using UnityEngine;
using System.Collections;

public class Entity
{

    protected string name = "";
    public string Name
    {
        get
        {
            return name;
        }
    }

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

    protected int hp = 0;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (value < 0)
            {
                hp = 0;
            }
            else if (value > hpStat.GetValue(level))
            {
                //Make sure we don't go above their max hp;
                hp = hpStat.GetValue(level);
            }
            else
            {
                hp = value;
            }
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
            if (req < 0)
            {
                req = 0;
            }
            return req;
        }
    }

    protected Stats virtueStat = new Stats();
    public int Virtue
    {
        get
        {
            return virtueStat.GetValue(level);
        }
    }

    protected Stats resolveStat = new Stats();
    public int Resolve
    {
        get
        {
            return resolveStat.GetValue(level);
        }
    }

    protected Stats spiritStat = new Stats();
    public int Spirit
    {
        get
        {
            return spiritStat.GetValue(level);
        }
    }

    protected Stats deftStat = new Stats();
    public int Deft
    {
        get
        {
            return deftStat.GetValue(level);
        }
    }

    protected Stats vitalityStat = new Stats();
    public int Vitality
    {
        get
        {
            return vitalityStat.GetValue(level);
        }
    }

    protected Stats hpStat = new Stats();
    public int MaxHP
    {
        get
        {
            return hpStat.GetValue(level);
        }
    }

    protected Stats xpStat = new Stats();
    public int BaseXP
    {
        get
        {
            return xpStat.GetValue(level);
        }
    }

    /// <summary>
    /// Hash function
    /// </summary>
    protected HashFunction random;

    /// <summary>
    /// Step counter for hash function
    /// </summary>
    protected int rCount = 0;

    public Entity(int seed) : this("NONAME", 1, seed)
    {
    }

    public Entity(string name, int seed) : this(name, 1, seed)
    {
    }

    public Entity(int level, int seed) : this("NONAME", level, seed)
    {
    }

    public Entity(string name, int level, int seed)
    {
        this.name = name;
        this.level = level;
        random = new XXHash(seed);
        GenerateFormulas();
        GenerateStats();

        //Get the values for hp and xp after generation
        hp = hpStat.GetValue(level);
        xp = xpStat.GetValue(level);
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
        xpFormula.SetSeed(unchecked((int)random.GetHash(rCount++)));
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

    public override bool Equals(object obj)
    {
        bool same = false;

        Entity other = obj as Entity;
        if (other != null)
        {
            //not including level or xp since those are mutable during battle
            same = this.name.Equals(other.name) && random.GetSeed().Equals(other.random.GetSeed());
        }

        return same;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode() * 17 + random.GetSeed().GetHashCode();
    }
}
