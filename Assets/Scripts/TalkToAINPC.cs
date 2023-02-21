using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TalkToAINPC : MonoBehaviour
{
    private bool playerInInteractionRange = false;
    private GameObject visibleInteractionTrigger;

    [Header("Setup")]
    public string dialogueModelUrl;
    public Sprite speakerAvatar;
    public string speakerName;
    
    [Header("State")]
    public List<DialogueLine> cachedDialogueLines;
    public bool fetchingNewDialogue = false;

    void Start()
    {
        visibleInteractionTrigger = transform.Find("Interactable Display").gameObject;
        visibleInteractionTrigger.SetActive(false);

        cachedDialogueLines = new List<DialogueLine>();
    }

    void Update()
    {
        if (playerInInteractionRange && Input.GetKeyDown(KeyCode.E))
        {
            // Since we use the same key to start conversations AND continue
            // through each line, we only want to queue up a new script if we're
            // not already in an active conversation.
            if (!DialogueManager.Instance.CurrentlyInConversation())
                StartDialogue();
        }
    }

    private void FixedUpdate()
    {
        if (cachedDialogueLines.Count == 0 && !fetchingNewDialogue)
        {
            Debug.Log("Fetching new dialogue lines");
            fetchingNewDialogue = true;

            // Make a GET request to dialogueModelUrl on Huggingface to get a line of dialogue
            // from the model. We'll use Unity's built-in WWW class to do this.
            // TODO
            string line = JsonUtility.FromJson<string>("\"Hello\"");
            
            //foreach (string line in lines)
            cachedDialogueLines.Add(new DialogueLine(speakerName, line, speakerAvatar));
        }
    }

    void StartDialogue()
    {
        if (cachedDialogueLines.Count > 0)
        {
            // Queue up our cached dialogue lines, then clear them to generate more
            DialogueManager.Instance.QueueDialogueScript(cachedDialogueLines.ToArray());
            cachedDialogueLines.Clear();
        }
    }

    // TODO actually write/use this or something like it :)
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
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
