using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractActionData : ActionData
{
    public string interactorName;
    public string interatedName;

    public InteractActionData(string interactorName, string interatedName) : base("Interaction")
    {
        this.interactorName = interactorName;
        this.interatedName = interatedName;

        description = ToString();
    }

    public override string ToString()
    {
        return interactorName + " -> " + interatedName;
    }
}
