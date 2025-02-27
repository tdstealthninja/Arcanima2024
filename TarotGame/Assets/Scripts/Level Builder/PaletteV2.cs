using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PaletteV2 : MonoBehaviour
{
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject levelButtonPrefab;

    private Placement placement;
    private SaveGTDataManagerV2 saveManager;
    private ItemButton selected;
    private LevelButtonV2 selectedLevel;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveGTDataManagerV2>();
        placement = FindObjectOfType<Placement>();
        MakeFolderButtons();
    }

    public void MakeFolderButtons()
    {
        ClearPallete();

        // Terrain Prefabs ===========================================================================================
        GameObject newItemButton = Instantiate(itemPrefab);
        ItemButton item = newItemButton.GetComponent<ItemButton>();

        // Remove then add to make sure it doesn't double up
        // delegates allow functions to be created at run time
        item.onClicked.RemoveListener(delegate { MakeObjectButtons(saveManager.objectPrefabList.terrainPrefabs); });
        item.onClicked.AddListener(delegate { MakeObjectButtons(saveManager.objectPrefabList.terrainPrefabs); });

        // Set text
        item.SetText("Terrain");

        // Child buttons to content
        newItemButton.transform.SetParent(content);

        // GridTransform Prefabs =====================================================================================
        newItemButton = Instantiate(itemPrefab);
        item = newItemButton.GetComponent<ItemButton>();

        // Remove then add to make sure it doesn't double up
        // delegates allow functions to be created at run time
        item.onClicked.RemoveListener(delegate { MakeObjectButtons(saveManager.objectPrefabList.gridTransformPrefabs); });
        item.onClicked.AddListener(delegate { MakeObjectButtons(saveManager.objectPrefabList.gridTransformPrefabs); });

        // Set text
        item.SetText("Grid Transforms");

        // Child buttons to content
        newItemButton.transform.SetParent(content);

        // Decoration Prefabs ========================================================================================
        newItemButton = Instantiate(itemPrefab);
        item = newItemButton.GetComponent<ItemButton>();

        // Remove then add to make sure it doesn't double up
        // delegates allow functions to be created at run time
        item.onClicked.RemoveListener(delegate { MakeObjectButtons(saveManager.objectPrefabList.decorationPrefabs); });
        item.onClicked.AddListener(delegate { MakeObjectButtons(saveManager.objectPrefabList.decorationPrefabs); });

        // Set text
        item.SetText("Decorations");

        // Child buttons to content
        newItemButton.transform.SetParent(content);

        // NPC's Prefabs ========================================================================================
        newItemButton = Instantiate(itemPrefab);
        item = newItemButton.GetComponent<ItemButton>();

        // Remove then add to make sure it doesn't double up
        // delegates allow functions to be created at run time
        item.onClicked.RemoveListener(delegate { MakeObjectButtons(saveManager.objectPrefabList.NpcPrefabs); });
        item.onClicked.AddListener(delegate { MakeObjectButtons(saveManager.objectPrefabList.NpcPrefabs); });

        // Set text
        item.SetText("NPC's");

        // Child buttons to content
        newItemButton.transform.SetParent(content);
    }

    private void MakeObjectButtons(List<GameObject> objects)
    {
        ClearPallete();
        List<Transform> list = new();

        // Make buttons for each
        foreach (GameObject o in objects)
        {
            GameObject newItemButton = Instantiate(itemPrefab);
            ItemButton item = newItemButton.GetComponent<ItemButton>();
            item.SetItem(o);
            item.SetText(o.name);

            // Remove then add to make sure it doesn't double up
            item.onClicked.RemoveListener(SelectItem);
            item.onClicked.AddListener(SelectItem);

            list.Add(newItemButton.transform);
        }

        var arr = list.ToArray();
        Sort.BubbleSort(arr, (rhs, lhs) => { return string.Compare(rhs.GetComponentInChildren<TextMeshProUGUI>().text, lhs.GetComponentInChildren<TextMeshProUGUI>().text) == 1; });
        foreach (var t in arr)
        {
            t.SetParent(content);
        }
    }

#if UNITY_EDITOR
    public void MakeLevelLoadButtons()
    {
        ClearPallete();

        string path = "Assets/Scripts/SaveData/";
        string fileExtension = ".asset";

        //// Get list of ground pieces
        //var objects = Resources.LoadAll(path, typeof(GameObject));
        string[] files = Directory.GetFiles(path, "*.asset");
        // Make buttons for each
        foreach (var f in files)
        {
            GameObject newItemButton = Instantiate(levelButtonPrefab);
            LevelButtonV2 item = newItemButton.GetComponent<LevelButtonV2>();
            string lvlName = f.Replace(path, "");
            lvlName = lvlName.Replace(fileExtension, "");
            Level lvl = (Level)AssetDatabase.LoadAssetAtPath(f, typeof(Level));
            item.SetLevel(lvl);
            item.SetText(lvlName);

            // Remove then add to make sure it doesn't double up
            item.onClicked.RemoveListener(SelectLevel);
            item.onClicked.AddListener(SelectLevel);

            // Child buttons to content
            newItemButton.transform.SetParent(content);
        }
    }
#endif

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

    public void SelectLevel(LevelButtonV2 newLevelSelected)
    {
        selectedLevel?.ResetColor();
        selectedLevel = newLevelSelected;
        selectedLevel?.SetColor(Color.yellow);

        saveManager.SetCurrentLoadLevel(newLevelSelected?.GetLevel());
    }
}
