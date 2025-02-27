using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLevelButton : MonoBehaviour
{
    public void ClearLevel() => FindObjectOfType<TerrainGrid>().ClearLevel();
}
