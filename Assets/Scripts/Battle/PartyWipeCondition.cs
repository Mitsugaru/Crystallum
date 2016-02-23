using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Condition that checks if the given party has all been KO'd
/// </summary>
public class PartyWipeCondition : AbstractCondition
{
    /// <summary>
    /// Party to monitor
    /// </summary>
    private List<Entity> party;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="party">Party to monitor</param>
    public PartyWipeCondition(List<Entity> party)
    {
        this.party = party;
    }

    public override void CheckCondition()
    {
        bool hasMembers = false;
        foreach (Entity entity in party)
        {
            if (entity.HP > 0)
            {
                hasMembers = true;
                break;
            }
        }
        fulfilled = !hasMembers;
    }
}
