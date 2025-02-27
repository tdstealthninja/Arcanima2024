using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class ConversationEditor : EditorWindow
{
    ConversationView conversationView;
    InspectorView inspectorView;
    ToolbarMenu toolbar;
    [MenuItem("ConversationEditor/Editor ...")]
    public static void OpenWindow()
    {
        ConversationEditor wnd = GetWindow<ConversationEditor>();
        wnd.titleContent = new GUIContent("ConversationEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/ConversationEditor.uxml");
        visualTree.CloneTree(root);
        

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/ConversationEditor.uss");
        
        root.styleSheets.Add(styleSheet);

        conversationView = root.Q<ConversationView>();
        inspectorView = root.Q<InspectorView>();
        toolbar = root.Q<ToolbarMenu>();

        FindConversations();

        conversationView.OnDialogueSelected = OnDialogueSelectionChanged;
        OnSelectionChange();
    }


    private void FindConversations()
    {
        string[] guids = AssetDatabase.FindAssets($"t:Conversation", new[] {"Assets/Scripts/DialogueScripts" });

        toolbar.Q<ToolbarMenu>("Conversations");
        string assetPath = "";

        foreach (string guid in guids)
        {
            assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Conversation conversation = (Conversation)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Conversation));
            toolbar.menu.AppendAction(conversation.name, a => SwapConversation(conversation), a => DropdownMenuAction.Status.Normal, conversation);
        }


    }

    private void OnSelectionChange() {
        Conversation conversation = Selection.activeObject as Conversation;
        if (conversation && AssetDatabase.CanOpenAssetInEditor(conversation.GetInstanceID()))
        {
            conversationView.PopulateView(conversation);
        }
    }

    void OnDialogueSelectionChanged(DialogueView dialogueView)
    {
        inspectorView.UpdateSelection(dialogueView);
    }

    void SwapConversation(Conversation conversation)
    {
        if (conversation && AssetDatabase.CanOpenAssetInEditor(conversation.GetInstanceID()))
        {
            conversationView.PopulateView(conversation);
        }
    }
}