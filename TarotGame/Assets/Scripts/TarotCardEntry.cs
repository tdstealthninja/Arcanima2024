using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "TarotGame/TarotInventory/TarotEntry")]
public class TarotCardEntry : ScriptableObject
{
    public Sprite card;
    [TextArea(5, 10)]
    public string description = "";
}
