using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Conversable : Interactable
{
    public UnityEvent onConversed;
    public UnityEvent onFinished;
    //public Conversation conversation;
    public List<Conversation> conversations;
    public Sprite npcImage;
    public GameObject npcAnimObj;
    [SerializeField]
    private GameObject createdAnimObj;
    [SerializeField]
    private DialogueManager dialogueManager;
    private int currentConvo = 0;
    [SerializeField]
    private string talkingAnimName;
    private string typeAnim = "Talking";
    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        npcImage = GetComponent<SpriteRenderer>().sprite;
    }

    public override bool Interact(Interactor interactor)
    {
        onConversed.Invoke();
        onInteractionSuccess?.Invoke(interactor, this);
        return true;
    }

    public override void OnStateChange()
    {
        // do nothing
    }

    public void StartConveration()
    {
        //dialogueManager.conversation = conversation;
        //dialogueManager.StartConversation();
        dialogueManager.conversation = conversations[currentConvo];
        dialogueManager.StartConversation();
        dialogueManager.SetSprite(npcImage);
        
        Debug.Log(npcAnimObj);
        if (npcAnimObj != null)
        {
            dialogueManager.npcAnimObj = Instantiate(npcAnimObj);
            dialogueManager.npcAnimObj.transform.parent = dialogueManager.npcObj.transform;
            Vector3 pos = new Vector3(0, -221, -18);
            Vector3 scal = new Vector3(43, 43, 1);
            
            dialogueManager.npcAnimObj.transform.localPosition = pos;
           
            dialogueManager.npcAnimObj.transform.localScale = scal;
            dialogueManager.npcAnimObj.transform.SetPositionAndRotation(dialogueManager.npcAnimObj.transform.position, new Quaternion(0,0,0,0));
            dialogueManager.npcAnimObj.GetComponent<Animator>().SetBool("talking", true);

            
            
        }
        if (currentConvo + 1 < conversations.Count)
            currentConvo++;
        else
        {
            dialogueManager.onEnd.RemoveListener(OnConversationEnded);
            dialogueManager.onEnd.AddListener(OnConversationEnded);
        }
    }

    private void OnConversationEnded()
    {
        dialogueManager.onEnd.RemoveListener(OnConversationEnded);
        onFinished?.Invoke();
        
    }
}
