using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class SaveGTDataManager : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<SaveGTDataManager> onLoad;

    public GameObject savefileInputButton;
    private readonly string SAVE_FILE_PATH = "Assets/Resources/LevelSaves/";
    private readonly string SAVE_FILE_EXSTENTION = ".bytes";
    private string fileName;
    private string levelToLoad = null;

    #region Getters Setters and Checks

    public void SetCurrentLoadLevel(string loadLevel)
    {
        levelToLoad = loadLevel;

        Load();
    }

    public string GetCurrentLoadLevel() => levelToLoad;

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
        // convert level to data
        TerrainGrid tg = FindObjectOfType<TerrainGrid>();
        TerrainGridData tgd = new(tg);

        // write data to file
        BinaryFormatter bf = new();
        FileStream file = new(SAVE_FILE_PATH + fileName + SAVE_FILE_EXSTENTION, FileMode.Create);
        bf.Serialize(file, tgd);

        // close file
        file.Close();
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

        // create level
        tgd.ToObject();
        Debug.Log("Finished Loading TerrainGridData");
    }
}
