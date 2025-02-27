using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// This should run before everything else to make sure that the instance is set up
[DefaultExecutionOrder(-1)]
public class ServerAccess : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private ServerInformation serverInformation;

    /// <summary>
    /// Receives array of args as a param
    /// </summary>
    /// <param name="args"></param>
    public delegate void Next(object[] args);

    // Error Messages
    private readonly string loggedInError = "ERROR: Application is not logged in to the server.";

    // status
    public bool LoggedIn { get; private set; }

    #endregion

    #region Actions
    [Header("Events")]
    public UnityEvent onAuthenticationSuccess;
    public UnityEvent onLogout;
    #endregion

    #region Single Instance

    public static ServerAccess Instance
    {
        get
        {
            if (instance == null)
                throw new("WebDataStorage.Instance has not been initialized.");
            else
                return instance;
        }
    }
    private static ServerAccess instance;

    private void Awake()
    {
        // make sure there is only one WebDataStorage that is active
        if (instance != null)
            throw new("ERROR: There is a WebDataStorage that is already logged in. Please make sure to log a WebDataStorage out before starting a new one.");

        // set up this WebDataStorage
        instance = this;
#if UNITY_EDITOR
        Authenticate(); // only authenticate in editor
#endif

        // This makes sure that the WebDataStorage logs out of the server before closing
        Application.wantsToQuit -= OnWantToDestroy;
        Application.wantsToQuit += OnWantToDestroy;
    }

    private bool OnWantToDestroy()
    {
        if (LoggedIn)
        {
            onLogout.AddListener(Application.Quit);
            Logout();
            return false; // not logged out, wait
        }
        else
            return true; // is logged out, close
    }

    #endregion

    #region Authenticate

    public void Authenticate()
    {
        StartCoroutine(PostAuthentication());
    }

    private IEnumerator PostAuthentication()
    {
        // make form
        List<IMultipartFormSection> wwwForm = new();
        wwwForm.Add(new MultipartFormDataSection("key", serverInformation.AuthenticationKey));

        // setup request
        using UnityWebRequest www = UnityWebRequest.Post(serverInformation.AuthenticationLink, wwwForm);

        // send
        yield return www.SendWebRequest();

        // check for error
        if (www.error != null)
            throw new(www.error);

        if (www.downloadHandler.text.Contains("loggedin"))
        {
            onAuthenticationSuccess?.Invoke();
            LoggedIn = true;
        }
    }

    public void Logout()
    {
        StartCoroutine(PostLogout());
    }

    private IEnumerator PostLogout()
    {
        using UnityWebRequest www = UnityWebRequest.Post(serverInformation.LogoutLink, serverInformation.LogoutLink);

        yield return www.SendWebRequest();

        onLogout?.Invoke();
        LoggedIn = false;
    }

    #endregion

    #region Save

    /// <summary>
    /// This serializes an object to a string and then saves it to a server.
    /// </summary>
    /// <param name="o">Object to be saved</param>
    public void Store(string data)
    {
        // send to server
        StartCoroutine(PostStore(data));
    }

    private IEnumerator PostStore(string data)
    {
        // make form
        List<IMultipartFormSection> wwwForm = new();
        wwwForm.Add(new MultipartFormDataSection("key", serverInformation.AuthenticationKey));
        wwwForm.Add(new MultipartFormDataSection("data", data));

        // setup request
        using UnityWebRequest www = UnityWebRequest.Post(serverInformation.StoreLink, wwwForm);

        // send
        yield return www.SendWebRequest();

        // check for error
        if (www.error != null)
            Debug.LogError(www.error);
        string text = www.downloadHandler.text;
        if (text.Contains("Failed: Not Logged In")) throw new(loggedInError);
        if (text != "")
            Debug.Log(www.downloadHandler.text);
    }

    #endregion

    #region Load

    /// <summary>
    /// Returns loaded object as the first object in the args param.
    /// </summary>
    /// <param name="next"></param>
    public void Load(string filePath, Next next)
    {
        StartCoroutine(PostLoad(filePath, next));
    }
    private IEnumerator PostLoad(string filePath, Next next)
    {
        // make form
        List<IMultipartFormSection> wwwForm = new();
        wwwForm.Add(new MultipartFormDataSection("path", filePath));

        // setup request
        using UnityWebRequest www = UnityWebRequest.Post(serverInformation.LoadLink, wwwForm);

        // send
        yield return www.SendWebRequest();

        // check for error
        string text = www.downloadHandler.text;
        if (www.error != null)
            Debug.LogError(www.error);
        else if (text.Contains("Failed: Not Logged In")) throw new(loggedInError);
        else
        {
            string[] files = new string[1];
            files[0] = text;
            next.Invoke(files);
        }
    }

    #endregion

    #region List

    /// <summary>
    /// Gets a list of folders that are stored on the server.
    /// </summary>
    /// <returns>List of folder names.</returns>
    public void List(Next next) => List(next: next);
    /// <summary>
    /// Gets a list of folders that are stored on the server.
    /// </summary>
    /// <returns>List of folder names.</returns>
    public void List(string folder = "", Next next = null)
    {
        StartCoroutine(GetList(folder, next));
    }
    /// <summary>
    /// Returns list of folders stored by date on the server.
    /// </summary>
    /// <returns>List of folder names.</returns>
    private IEnumerator GetList(string folder, Next next)
    {
        // make form
        List<IMultipartFormSection> wwwForm = new();
        if (folder != "")
            wwwForm.Add(new MultipartFormDataSection("folder", folder));

        // setup request
        using UnityWebRequest www = UnityWebRequest.Post(serverInformation.ListLink, wwwForm);

        // send
        yield return www.SendWebRequest();

        // check for error
        string text = www.downloadHandler.text;
        if (www.error != null)
            Debug.LogError(www.error);
        else if (text.Contains("Failed: Not Logged In")) throw new(loggedInError);
        else
        {
            string[] files = text.Split(']'); // split so that the list of files is first
            files = files[0].Split(','); // grab list of files and split them by ','
            next.Invoke(files);
        }
    }

    #endregion
}
