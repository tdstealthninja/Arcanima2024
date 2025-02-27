using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerDataV2 : MonoBehaviour
{
    [SerializeField]
    public Level level;
    [SerializeField, TextArea]
    public string levelDialogue = "";
}
