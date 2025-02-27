using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWin : MonoBehaviour
{
    public void ShouldWinLevel() => FindObjectOfType<PlayLoadedLevelV2>().LevelCompleted();
}
