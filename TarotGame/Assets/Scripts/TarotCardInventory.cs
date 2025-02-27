using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TarotCardInventory : MonoBehaviour
{
    public List<bool> cardEntriesFound = new List<bool>();
    public List<TarotCardEntry> cardEntries = new List<TarotCardEntry>();

    [SerializeField]
    TarotCardEntry defaultEntry;
    [SerializeField]
    int currentEntry = 0;

    [SerializeField]
    TextMeshProUGUI cardName;
    [SerializeField]
    Image cardImage;
    [SerializeField]
    TextMeshProUGUI cardDescription;

    //Awake runs before Start
    private void Awake()
    {
        //cardEntriesFound = new List<bool>();
        //cardEntries = new List<TarotCardEntry>();
    }

    // Start is called before the first frame update, after Awake
    void Start()
    {
        ListResize(cardEntriesFound, cardEntries.Count);
        ShowEntry(currentEntry);
    }

    // Update is called once per frame
    void Update()
    {
        if (cardEntriesFound.Count != cardEntries.Count)
        {
            ListResize(cardEntriesFound, cardEntries.Count);
        }

    }

    void ListResize<T>(List<T> list, int size)
    {
        if (size > list.Count)
            while (size - list.Count > 0)
                list.Add(default);
        else if (size < list.Count)
            while (list.Count - size > 0)
                list.RemoveAt(list.Count - 1);
    }


    public void NextEntry()
    {
        if (currentEntry < cardEntries.Count -1)
            currentEntry++;
        ShowEntry(currentEntry);
    }

    public void PrevEntry()
    {
        if (currentEntry > 0)
            currentEntry--;
        ShowEntry(currentEntry);
    }

    public void ShowEntry(int entryNum)
    {
        if (cardEntriesFound[entryNum])
        {
            cardName.text = cardEntries[entryNum].name;
            cardImage.sprite = cardEntries[entryNum].card;
            cardDescription.text = cardEntries[entryNum].description;
        }
        else
        {
            cardName.text = defaultEntry.name;
            cardImage.sprite = defaultEntry.card;
            cardDescription.text = defaultEntry.description;
        } 
            
        
    }
}
