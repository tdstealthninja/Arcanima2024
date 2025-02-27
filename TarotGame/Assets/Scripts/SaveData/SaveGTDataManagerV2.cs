using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class SaveGTDataManagerV2 : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<SaveGTDataManager> onLoad;

    public GameObject savefileInputButton;
    //public GameObject terrainGridPrefab;
    public LevelObjects objectPrefabList;
    private readonly string SAVE_FILE_PATH = "Assets/Scripts/SaveData/";
    private readonly string SAVE_FILE_EXSTENTION = ".asset";
    private string fileName;
    private string levelToLoad = null;
    [SerializeField]
    private Level levelObjToLoad = null;

    #region Getters Setters and Checks

    public void SetCurrentLoadLevel(string loadLevel)
    {
        levelToLoad = loadLevel;

        Load();
    }

    public void SetCurrentLoadLevel(Level level)
    {
        levelObjToLoad = level;

        LoadFromObject();
    }

    public string GetCurrentLoadLevel() => levelToLoad;

    public Level GetCurrentLevelObject() => levelObjToLoad;

    #endregion

    public void ToggleSaveFileInputField()
    {
        if (!savefileInputButton.activeInHierarchy)
            savefileInputButton.SetActive(true);
        else
            savefileInputButton.SetActive(false);
    }


    public void FileNameInput(string fileName)
    {
        if (fileName != null || fileName != "")
        {
            this.fileName = fileName;
            ToggleSaveFileInputField();
            Save();
        }
    }

    public void Save()
    {

        Level newLevel = ScriptableObject.CreateInstance<Level>();
        
        // convert level to data
        // TerrainGrid tg = FindObjectOfType<TerrainGrid>();
        // TerrainGridData tgd = new(tg);
        
        newLevel.InitLevel(objectPrefabList);

        // write data to file
        // BinaryFormatter bf = new();
        // FileStream file = new(SAVE_FILE_PATH + fileName + SAVE_FILE_EXSTENTION, FileMode.Create);
        // bf.Serialize(file, tgd);

        // close file
        // file.Close();

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(newLevel, SAVE_FILE_PATH + fileName + SAVE_FILE_EXSTENTION);
#endif


    }

    public void Load()
    {
        // Gets rid of last level
        TerrainGrid grid = FindObjectOfType<TerrainGrid>();
        if (grid != null)
        {
            grid.ClearLevel();
            Destroy(grid.gameObject);
        }

        // read level
        BinaryFormatter bf = new();
        TextAsset level = Resources.Load("LevelSaves/" + levelToLoad) as TextAsset;
        MemoryStream reader = new(level.bytes);
        TerrainGridData tgd = bf.Deserialize(reader) as TerrainGridData;

        //create level
        tgd.ToObject();
        Debug.Log("Finished Loading TerrainGridData");
    }


    public void LoadFromObject()
    {
        // Gets rid of last level
        TerrainGrid grid = FindObjectOfType<TerrainGrid>();
        if (grid != null)
        {
            grid.ClearLevel();
            Destroy(grid.gameObject);
        }
        
        GameObject go = GameObject.Instantiate(objectPrefabList.debugObjs[1]);
        TerrainGrid tg = go.GetComponent<TerrainGrid>();

        if (levelObjToLoad != null)
        {
            Debug.Log("Loading Terrain");
            foreach (LevelData levelData in levelObjToLoad.terrainLevelData)
            {
                GameObject terrainChildObj = GameObject.Instantiate(objectPrefabList.terrainPrefabs[levelData.id]);
                GameObject terrainObj = GameObject.Instantiate(objectPrefabList.debugObjs[0]);
                terrainChildObj.transform.parent = terrainObj.transform;
                terrainObj.transform.parent = go.transform;
                Terrain terrain = terrainObj.GetComponent<Terrain>();
                terrain.SetCellPostion(Vector3Int.FloorToInt(levelData.location));
                tg.UpdateTerrainList();
            }

            Debug.Log("Loading GridTransform");
            foreach (LevelData levelData in levelObjToLoad.gtLevelData)
            {
                GameObject gridTransformObj = GameObject.Instantiate(objectPrefabList.gridTransformPrefabs[levelData.id]);
                GridTransform gt = gridTransformObj.GetComponent<GridTransform>();
                gt.ForceSetGridPosition(Vector3Int.FloorToInt(levelData.location));
                gt.Build();
            }

            Debug.Log("Loading NPC's");
            foreach (LevelData levelData in levelObjToLoad.npcLevelData)
            {
                GameObject gridTransformObj = GameObject.Instantiate(objectPrefabList.NpcPrefabs[levelData.id]);
                GridTransform gt = gridTransformObj.GetComponent<GridTransform>();
                gt.ForceSetGridPosition(Vector3Int.FloorToInt(levelData.location));
                gt.Build();
            }

            Debug.Log("Loading Decorations");
            foreach(LevelData levelData in levelObjToLoad.decorationLevelData)
            {
                GameObject decorationObj = GameObject.Instantiate(objectPrefabList.decorationPrefabs[levelData.id]);
                decorationObj.transform.position = levelData.location;
                Terrain terrain = tg.CellToTerrain(Vector3Int.RoundToInt(levelData.location));
                if (terrain != null)
                {
                    terrain.AddDecoration(decorationObj);
                }
            }

        }

        Debug.Log("Building Grid");
        tg.BuildGrid();

        Debug.Log("Finished Loading LevelData");
    }
}
