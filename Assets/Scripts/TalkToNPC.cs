using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNPC : MonoBehaviour
{
    private bool playerInInteractionRange = false;
    private GameObject visibleInteractionTrigger;

    public DialogueLine[] dialogueLines;

    void Start()
    {
        visibleInteractionTrigger = transform.Find("Interactable Display").gameObject;
        visibleInteractionTrigger.SetActive(false);
    }

    void Update()
    {
        /*
        if (playerInInteractionRange && Input.GetKeyDown(KeyCode.E))
        {
            // Since we use the same key to start conversations AND continue
            // through each line, we only want to queue up a new script if we're
            // not already in an active conversation.
            if (!DialogueManager.Instance.CurrentlyInConversation())
                StartDialogue();
        }
        */
    }

    void StartDialogue()
    {
        DialogueManager.Instance.QueueDialogueScript(dialogueLines);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInInteractionRange = true;
            visibleInteractionTrigger.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInInteractionRange = false;
            visibleInteractionTrigger.SetActive(false);
        }
    }
}
