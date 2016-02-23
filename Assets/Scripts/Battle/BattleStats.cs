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

    public int HP
    {
        get
        {
            return entity.HP + tempHp;
        }
        set
        {
            if (value > entity.MaxHP)
            {
                entity.HP = entity.MaxHP;
                tempHp = value - entity.HP;
                if (tempHp > maxTempHp)
                {
                    tempHp = maxTempHp;
                }
            }
            else
            {
                tempHp = 0;
                entity.HP = value;
            }
        }
    }
    /// <summary>
    /// Max HP value
    /// </summary>
    public int MaxHP
    {
        get
        {
            return entity.MaxHP + maxTempHp;
        }
    }

    protected int modVirtue = 0;
    /// <summary>
    /// Current virtue value
    /// </summary>
    public int Virtue
    {
        get
        {
            return CalculatePositiveSum(entity.Virtue, modVirtue);
        }
        set
        {
            modVirtue = CalculateDiff(entity.Virtue, value);
        }
    }

    protected int modResolve = 0;
    /// <summary>
    /// Current resolve value
    /// </summary>
    public int Resolve
    {
        get
        {
            return CalculatePositiveSum(entity.Resolve, modResolve);
        }
        set
        {
            modResolve = CalculateDiff(entity.Resolve, value);
        }
    }

    protected int modSpirit = 0;
    /// <summary>
    /// Current spirit value
    /// </summary>
    public int Spirit
    {
        get
        {
            return CalculatePositiveSum(entity.Spirit, modSpirit);
        }
        set
        {
            modSpirit = CalculateDiff(entity.Spirit, value);
        }
    }

    protected int modDeft = 0;
    /// <summary>
    /// Current deft value
    /// </summary>
    public int Deft
    {
        get
        {
            return CalculatePositiveSum(entity.Deft, modDeft);
        }
        set
        {
            modDeft = CalculateDiff(entity.Spirit, value);
        }
    }
    //TODO maybe not have this modifiable... Would be difficult as we calculate
    //entity HP from their base stat and not from this masked stat...
    //Unless we give each Entity a BattleStats instance... which I don't
    //particularly care for.
    protected int modVitality = 0;
    /// <summary>
    /// Current vitality value
    /// </summary>
    public int Vitality
    {
        get
        {
            return CalculatePositiveSum(entity.Vitality, modVitality);
        }
        set
        {
            modVitality = CalculateDiff(entity.Vitality, modVitality);
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
