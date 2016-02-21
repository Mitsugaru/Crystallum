using UnityEngine;
using System.Collections;

/// <summary>
/// This holds temporary stat modifiers on a per battle instance
/// </summary>
public class BattleStats
{

    private Entity entity;
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

    public int MaxHP
    {
        get
        {
            return entity.MaxHP + maxTempHp;
        }
    }

    protected int modVirtue = 0;

    public int Virtue
    {
        get
        {
            return CalculatePositiveSum(entity.Virtue, modVirtue);
        }
    }

    protected int modResolve = 0;

    public int Resolve
    {
        get
        {
            return CalculatePositiveSum(entity.Resolve, modResolve);
        }
    }

    protected int modSpirit = 0;

    public int Spirit
    {
        get
        {
            return CalculatePositiveSum(entity.Spirit, modSpirit);
        }
    }

    protected int modDeft = 0;

    public int Deft
    {
        get
        {
            return CalculatePositiveSum(entity.Deft, modDeft);
        }
    }

    protected int modVitality = 0;

    public int Vitality
    {
        get
        {
            return CalculatePositiveSum(entity.Vitality, modVitality);
        }
    }

    public BattleStats(Entity entity)
    {
        this.entity = entity;
    }

    protected int CalculatePositiveSum(params int[] values)
    {
        int result = 0;
        foreach (int v in values)
        {
            result += v;
        }
        if (result < 0)
        {
            //Wonder if this should be at minimum 1
            result = 0;
        }
        return result;
    }
}
