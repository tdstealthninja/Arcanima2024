using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ServerInformationScriptableObject", menuName = "ScriptableObjects/ServerInformation")]
public class ServerInformation : ScriptableObject
{
    // Variables
    [SerializeField]
    private string authenticationKey;
    [SerializeField]
    private string authenticationLink;
    [SerializeField]
    private string logoutLink;
    [SerializeField]
    private string storeLink;
    [SerializeField]
    private string loadLink;
    [SerializeField]
    private string listLink;

    // Accessors
    public string AuthenticationKey { get { return authenticationKey; } private set { authenticationKey = value; } }
    public string AuthenticationLink { get { return authenticationLink; } private set { authenticationLink = value; } }
    public string LogoutLink { get { return logoutLink; } private set { logoutLink = value; } }
    public string StoreLink { get { return storeLink; } private set { storeLink = value; } }
    public string LoadLink { get { return loadLink; } private set { loadLink = value; } }
    public string ListLink { get { return listLink; } private set { listLink = value; } }
}
