using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelManagerV2 : MonoBehaviour
{
    public static LevelManagerV2 Instance;

    //[SerializeField]
    //private List<LevelManagerDataV2> levelData;
    [SerializeField]
    private List<Level> levels;

    private int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance ??= this;
    }

    // public string GetLevelName(int index) => levelData[index].levelName;
    //public Level GetLevelObj(int index) => levelData[index].level;

    public Level GetLevelObj(int index) => levels[index];
    public string GetLevelDialogue(int index) => "";//levelData[index].levelDialogue;

    public int GetCurrentLevel() => currentLevel;
    public void SetCurrentLevel(int current) => currentLevel = current;

    public bool InvalidIndex() => currentLevel >= levels.Count || currentLevel < 0;

// #if UNITY_EDITOR
//     [CustomEditor(typeof(LevelManagerV2))]
//     public class LevelManagerV2Editor : Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             serializedObject.Update();

//             EditorGUILayout.PropertyField(serializedObject.FindProperty("levelData"), true);

//             serializedObject.ApplyModifiedProperties();
//         }
//     }
// #endif
}
