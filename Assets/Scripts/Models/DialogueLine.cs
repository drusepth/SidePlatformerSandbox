using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public enum MessageType
    {
        BasicMessage,
        //MultipleChoice,
        //OpenShop,
        //SceneChange
    };

    public MessageType messageType;
    
    public string speakerName;
    public Sprite speakerPortrait;
    public string lineText;

    public DialogueLine(string name, string text, Sprite portrait=null)
    {
        speakerName = name;
        lineText = text;
        speakerPortrait = portrait;
    }
}
