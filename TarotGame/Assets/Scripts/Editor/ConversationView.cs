using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using GView = UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;

public class ConversationView : GView.GraphView
{
    public Action<DialogueView> OnDialogueSelected;
    public new class UxmlFactory : UxmlFactory<ConversationView, GView.GraphView.UxmlTraits> { }
    Conversation conversation;
    

    public ConversationView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/ConversationEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;


    }

    private void OnUndoRedo()
    {
        PopulateView(conversation);
        AssetDatabase.SaveAssets();
    }

    DialogueView FindDialogueView(Dialogue dialogue)
    {
        DialogueView dialogueView = GetNodeByGuid(dialogue.guid) as DialogueView;
        return dialogueView;
    }

    internal void PopulateView(Conversation conversation)
    {
        if (conversation != null)
        {
            this.conversation = conversation;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (conversation.rootDialogue == null)
            {
                conversation.rootDialogue = conversation.CreateDialogue(typeof(RootDialogue)) as RootDialogue;
                EditorUtility.SetDirty(conversation);
                AssetDatabase.SaveAssets();
            }

            // Creates Dialogue view
            conversation.dialogues.ForEach(d => CreateDialogueView(d));

            // Creates Edges
            conversation.dialogues.ForEach(d => {
                var children = conversation.GetChildren(d);
                children.ForEach(c => {
                    DialogueView parentView = FindDialogueView(d);
                    DialogueView childView = FindDialogueView(c);

                    Branch branch = d as Branch;
                    if (branch)
                    {
                        branch.options.ForEach(o => {
                            DialogueView childView2 = FindDialogueView(c.child);
                            Edge edge2 = childView.output.ConnectTo(childView2.input);
                            if (edge2 != null)
                                AddElement(edge2);
                        });

                    }

                    Edge edge = parentView.output.ConnectTo(childView.input);
                    if (edge != null)
                        AddElement(edge);
                });
            });
        }
        
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => 
        endPort.direction != startPort.direction && 
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem => {
                DialogueView dialogueView = elem as DialogueView;
                if (dialogueView != null)
                {
                    conversation.DeleteDialogue(dialogueView.dialogue);
                }
                
                Edge edge = elem as Edge;
                if (edge != null)
                {
                    DialogueView parentView = edge.output.node as DialogueView;
                    DialogueView childView = edge.input.node as DialogueView;
                    conversation.RemoveChild(parentView.dialogue, childView.dialogue);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge => {
                DialogueView parentView = edge.output.node as DialogueView;
                DialogueView childView = edge.input.node as DialogueView;
                conversation.AddChild(parentView.dialogue, childView.dialogue);
            });
        }
        

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        var dialogueType = typeof(Dialogue);
        var branchType = typeof(Branch);
        //var optionType = typeof(Option);
        evt.menu.AppendAction("Dialogue", (a) => CreateDialogue(dialogueType));
        evt.menu.AppendAction("Branch", (a) => CreateDialogue(branchType));
        //evt.menu.AppendAction("Option", (a) => CreateDialogue(optionType));
    }

    void CreateDialogue(System.Type type)
    {
        Dialogue dialogue = conversation.CreateDialogue(type);
        CreateDialogueView(dialogue);
    }


    void CreateDialogueView(Dialogue dialogue)
    {
        DialogueView dialogueView = new DialogueView(dialogue);
        dialogueView.OnDialogueSelected = OnDialogueSelected;
        AddElement(dialogueView);
    }

}
