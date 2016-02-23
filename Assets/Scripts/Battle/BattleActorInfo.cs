using UnityEngine;
using System.Collections;

public class BattleActorInfo
{

    protected BattleStats stats;
    public BattleStats Stats
    {
        get
        {
            return stats;
        }
    }

    protected int value = 0;
    public int Value
    {
        get
        {
            return value;
        }
    }

    protected int level = 0;
    public int Level
    {
        get
        {
            return level;
        }
    }

    public BattleActorInfo(BattleStats stats, int value, int level)
    {
        this.stats = stats;
        this.value = value;
        this.level = level;
    }
}
