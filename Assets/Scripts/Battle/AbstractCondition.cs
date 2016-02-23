using UnityEngine;
using System.Collections;
using System;

public abstract class AbstractCondition : ICondition
{
    protected bool fulfilled = false;
    public bool Fulfilled
    {
        get
        {
            return fulfilled;
        }
    }

    public abstract void CheckCondition();
}
