using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WaifuTaxi;

public abstract class Pasajero:MonoBehaviour
{
    protected List<Dialogue> possibleDialogue;
    protected List<TurnDialogue> turnLeftDialogue,turnRightDialogue;
    protected List<QuestionDialogue> questionDialogue;
    protected Dialogue introduction;

    public List<Dialogue> PossibleDialogue{get{return possibleDialogue;}}
    public List<TurnDialogue> TurnLeftDialogue{get{return turnLeftDialogue;}}
    public List<TurnDialogue> TurnRightDialogue{get {return turnRightDialogue;}}
    public Dialogue Introduction{get{return introduction;}}

    public Sprite spriteAngry;
    public Sprite spriteAsking;
    public Sprite spriteBlush;
    public Sprite spriteNormal;

    public abstract void addDialogue();

    public void Start()
    {
        this.possibleDialogue=new List<Dialogue>();
        this.turnLeftDialogue = new List<TurnDialogue>();
        this.turnRightDialogue = new List<TurnDialogue>();
        this.questionDialogue = new List<QuestionDialogue>();
        addDialogue();
    }

    public void addPossibleDialogue(string text,Dialogue.emotions emotion)
    {
        Dialogue dialogue = new Dialogue(text,emotion);
        this.possibleDialogue.Add(dialogue);
    }

    public void addPossibleDialogue(string text)
    {
        Dialogue dialogue = new Dialogue(text);
        this.possibleDialogue.Add(dialogue);
    }

    public void addTurnLeftDialogue(string text,Dialogue.emotions emotion)
    {
        TurnDialogue turnDialogue = new TurnDialogue(text,emotion,Indication.TurnLeft);
        this.turnLeftDialogue.Add(turnDialogue);
    }

     public void addTurnRightDialogue(string text,Dialogue.emotions emotion)
    {
        TurnDialogue turnDialogue = new TurnDialogue(text,emotion,Indication.TurnRight);
        this.turnRightDialogue.Add(turnDialogue);
    }

    public void addQuestionDialogue(string text,Dialogue.emotions emotion,string[] options,int correct,string afterDialogue,string correctDialogue,string failDialogue)
    {
        QuestionDialogue dialogue=new QuestionDialogue(text,emotion,options,correct,afterDialogue,correctDialogue,failDialogue);
        this.questionDialogue.Add(dialogue);
    }

    public QuestionDialogue getRandomQuestionDialogue()
    {
        QuestionDialogue dialogue=null;
        int k= Random.Range(0,questionDialogue.Count);
        dialogue = questionDialogue[k];
        return dialogue;
    }

    public TurnDialogue getRandomTurnDialogue()
    {
        TurnDialogue turnDialogue=null;
        int k = Random.Range(0,2);
        switch(k)
        {
            case 0:
                turnDialogue=getTurnLeftDialogue();
                break;
            case 1:
                turnDialogue=getTurnRightDialogue();
                break;
        }
        Debug.Assert(turnDialogue!=null,"Estas devolviendo un dialogo random nulo");
        return turnDialogue;
    }

    public Dialogue getPossibleDialogue()
    {
        Dialogue dialogue;
        dialogue = this.possibleDialogue[Random.Range(0,this.possibleDialogue.Count)];
        Debug.Assert(dialogue!=null,"esta devolviendo un dialogo nulo");
        return dialogue;
    }

    public TurnDialogue getTurnLeftDialogue()
    {
        TurnDialogue dialogue;
        dialogue = this.turnLeftDialogue[Random.Range(0,this.turnLeftDialogue.Count)];
        Debug.Assert(dialogue!=null,"esta devolviendo un dialogo nulo");
        return dialogue;
    }

    public TurnDialogue getTurnRightDialogue()
    {
        TurnDialogue dialogue;
        dialogue = this.turnRightDialogue[Random.Range(0,this.turnRightDialogue.Count)];
        Debug.Assert(dialogue!=null,"esta devolviendo un dialogo nulo");
        return dialogue;
    }

        
}
