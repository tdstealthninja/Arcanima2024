using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateMappedMaterial : EditorWindow
{
    string materialName = "MappedMaterial";
    public Material createdMaterial;   
    Texture colorMap;
    Texture normalMap;
    Texture metalicMap;
    //Renderer renderer;



    [MenuItem("Tools/Create Mapped Material")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CreateMappedMaterial));  //GetWindow comes from EditorWindow
    }


    private void OnGUI()
    {
        GUILayout.Label("Create New Material using Maps", EditorStyles.boldLabel);

        materialName = EditorGUILayout.TextField("Material name", materialName);
        colorMap = EditorGUILayout.ObjectField("Color map", colorMap, typeof(Texture), false) as Texture;
        normalMap = EditorGUILayout.ObjectField("Normal map", normalMap, typeof(Texture), false) as Texture;
        metalicMap = EditorGUILayout.ObjectField("Metalic map", metalicMap, typeof(Texture), false) as Texture;

        if (GUILayout.Button("Create Material"))
        {
            CreateMaterial();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CreateMaterial()
    {
        createdMaterial = new Material(Shader.Find("Standard"));
        createdMaterial.EnableKeyword("_NORMALMAP");
        createdMaterial.EnableKeyword("_METALLICGLOSSMAP");
        createdMaterial.SetTexture("_MainTex", colorMap);
        createdMaterial.SetTexture("_BumpMap", normalMap);
        createdMaterial.SetTexture("_MetallicGlossMap", metalicMap);

        string createdMaterialPath = "Assets/" + materialName + ".mat";

        AssetDatabase.CreateAsset(createdMaterial, createdMaterialPath);

        Debug.Log(AssetDatabase.GetAssetPath(createdMaterial));
    }
}
