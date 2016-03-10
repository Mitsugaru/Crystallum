using UnityEngine;
using System.Collections;

/// <summary>
/// This holds temporary stat modifiers on a per battle instance.
/// Masks the entity's base values with modifiers.
/// Will only modify the HP if affected.
/// </summary>
public class BattleStats
{

    private Entity entity;
    /// <summary>
    /// Entity the battle stats apply to.
    /// </summary>
    public Entity Entity
    {
        get
        {
            return entity;
        }
    }

    //TODO have different kinds of temp hp. Ones that can be regenerated. Versus ones that cannot.
    protected int tempHp = 0;
    protected int maxTempHp = 0;
    protected int modVirtue = 0;
    protected int modResolve = 0;
    protected int modSpirit = 0;
    protected int modDeft = 0;
    //TODO maybe not have this modifiable... Would be difficult as we calculate
    //entity HP from their base stat and not from this masked stat...
    //Unless we give each Entity a BattleStats instance... which I don't
    //particularly care for.
    protected int modVitality = 0;

    public int Limit
    {
        get
        {
            return entity.Limit;
        }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="entity"></param>
    public BattleStats(Entity entity)
    {
        this.entity = entity;
    }

    public int GetHP()
    {
        return entity.HP + tempHp;
    }

    public void SetHP(int value)
    {
        if (value > entity.MaxHP)
        {
            entity.SetHP(entity.MaxHP);
            tempHp = value - entity.HP;
            if (tempHp > maxTempHp)
            {
                tempHp = maxTempHp;
            }
        }
        else
        {
            tempHp = 0;
            entity.SetHP(value);
        }
    }

    /// <summary>
    /// Max HP value
    /// </summary>
    /// <returns>Max HP</returns>
    public int GetMaxHP()
    {
        return entity.MaxHP + maxTempHp;
    }

    public int GetVirtue()
    {
        return CalculatePositiveSum(entity.Virtue, modVirtue);
    }

    public void SetVirtue(int value)
    {
        modVirtue = CalculateDiff(entity.Virtue, value);
    }

    public int GetResolve()
    {
        return CalculatePositiveSum(entity.Resolve, modResolve);
    }

    public void SetResolve(int value)
    {
        modResolve = CalculateDiff(entity.Resolve, value);
    }

    public int GetSpirit()
    {
        return CalculatePositiveSum(entity.Spirit, modSpirit);
    }

    public void SetSpirit(int value)
    {
        modSpirit = CalculateDiff(entity.Spirit, value);
    }

    public int GetDeft()
    {
        return CalculatePositiveSum(entity.Deft, modDeft);
    }

    public void SetDeft(int value)
    {
        modDeft = CalculateDiff(entity.Spirit, value);
    }

    public int GetVitality()
    {
        return CalculatePositiveSum(entity.Vitality, modVitality);
    }

    public void SetVitality(int value)
    {
        modVitality = CalculateDiff(entity.Vitality, modVitality);
    }

    /// <summary>
    /// Calculates the positive sum of the given array of values.
    /// Ensures a positive, non-zero result.
    /// </summary>
    /// <param name="values">Values to sum</param>
    /// <returns>Sumation of values, with floor of 1.</returns>
    protected int CalculatePositiveSum(params int[] values)
    {
        int result = 0;
        foreach (int v in values)
        {
            result += v;
        }
        if (result <= 0)
        {
            //At minimum 1, just in case 0 does weird things
            result = 1;
        }
        return result;
    }

    /// <summary>
    /// Calculates the diff value between given base and new values.
    /// Ensures a diff value that will not, when added with base value, will return at least 1.
    /// </summary>
    /// <param name="baseValue">The base value</param>
    /// <param name="newValue">What the new value should be</param>
    /// <returns>The difference between base and new value.</returns>
    protected int CalculateDiff(int baseValue, int newValue)
    {
        int result = newValue - baseValue;
        if (result <= -baseValue)
        {
            //At minimum 1 - baseValue, just in case diff resulting in 0 does weird things
            result = 1 - baseValue;
        }
        return result;
    }
}
