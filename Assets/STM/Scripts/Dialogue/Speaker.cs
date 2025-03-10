using System;
using UnityEngine;

[Serializable]

public class Speaker
{
    public string charID;
    public string charName;
    public Sprite charPortrait;

    public Speaker (string charID, string charName, Sprite charPortrait)
    {
        this.charID = charID;
        this.charName = charName;
        this.charPortrait = charPortrait;
    }
}
