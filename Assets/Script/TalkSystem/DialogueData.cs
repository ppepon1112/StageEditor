using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string SpeakerName;
    [TextArea(2,5)]
    public string text;
    public Sprite portrait;
    public int nextID;

    public List<Choice> choice;
}
[System.Serializable]
public class Choice
{
    public string text;
    public int nextID;
}

[CreateAssetMenu(menuName = "Game/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines;
}
