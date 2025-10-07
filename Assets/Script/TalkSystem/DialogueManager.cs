using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public DialogueData dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public Image portraitImage;
    public GameObject choiceButtonPrefab;
    public Transform choiceContainer;
    public Button nextButton;


    private int currentID = 0;
    private GameObject currentNPC;
    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel.SetActive(false);

    }
    public void StartDialogue(DialogueData data,GameObject npcObj)
    {
        dialogueData = data;
        currentNPC = npcObj;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.GetComponent<RiraMove>().canMove = false;

        var agent = currentNPC.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;


        dialoguePanel.SetActive(true);
        ShowLine(0); // 最初の会話行を表示
        Debug.Log("会話フレーム表示");
    }

    void ShowLine(int id)
    {
        if(dialogueData == null || dialogueData.lines == null || id < 0 || id >= dialogueData.lines.Count)
        {
            Debug.LogError($"不正な会話ID:{id}");
            EndDialogue();
            return;
        }
        currentID = id;
        var line = dialogueData.lines[id];

        speakerText.text = line.SpeakerName;
        dialogueText.text = line.text;
        portraitImage.sprite = line.portrait;

        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);

        if(line.choice != null && line.choice.Count > 0)
        {
            foreach(var choice in line.choice)
            {
                var button = Instantiate(choiceButtonPrefab, choiceContainer);
                button.GetComponentInChildren<TMP_Text>().text = choice.text;
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    ShowLine(choice.nextID);
                });
            }
        }
        else
        {
            nextButton.onClick.RemoveAllListeners();
            if(line.nextID >= 0)
            {
                nextButton.onClick.AddListener(() => ShowLine(line.nextID));
            }
            else
            {
                nextButton.onClick.AddListener(() => EndDialogue());
            }
        }
    }

    void EndDialogue()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.GetComponent<RiraMove>().canMove = true;

        if(currentNPC != null)
        {
            var agent = currentNPC.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.isStopped = false;
        }
        dialoguePanel.SetActive(false);
        Debug.Log("会話終了");
    }

    // Update is called once per frame
    void Update()
    {
        if(dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialogue();
        }
    }
}
