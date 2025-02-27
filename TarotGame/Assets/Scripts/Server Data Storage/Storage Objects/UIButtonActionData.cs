using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIButtonActionData : ActionData
{
    public string buttonName;

    public UIButtonActionData(string buttonName) : base("Button Pressed")
    {
        this.buttonName = buttonName;
    }

    public override string ToString()
    {
        return "";
    }
}
