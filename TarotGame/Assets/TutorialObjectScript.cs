using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjectScript : MonoBehaviour
{
    //public TutorialListScript tutorial;
    public static bool explained = false;

    private void Start()
    {
        FindObjectOfType<StateChange>().onStateChange.AddListener(() => OnExplain());

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Explain()
    {
        explained = true;
    }

    public bool IsExplained()
    {
        return TutorialObjectScript.explained;
    }

    private void OnDisable()
    {
        
        TutorialListScript.ObjExplain(this.gameObject.transform.parent.gameObject);
        TutorialListScript.DisableAlreadyExplained();
    }

    public void OnExplain()
    {
        
        TutorialListScript.ObjExplain(this.gameObject.transform.parent.gameObject);
        TutorialListScript.DisableAlreadyExplained();
    }


}
