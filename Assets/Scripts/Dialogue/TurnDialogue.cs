using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDialogue : Dialogue
{

    public enum directions
    {
        left,
        right
    }
    private directions direction;



    public directions Direction{get{return direction;}}

    public TurnDialogue(string text,emotions emotion,directions direction):base(text,emotion)
    {
        this.direction=direction;
    }


    public void setDirection(directions direction)
    {
        this.direction=direction;
    }

    

}