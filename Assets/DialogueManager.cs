using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Setup")]
    public GameObject dialoguePanel;
    public Image speakerAvatar;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakerText;

    [Header("State")]
    public bool currentlyShowingDialogue = false;
    public List<DialogueLine> queuedDialogueLines = new List<DialogueLine>();
    public float afterConversationPadding = 0.5f;
    private float afterConversationTimer = 0f;

    [Header("User config")]
    public KeyCode dismissMessageButton = KeyCode.E;

    private void FixedUpdate()
    {
        // If we're not currently showing dialogue AND there's queued lines to show, do it
        if (!currentlyShowingDialogue && queuedDialogueLines.Count > 0)
        {            
            ShowDialogueLine(queuedDialogueLines[0]);
            queuedDialogueLines.RemoveAt(0);
        }
    }

    void Update()
    {
        // If we ARE showing dialogue and the user clears it, actually clear it.
        // This intentionally happens in Update() instead of FixedUpdate() to make
        // sure we don't miss the keypress event.
        if (currentlyShowingDialogue && Input.GetKeyDown(dismissMessageButton))
            ClearDisplayedDialogueLine();

        // After conversations, start ticking down our padding timer to give some
        // padding before another conversation can begin.
        if (!currentlyShowingDialogue && queuedDialogueLines.Count == 0)
            afterConversationTimer -= Time.deltaTime;
    }

    public bool CurrentlyInConversation()
    {
        return currentlyShowingDialogue || queuedDialogueLines.Count > 0 || afterConversationTimer > 0f;
    }

    public void LoadCanvasReferencesForNewScene()
    {
        // TODO we probably need this instead of manual references if canvas aren't
        // persisted between scenes, dunno
    }

    public void QueueDialogueLine(DialogueLine line)
    {
        queuedDialogueLines.Add(line);
    }

    public void ShowDialogueLine(DialogueLine line)
    {
        if (line.speakerPortrait != null)
            speakerAvatar.sprite = line.speakerPortrait;

        speakerName.text = line.speakerName;
        speakerText.text = line.lineText;

        currentlyShowingDialogue = true;
        dialoguePanel.SetActive(true);

        // Reset our after-conversation padding
        afterConversationTimer = afterConversationPadding;
    }

    public void ClearDisplayedDialogueLine()
    {
        dialoguePanel.SetActive(false);
        currentlyShowingDialogue = false;
    }

    public void QueueDialogueScript(DialogueLine[] script)
    {
        foreach (DialogueLine line in script)
            QueueDialogueLine(line);
    }
}
