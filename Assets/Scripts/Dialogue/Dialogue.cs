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
    private emotions emotion;

    public string Text{get;set;}
    public emotions Emotion{get{return emotion;}}

    public Dialogue(string text,emotions emotion)
    {
        this.Text=text;
        this.emotion=emotion;
    }

    public Dialogue(string text)
    {
        this.Text=text;
        this.emotion=emotions.normal;
    }



}
