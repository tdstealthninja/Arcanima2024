using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using FMODUnity;

public class DialogueManager : MonoBehaviour
{
    public Conversation conversation;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public float textScrollDelay = 0.1f;
    public Button continueButton;
    public GameObject optionButtonPrefab;
    public RectTransform buttonLayout;
    public GameObject dialogueWindow;
    public Image npcImage;
    public GameObject npcObj;
    public GameObject tutroialLayout;
    public GameObject otherUI;
    public GameObject npcAnimObj;

    private Queue<string> sentances;
    private Queue<string> names;
    private string currentSentance = "";
    private string currantName = "";
    private Dialogue currentDialogue;
    [SerializeField]
    private int numWordsInButton = 4;
    private bool finishSentance = true;
    private bool buttonsDisabled = false;
    private StudioEventEmitter studioEventEmitter;
    private List<GameObject> objectsBeingExplained;
    [SerializeField]
    private GameObject tutorialObjPrefab;
    FMOD.Studio.EventInstance soundeventInstance;


    public UnityEvent onSpeak;
    public UnityEvent onEnd;

    private void Start()
    {
        sentances = new Queue<string>();
        names = new Queue<string>();
        studioEventEmitter = GetComponent<StudioEventEmitter>();

    }

    public void StartDialogue(Dialogue dialogue)
    {
        UnityEngine.Debug.Log("Starting Dailogue with " + dialogue.Name);
    }

    public void StartConversation()
    {
        if (!dialogueWindow.activeSelf)
        {
            dialogueWindow.SetActive(true);
            ToggleButtons();

        }
        Dialogue dialogue = conversation.rootDialogue;
        currentDialogue = conversation.rootDialogue;
        nameText.text = currentDialogue.Name;
        UnityEngine.Debug.Log("Starting conversation with " + currentDialogue.Name);
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm)
            FindObjectOfType<PlayerMovement>().moveEnabled = false; // disable player movement while talking

        sentances.Clear();

        QueueDialogue(dialogue);

        DisplayNextSentance();
    }

    public void DisplayNextSentance()
    {

        if (dialogueText.text.ToString() != currentSentance && finishSentance)
        {
            StopAllCoroutines();
            dialogueText.text = "";
            dialogueText.text = currentSentance;
            return;
        }

        if (!currentDialogue.dialogueSound.IsNull)
        {
            StopDialogue();
            if (currentDialogue?.dialogueSound != null)
                StartDialogueSound(currentDialogue.dialogueSound);
        }


        if (currentDialogue is Branch)
        {
            Branch branch = currentDialogue as Branch;
            continueButton.gameObject.SetActive(false);
            foreach (Dialogue dialogue in branch.options)
            {
                GameObject newOptionButton = Instantiate(optionButtonPrefab);
                DialogueButton dialogueButton = newOptionButton.GetComponent<DialogueButton>();
                dialogueButton.SetDialogue(dialogue);
                string[] words = dialogue.Text.Split();
                string buttonText = "";

                if (words.Length >= numWordsInButton)
                {
                    for (int i = 0; i < numWordsInButton; i++)
                    {
                        buttonText += words[i];
                        buttonText += " ";
                    }
                }

                else
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        buttonText += words[i];
                        buttonText += " ";
                    }
                }


                dialogueButton.SetText(buttonText);
                dialogueButton.onClicked.RemoveListener(ChooseBranchOption);
                dialogueButton.onClicked.AddListener(ChooseBranchOption);

                dialogueButton.transform.SetParent(buttonLayout, false);
            }
        }

        if (names.Count == 0 || sentances.Count == 0)
        {
            EndDialogue();
            return;
        }



        string sentance = sentances.Dequeue();
        string npcName = names.Dequeue();

        //Debug.Log(npcName);
        //Debug.Log(sentance);

        nameText.text = npcName;
        currentSentance = sentance;
        if (currentDialogue.child != null)
            currentDialogue = currentDialogue.child;
        StopAllCoroutines();

        //if (currentDialogue.objectsToExplain.Count > 0)
        //    ExplainObjects(currentDialogue.objectsToExplain);
        StartCoroutine(TypeSentance(sentance));

        finishSentance = true;
    }

    public List<Dialogue> GetBranchOptions(Branch branch)
    {
        return branch.options;
    }

    public void QueueDialogue(Dialogue dialogue)
    {
        while (dialogue is not Branch && dialogue != null)
        {
            sentances.Enqueue(dialogue.Text);
            names.Enqueue(dialogue.Name);
            //if (!dialogue.dialogueSound.IsNull)
            //    dialogue.OnSpeakDialogue += StartDialogueSound;
            //if (dialogue.objectsToExplain.Count > 0)
            //    dialogue.OnExplainObjects += ExplainObjects;
            dialogue = dialogue.child;
        }
        if (dialogue is Branch)
        {
            sentances.Enqueue(dialogue.Text);
            names.Enqueue(dialogue.Name);
            //if (!dialogue.dialogueSound.IsNull)
            //    dialogue.OnSpeakDialogue += StartDialogueSound;
            //if (dialogue.objectsToExplain.Count > 0)
            //    dialogue.OnExplainObjects += ExplainObjects;
        }
    }

    public void EndDialogue()
    {
        Destroy(npcAnimObj);
        npcAnimObj = null;
        UnityEngine.Debug.Log("End of Conversation");
        dialogueWindow.SetActive(false);
        ToggleButtons();
        FindObjectOfType<PlayerMovement>().moveEnabled = true; // enable player movement when done talking
        onEnd?.Invoke();
        StopDialogue();
        //StopExplainingObjects();
    }

    public void ChooseBranchOption(DialogueButton dialogueButton)
    {
        QueueDialogue(dialogueButton.GetDialogue());
        finishSentance = false;
        DisplayNextSentance();
        continueButton.gameObject.SetActive(true);
        DialogueButton[] buttons = FindObjectsOfType<DialogueButton>();
        foreach (DialogueButton db in buttons)
        {
            Destroy(db.gameObject);
        }
        //dialogueButton.gameObject.SetActive(false);
    }

    IEnumerator TypeSentance(string sentance)
    {
        dialogueText.text = "";


        foreach (char letter in sentance.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textScrollDelay);
            //yield return null;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        npcImage.sprite = sprite;
    }

    public void TempSetSprite(Sprite sprite)
    {
        npcImage.sprite = sprite;
    }

    public void ToggleButtons()
    {
        if (!buttonsDisabled)  // If buttons are active
        {
            otherUI.SetActive(false);
            buttonsDisabled = true;
        }
        else   // If buttons are disabled
        {
            otherUI.SetActive(true);
            buttonsDisabled = false;
        }

    }

    public void StartDialogueSound(EventReference sound)
    {
        soundeventInstance = RuntimeManager.CreateInstance(sound);

        //soundeventInstance.setVolume(10f);
        soundeventInstance.start();
        //Debug.Log(sound);
        //studioEventEmitter.EventReference = sound;
        //studioEventEmitter.EventReference.Path = sound.Path;
        //Debug.Log(sound);
        //studioEventEmitter.Play();
        //onSpeak?.Invoke();
    }

    public void StopDialogue()
    {
        //onEnd?.Invoke();
        //studioEventEmitter.Stop();
        soundeventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //Debug.Log("Stopping dialogue");
    }

    public void ExplainObjects(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            GameObject tutorialObj = Instantiate(tutorialObjPrefab);
            tutorialObj.transform.parent = tutroialLayout.transform;
            tutorialObj.GetComponent<Image>().sprite = gameObject.GetComponent<Sprite>();
            //tutorialObj.GetComponentInChildren<Image>().sprite = 
        }
    }



    public void StopExplainingObjects()
    {
        foreach (GameObject explainedObj in objectsBeingExplained)
        {
            explainedObj.SetActive(false);
        }
        objectsBeingExplained.Clear();
    }

}


