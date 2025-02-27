using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public void OnCollected()
    {
        FindObjectOfType<StateChange>().AddCount();
        Destroy(gameObject);
    }
}
