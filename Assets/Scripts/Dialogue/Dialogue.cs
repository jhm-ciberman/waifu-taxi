using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public enum emotions
    {
        angry,
        asking,
        blush,
        normal
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

    public Dialogue(string text)
    {
        this.text=text;
        this.emotion=emotions.normal;
    }



}
