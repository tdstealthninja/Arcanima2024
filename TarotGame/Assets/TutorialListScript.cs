using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialListScript : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public static List<GameObject> tutorialObjs = new List<GameObject>();
    public static List<bool> objsExplained = new List<bool>();
     // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gameObject in objects)
        {
            if (!TutorialListScript.tutorialObjs.Contains(gameObject))
            {
                TutorialListScript.tutorialObjs.Add(gameObject);
            }
        }
        if (TutorialListScript.objsExplained.Count <= 0)
        {
            for (int i = 0; i < tutorialObjs.Count; i++)
            {
                TutorialListScript.objsExplained.Add(false);
            }
        }
        //objsExplained = new List<bool>();

        // objsExplained.Capacity = tutorialObjs.Capacity;
        FindObjectOfType<StateChange>().onStateChange.AddListener(() => DisableAlreadyExplained());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ObjExplain(GameObject gameObject)
    {
        
        string name = gameObject.name.Replace("(Clone)", "");
        //Debug.LogWarning(name);
        //Debug.LogWarning(tutorialObjs.Contains(gameObject));
        int current = 0;
        foreach (GameObject obj in tutorialObjs)
        {
            if (obj.name == name)
            {
                if (TutorialListScript.objsExplained.Count > 0 && current < TutorialListScript.objsExplained.Count)
                {
                    TutorialListScript.objsExplained[current] = true;
                    //Debug.LogError(TutorialListScript.objsExplained[current]);
                    //objsExplained[tutorialObjs.IndexOf(obj)] = true;
                }
            }
            current++;
        }

        
    }

    public static void DisableAlreadyExplained()
    {
        foreach( TutorialObjectScript gameObject in GameObject.FindObjectsOfType<TutorialObjectScript>())
        {
            GameObject tutorialObj = gameObject.transform.parent.gameObject;
            if (tutorialObj != null)
            {
                string name = tutorialObj.name.Replace("(Clone)", "");
                bool explainedInPrevRelm = true;
                int current = 0;
                
                foreach (GameObject obj in TutorialListScript.tutorialObjs)
                {
                    if (obj.name == name)
                    {
                        explainedInPrevRelm = false;
                        if (TutorialListScript.objsExplained.Count > 0 && current < TutorialListScript.objsExplained.Count)
                        {
                            //Debug.LogError(TutorialListScript.objsExplained[current]);
                            if (TutorialListScript.objsExplained[current] == true)
                            {
                                gameObject.gameObject.SetActive(false);
                            }
                        }
                    }
                    current++;
                }
                if(explainedInPrevRelm)
                {
                    gameObject.gameObject.SetActive(false);
                }
                current = 0;
            }
        }
    }    
}
