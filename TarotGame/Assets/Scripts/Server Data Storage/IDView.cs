using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IDView : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerID.id;
        transform.parent.gameObject.SetActive(PlayerID.privacyPolicyAccepted);
    }
}
