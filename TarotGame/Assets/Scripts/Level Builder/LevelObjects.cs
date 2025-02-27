using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "TarotGame/LevelObjects")]
[System.Serializable]
public class LevelObjects : ScriptableObject
{
    public List<GameObject> debugObjs; 
    public List<GameObject> terrainPrefabs;
    public List<GameObject> gridTransformPrefabs;
    public List<GameObject> decorationPrefabs;
    public List<GameObject> NpcPrefabs;
}
