using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Liquid
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void OnPlayerCollision(Collision collision)
    {
        // Water doesn't do anything to the player
    }
}
