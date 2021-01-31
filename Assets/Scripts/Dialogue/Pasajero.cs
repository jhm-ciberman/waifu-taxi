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
    protected List<Dialogue> introduction;

    public float SpeedRandomFactor{get;private set;}
    public float FastTextSpeed{get;private set;}
    public float SlowTextSpeed{get;private set;}

    public List<Dialogue> PossibleDialogue{get{return possibleDialogue;}}
    public List<TurnDialogue> IndicationDialogue{get; private set;}
    public List<TurnDialogue> TurnLeftDialogue{get{return turnLeftDialogue;}}
    public List<TurnDialogue> TurnRightDialogue{get {return turnRightDialogue;}}
    public List<Dialogue> failDirectionDialogue{get;private set;}
    public List<Dialogue> Introduction{get{return introduction;}}

    public Sprite spriteAngry;
    public Sprite spriteAsking;
    public Sprite spriteBlush;
    public Sprite spriteNormal;

    public abstract void addDialogue();

    private bool useFastText = false;

    public void Start()
    {
        this.possibleDialogue=new List<Dialogue>();
        this.turnLeftDialogue = new List<TurnDialogue>();
        this.turnRightDialogue = new List<TurnDialogue>();
        this.questionDialogue = new List<QuestionDialogue>();
        this.failDirectionDialogue = new List<Dialogue>();
        this.introduction=new List<Dialogue>();
        IndicationDialogue=new List<TurnDialogue>();
        SpeedRandomFactor = 0.004f;
        FastTextSpeed = 0.004f;
        SlowTextSpeed = 0.008f;
        addDialogue();
    }

    public float getSpeed(bool forceFast = false)
    {
        if (forceFast) {
            this.useFastText = true;
        } else {
            this.useFastText = ! this.useFastText;
        }

        float speedDelta = Random.Range(0f, SpeedRandomFactor);
        return (this.useFastText ? FastTextSpeed : SlowTextSpeed) + speedDelta;
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

    public void addIndicationDialogue(string text)
    {
        TurnDialogue dialogue = new TurnDialogue(text,Dialogue.emotions.normal);
        IndicationDialogue.Add(dialogue);
    }

    public void addIntroduction(string text)
    {
        Dialogue dialogue = new Dialogue(text);
        this.introduction.Add(dialogue);
    }

    public TurnDialogue getIndication()
    {
        TurnDialogue dialogue = null;
        int k= Random.Range(0,IndicationDialogue.Count);
        dialogue=IndicationDialogue[k];
        return dialogue;
    }


    public void addQuestionDialogue(string text,Dialogue.emotions emotion,string[] options,int correct,string afterDialogue,string correctDialogue,string failDialogue)
    {
        QuestionDialogue dialogue=new QuestionDialogue(text,emotion,options,correct,correctDialogue,failDialogue);
        this.questionDialogue.Add(dialogue);
    }

    public void addFailDialogue(string text,Dialogue.emotions emotion)
    {
        Dialogue dialogue = new Dialogue(text,emotion);
        this.failDirectionDialogue.Add(dialogue);
    }

    public Dialogue getFailDialogue()
    {
        Dialogue dialogue = null;
        int k= Random.Range(0,failDirectionDialogue.Count);
        dialogue=failDirectionDialogue[k];
        return dialogue;

    }

    public Dialogue getIntroduction()
    {
        Dialogue dialogue = null;
        int k= Random.Range(0,introduction.Count);
        dialogue=introduction[k];
        return dialogue;
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
