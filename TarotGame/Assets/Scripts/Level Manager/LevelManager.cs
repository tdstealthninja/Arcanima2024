using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField]
    private List<LevelManagerData> levelData;

    private int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance ??= this;
    }

    public string GetLevelName(int index) => levelData[index].levelName;
    public string GetLevelDialogue(int index) => levelData[index].levelDialogue;

    public int GetCurrentLevel() => currentLevel;
    public void SetCurrentLevel(int current) => currentLevel = current;

    public bool InvalidIndex() => currentLevel >= levelData.Count || currentLevel < 0;

#if UNITY_EDITOR
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("levelData"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
