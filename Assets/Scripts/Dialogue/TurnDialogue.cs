using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaifuTaxi;

public class TurnDialogue : Dialogue
{


    public Indication indication{get;private set;}
    public string failDialogue;

    public TurnDialogue(string text,emotions emotion,Indication indication):base(text,emotion)
    {
        this.indication=indication;
    }


    public void setDirection(Indication indication)
    {
        this.indication=indication;
    }

    

}