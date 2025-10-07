using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData;

    private DialogueManager dialogueManager;
    private bool isPlayerInRange = false;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        anim.SetTrigger("IsWalk");
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.dialogueData = dialogueData;
            dialogueManager.StartDialogue(dialogueData,this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Stop");
            isPlayerInRange = true;
            Debug.Log("�v���C���[��NPC�ɐڐG��");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("IsWalk");
            isPlayerInRange = false;
            Debug.Log("�v���C���[��NPC���痣�ꂽ");
        }
    }
} 
