using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelManagerData
{
    [SerializeField]
    public string levelName;
    [SerializeField, TextArea]
    public string levelDialogue;
}
