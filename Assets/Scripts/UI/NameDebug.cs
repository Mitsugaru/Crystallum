using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class NameDebug : View
{

    [Inject]
    public INameGenerator NameGenerator { get; set; }

    public void OutputName()
    {
        Debug.Log(NameGenerator.GenerateName());
    }
}
