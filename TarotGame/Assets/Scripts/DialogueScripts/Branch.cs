using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TarotGame/DialogueSystem/Branch")]

public class Branch : Dialogue
{
    public List<Dialogue> options = new List<Dialogue>();
}
