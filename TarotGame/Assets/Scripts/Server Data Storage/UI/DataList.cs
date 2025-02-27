using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataList : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private ServerAccess serverAccess;

    public UnityEvent<string> onFileLoaded;

    private List<string> options = new();
    private string folder = "";
    private readonly string loadingMessage = "Loading Options...";
    private readonly string nullOption = "Loaded";

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        serverAccess = ServerAccess.Instance;

        serverAccess.onAuthenticationSuccess.RemoveListener(OnAuthenticate);
        serverAccess.onAuthenticationSuccess.AddListener(OnAuthenticate);
    }

    // fixes error with adding UpdateOptions() to onAuthenticationSuccess event 
    private void OnAuthenticate() => UpdateOptions();

    public void UpdateOptions(string folder = "")
    {
        dropdown.ClearOptions();
        options.Clear();
        options.Add(loadingMessage);
        dropdown.AddOptions(options);
        dropdown.value = 0;

        serverAccess.List(folder, (args) =>
        {
            options = new(args as string[]);
            options.Insert(0, nullOption); // no option selected
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
        });
    }

    public void OnSelectionChanged(int index)
    {
        // dont do anything when loading or with the no option text
        if (options[index] == loadingMessage || options[index] == nullOption)
            return;

        if(folder == "")
        {
            // selects a folder to view
            folder = options[index];
            UpdateOptions(folder);
        }
        else
        {
            // create path string
            string path = folder + '/' + options[index];

            // loads file
            serverAccess.Load(path, (args) =>
            {
                // go back to root folder
                folder = "";
                UpdateOptions();

                // sends file data to whoever needs it
                onFileLoaded?.Invoke(args[0] as string);
            });
        }
    }
}
