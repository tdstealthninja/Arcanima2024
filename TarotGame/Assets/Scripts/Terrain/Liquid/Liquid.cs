using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Liquid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Combines a section of liquid into one object.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void Combine(int start, int end)
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
            OnPlayerCollision(collision);
    }

    protected abstract void OnPlayerCollision(Collision collision);
}
