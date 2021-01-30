using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pasajero
{
    protected List<Dialogue> possibleDialogue;
    protected List<Dialogue> turnLeftDialogue,turnRightDialogue;
    protected Dialogue introduction;
    protected string name;

    public List<Dialogue> PossibleDialogue{get{return possibleDialogue;}}
    public Dialogue Introduction{get{return introduction;}}

    public List<Dialogue> TurnLeftDialogue{get{return turnLeftDialogue;}}


    public Sprite happySprite{get;set;}
    public Sprite angrySprite{get;set;}
    public Sprite embarrassedSprite{get;set;}

    public Pasajero()
    {
        this.possibleDialogue=new List<Dialogue>();
    }

    public Dialogue getPossibleDialogue()
    {
        Dialogue dialogue;
        dialogue = this.possibleDialogue[Random.Range(0,this.possibleDialogue.Count)];
        Debug.Assert(dialogue!=null,"esta devolviendo un dialogo nulo");
        return dialogue;
    }

    public Sprite GetSprite(Dialogue.emotions emotion)
    {
        Sprite sprite =null;
        if(emotion == Dialogue.emotions.happy)
        {
            sprite=happySprite;
        }
        else if(emotion == Dialogue.emotions.angry)
        {
            sprite=angrySprite;
        }
        else if(emotion == Dialogue.emotions.embarrased)
        {
            sprite=embarrassedSprite;
        }
        Debug.Assert(sprite!=null,"No obtuvo el sprite");
        return sprite;
    }
}
