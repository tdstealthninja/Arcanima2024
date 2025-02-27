using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateTarotEntryEditor : EditorWindow
{
    string entryName = "";
    Sprite cardSprite;
    //[TextArea(5,10)]
    string cardEntry = "";

#if UNITY_EDITOR
    [MenuItem("Tools/Create Tarot Entry")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CreateTarotEntryEditor), true, "Create Tarot Card Entry");  //GetWindow comes from EditorWindow
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Tarot Entry Object", EditorStyles.boldLabel);

        EditorStyles.textField.wordWrap = true;

        entryName = EditorGUILayout.TextField("Entry Name", entryName);
        cardSprite = EditorGUILayout.ObjectField("Card Sprite", cardSprite, typeof(Sprite), false) as Sprite;
        //GUILayout.Label(cardEntry, EditorStyles.wordWrappedLabel);
        cardEntry = EditorGUILayout.TextField("Card Entry", cardEntry, GUILayout.ExpandHeight(true));
        


        //materialName = EditorGUILayout.TextField("Material name", materialName);
        //colorMap = EditorGUILayout.ObjectField("Color map", colorMap, typeof(Texture), false) as Texture;
        //normalMap = EditorGUILayout.ObjectField("Normal map", normalMap, typeof(Texture), false) as Texture;
        //metalicMap = EditorGUILayout.ObjectField("Metalic map", metalicMap, typeof(Texture), false) as Texture;

        if (GUILayout.Button("Create Entry"))
        {
            if (entryName.Length <= 0)
                Debug.Log("Make sure to name the entry before creating it");
            else
                CreateEntry();
        }

    }


    public void CreateEntry()
    {
        TarotCardEntry entry = ScriptableObject.CreateInstance<TarotCardEntry>();
        entry.name = entryName;
        entry.card = cardSprite;
        entry.description = cardEntry;

        string path = "Assets/TarotEntries/" + entry.name + ".asset";

        AssetDatabase.CreateAsset(entry, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(AssetDatabase.GetAssetPath(entry));
        //Debug.Log("Created Log");
        //Debug.Log(AssetDatabase.GetAssetPath(createdMaterial));
    }
#endif
}
