using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;


public class DialogueView : UnityEditor.Experimental.GraphView.Node
{
    public Action<DialogueView> OnDialogueSelected;
    public Dialogue dialogue;
    public Port input;
    public Port output;

    public DialogueView(Dialogue dialogue)
    {
        this.dialogue = dialogue;
        this.title = dialogue.name;
        this.viewDataKey = dialogue.guid;
        

        style.left = dialogue.position.x;
        style.top = dialogue.position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(dialogue, "Conversation (Set Position)");
        dialogue.position.x = newPos.xMin;
        dialogue.position.y = newPos.yMin;
        EditorUtility.SetDirty(dialogue);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnDialogueSelected != null)
        {
            OnDialogueSelected.Invoke(this);
        }
    }

    private void CreateInputPorts()
    {
        if (dialogue is RootDialogue)
        {
            
        }
        else
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        }

        if (input != null)
        {
            input.portName = "";
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (dialogue is RootDialogue)
        {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        if (dialogue is Branch)
        {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (dialogue is Dialogue)
        {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null)
        {
            output.portName = "";
            outputContainer.Add(output);
        }
    }

    

    
}
