using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    [SerializeField]
    private GameObject privacyPolicyPanel;
    [SerializeField]
    private TextMeshProUGUI idView;

    private static bool viewed = false;
    public static bool privacyPolicyAccepted = false;
    public static string id { get; private set; }

    void Awake()
    {
        if (viewed)
            ClosePrivacyPolicy();
        else
        {
            CreateID();
            idView.text = id;
        }
    }

    private static void CreateID()
    {
        id = DateTime.Now.ToString("MMddyyyyHHmm"); // get timedate
        long idNumber = long.Parse(id); // convert to int
        id = idNumber.ToString("X8"); // convert to hex string
    }

    public void Accept()
    {
        privacyPolicyAccepted = true;
        ClosePrivacyPolicy();
    }

    public void Decline() => ClosePrivacyPolicy();

    private void ClosePrivacyPolicy()
    {
        viewed = true;
        privacyPolicyPanel.SetActive(false);
    }

    private static void OnAccepted() => privacyPolicyAccepted = true;
}
