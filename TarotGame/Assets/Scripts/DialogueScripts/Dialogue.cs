using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "TarotGame/DialogueSystem/Dialogue")]

public class Dialogue : ScriptableObject
{
    public string Name = "";
    [TextArea(10,15)]
    public string Text = "";
    public string guid;

    public Vector2 position;

    public Dialogue child;

    public UnityAction<List<GameObject>> OnExplainObjects;
    public UnityAction<GameObject> OnExplainObject;
    public UnityAction<GameObject> OnExplainUI;
    public UnityAction<EventReference> OnSpeakDialogue;

    public List<GameObject> objectsToExplain;
    public GameObject uiToExplain;
    public EventReference dialogueSound;

    public void ExplainObjects(List<GameObject> gameObjects)
    {
        OnExplainObjects?.Invoke(gameObjects);
    }

    public void ExplainObject(GameObject gameObject)
    {
        OnExplainObject?.Invoke(gameObject);
    }
    
    public void ExplainUI(GameObject gameObject)
    {
        OnExplainUI?.Invoke(gameObject);
    }

    public void SpeakDialogue(EventReference sound)
    {
        OnSpeakDialogue?.Invoke(sound);
    }
}

/*
This is test dialogue, thus I am using this time to not only not code the rest of this system but also to test how much I can type before I fill up the entire box in the inspector. It turns out [TextArea(10,15)] is pretty large for a box and should be good enough for any dialogue lines, not to mention the box stretches with the inspector window and thus with how I have it I only have this last line where I am typing last, or not because it seems to add a line before I even got close to the end and only now did the scroll bar has appeared so I shall stop typing now. 
*/