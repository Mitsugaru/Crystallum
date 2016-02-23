using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a battle objective that can be fulfilled.
/// </summary>
public interface ICondition
{

    /// <summary>
    /// Condition fulfilled flag
    /// </summary>
    bool Fulfilled { get; }

    /// <summary>
    /// Check if the condition has been fulfilled
    /// </summary>
    void CheckCondition();

}
