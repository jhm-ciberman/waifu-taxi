using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaifuTaxi;

public class TurnDialogue : Dialogue
{
    public Indication indication{get;private set;}
    public string failDialogue;

    public TurnDialogue(string text,emotions emotion):base(text,emotion)
    {
        
    }


    public void setDirection(Indication indication)
    {
        this.indication=indication;
    }

    public void getText()
    {
        
    }
    

    

}