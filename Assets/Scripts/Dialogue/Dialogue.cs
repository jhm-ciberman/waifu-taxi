using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public enum emotions
    {
        happy,
        angry,
        embarrased
    }
    protected string text;
    private emotions emotion;

    public string Text{get{return text;}}
    public emotions Emotion{get{return emotion;}}

    public Dialogue(string text,emotions emotion)
    {
        this.text=text;
        this.emotion=emotion;
    }



}
