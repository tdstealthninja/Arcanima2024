using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Palette : MonoBehaviour
{
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject levelButtonPrefab;

    // This is the list of folders that contain GameObjects to be placed in the scene
    private string[] folders = new string[]
    {
        "Terrain/Ground/",
        "Object/",
        "Decorations/"
    };

    private Placement placement;
    private SaveGTDataManager saveManager;
    private ItemButton selected;
    private LevelButton selectedLevel;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveGTDataManager>();
        placement = FindObjectOfType<Placement>();
        MakeFolderButtons();
    }

    private string ParseFolderName(string folderPath)
    {
        string[] folderArray = folderPath.Split('/', StringSplitOptions.RemoveEmptyEntries); // splits folder path into individual folders
        string folderName = folderArray.Last(); // gets the last folder in path
        return folderName;
    }

    public void MakeFolderButtons()
    {
        ClearPallete();

        foreach (string folder in folders)
        {
            GameObject newItemButton = Instantiate(itemPrefab);
            ItemButton item = newItemButton.GetComponent<ItemButton>();
            item.SetText(ParseFolderName(folder));

            // Remove then add to make sure it doesn't double up
            // delegates allow functions to be created at run time
            item.onClicked.RemoveListener(delegate { MakeObjectButtons(folder); });
            item.onClicked.AddListener(delegate { MakeObjectButtons(folder); });

            // Child buttons to content
            newItemButton.transform.SetParent(content);
        }
    }

    private void MakeObjectButtons(string path)
    {
        ClearPallete();

        // Get list of ground pieces
        var objects = Resources.LoadAll(path, typeof(GameObject));
        // Make buttons for each
        foreach (var o in objects)
        {
            GameObject newItemButton = Instantiate(itemPrefab);
            ItemButton item = newItemButton.GetComponent<ItemButton>();
            item.SetItem(o as GameObject);
            item.SetText(o.name);

            // Remove then add to make sure it doesn't double up
            item.onClicked.RemoveListener(SelectItem);
            item.onClicked.AddListener(SelectItem);

            // Child buttons to content
            newItemButton.transform.SetParent(content);
        }
    }

    public void MakeLevelLoadButtons()
    {
        ClearPallete();

        string path = "Assets/Resources/LevelSaves/";
        string fileExtension = ".bytes";

        //// Get list of ground pieces
        //var objects = Resources.LoadAll(path, typeof(GameObject));
        string[] files = Directory.GetFiles(path, "*.bytes");
        // Make buttons for each
        foreach (var f in files)
        {
            GameObject newItemButton = Instantiate(levelButtonPrefab);
            LevelButton item = newItemButton.GetComponent<LevelButton>();
            string lvlName = f.Replace(path,"");
            lvlName = lvlName.Replace(fileExtension, "");
            item.SetFileName(lvlName);
            item.SetText(lvlName);

            // Remove then add to make sure it doesn't double up
            item.onClicked.RemoveListener(SelectLevel);
            item.onClicked.AddListener(SelectLevel);

            // Child buttons to content
            newItemButton.transform.SetParent(content);
        }
    }

    private void ClearPallete()
    {
        Transform[] children = content.GetComponentsInChildren<Transform>();

        SelectItem(null);

        for (int i = children.Length - 1; i >= 0; i--)
        {
            Transform child = children[i];
            if (child != content)
                Destroy(child.gameObject);
        }
    }

    public void SelectItem(ItemButton newSelected)
    {
        selected?.ResetColor();
        selected = newSelected;
        selected?.SetColor(Color.yellow);

        placement.SetCurrent(selected?.GetItem());
    }

    public void SelectLevel(LevelButton newLevelSelected)
    {
        selectedLevel?.ResetColor();
        selectedLevel = newLevelSelected;
        selectedLevel?.SetColor(Color.yellow);

        saveManager.SetCurrentLoadLevel(newLevelSelected?.GetFileName());
    }
}
