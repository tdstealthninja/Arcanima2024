using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "TarotGame/DialogueSystem/Conversation")]
public class Conversation : ScriptableObject
{
    public Dialogue rootDialogue;
    public List<Dialogue> dialogues = new List<Dialogue>();
    //public List<Sound> dialogueLines = new List<Sound>();

#if UNITY_EDITOR
    public Dialogue CreateDialogue(System.Type type) 
    {
        Dialogue dialogue = ScriptableObject.CreateInstance(type) as Dialogue;
        dialogue.name = type.Name;
        dialogue.guid = GUID.Generate().ToString();
        
        Undo.RecordObject(this, "Conversation (CreateNode)");

        dialogues.Add(dialogue);
        EditorUtility.SetDirty(dialogue);

        AssetDatabase.AddObjectToAsset(dialogue, this);
        Undo.RegisterCreatedObjectUndo(dialogue, "Conversation (CreateDialogue)");
        AssetDatabase.SaveAssets();
        return dialogue;

    }

    public void DeleteDialogue(Dialogue dialogue)
    {
        Undo.RecordObject(this, "Conversation (DeleteNode)");
        dialogues.Remove(dialogue);

        Undo.DestroyObjectImmediate(dialogue);
        //AssetDatabase.RemoveObjectFromAsset(dialogue);
        AssetDatabase.SaveAssets();

    }

    public void AddChild(Dialogue parent, Dialogue child)
    {
        Branch branch = parent as Branch;
        //Option option = child as Option;
        if (branch)
        {
            Undo.RecordObject(branch, "Conversation (AddChild)");
            branch.options.Add(child);
            EditorUtility.SetDirty(branch);
        }

        RootDialogue rootDialogue = parent as RootDialogue;
        if (rootDialogue)
        {
            Undo.RecordObject(rootDialogue, "Conversation (AddChild)");
            rootDialogue.child = child;
            EditorUtility.SetDirty(rootDialogue);
        }
        
        else if (parent != null && child != null)
        {
            Undo.RecordObject(parent, "Conversation (AddChild)");
            parent.child = child;
            EditorUtility.SetDirty(parent);
        }
    }
    
    public void RemoveChild(Dialogue parent, Dialogue child)
    {
        Branch branch = parent as Branch;
        //Option option = child as Option;
        if (branch)
        {
            Undo.RecordObject(branch, "Conversation (RemoveChild)");
            branch.options.Remove(child);
            EditorUtility.SetDirty(branch);
        }
        Branch bc = child as Branch;

        if (bc)
        {
            for (int i = 0; i < bc.options.Count; i++)
            {
                RemoveChild(bc, bc.options[i]);
            }
        }

        

        RootDialogue rootDialogue = parent as RootDialogue;
        if (rootDialogue)
        {
            Undo.RecordObject(rootDialogue, "Conversation (RemoveChild)");
            rootDialogue.child = null;
            EditorUtility.SetDirty(branch);
        }

        else if (parent != null && child != null)
        {
            Undo.RecordObject(parent, "Conversation (RemoveChild)");
            parent.child = null;
            EditorUtility.SetDirty(branch);
        }
    }
#endif

    public List<Dialogue> GetChildren(Dialogue parent)
    {
        List<Dialogue> children = new List<Dialogue>();
        Branch branch = parent as Branch;
        RootDialogue rootDialogue = parent as RootDialogue;
        if (branch)
        {
            return branch.options;
        }
        if (rootDialogue && rootDialogue.child != null)
        {
            children.Add(rootDialogue.child);
        }
        else if (parent.child != null)
        {
            children.Add(parent.child);
        }
        return children;
    }

    //public List<Option> GetOptions(Branch branch)
    //{
    //    return branch.options;
    //}

    public Dialogue GetChild(Dialogue parent)
    {
        return parent.child;
    }
    

}
