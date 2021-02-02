using System.Collections.Generic;
using UnityEngine;
using WaifuTaxi;

public class Character
{
    protected List<Dialogue> possibleDialogue;
    protected List<TurnDialogue> turnLeftDialogue;
    protected List<TurnDialogue> turnRightDialogue;
    protected List<QuestionDialogue> questionDialogue;
    protected List<Dialogue> introduction;

    public float SpeedRandomFactor {get; private set;}
    public float FastTextSpeed {get; private set;}
    public float SlowTextSpeed {get; private set;}

    public List<TurnDialogue> IndicationDialogue {get; private set;}
    public List<Dialogue> failDirectionDialogue {get;private set;}

    public Portrait portrait;

    public Character(Portrait portrait)
    {
        this.portrait = portrait;
        this.possibleDialogue = new List<Dialogue>();
        this.turnLeftDialogue = new List<TurnDialogue>();
        this.turnRightDialogue = new List<TurnDialogue>();
        this.questionDialogue = new List<QuestionDialogue>();
        this.failDirectionDialogue = new List<Dialogue>();
        this.introduction = new List<Dialogue>();
        IndicationDialogue = new List<TurnDialogue>();
        SpeedRandomFactor = 0.003f;
        FastTextSpeed = 0.003f;
        SlowTextSpeed = 0.009f;
    }

    public void AddIntroduction(string text)
    {
        this.introduction.Add(new Dialogue(text));
    }

    private bool useFastText = false;

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

    public void addPossibleDialogue(string text, Emotion emotion)
    {
        this.possibleDialogue.Add(new Dialogue(text, emotion));
    }

    public void AddDialogue(string text)
    {
        Emotion emotion = Emotion.Normal;

        switch (UnityEngine.Random.Range(0, 4)) {
            case 0: emotion = Emotion.Angry; break;
            case 1: emotion = Emotion.Asking; break;
            case 2: emotion = Emotion.Blush; break;
            case 3: emotion = Emotion.Normal; break;
        }

        this.possibleDialogue.Add(new Dialogue(text, emotion));
    }

    public void AddIndication(string text)
    {
        IndicationDialogue.Add(new TurnDialogue(text, Emotion.Normal));
    }

    public TurnDialogue getIndication()
    {
        TurnDialogue dialogue = null;
        int k= Random.Range(0,IndicationDialogue.Count);
        dialogue=IndicationDialogue[k];
        return dialogue;
    }

    public void AddQuestion(string text, Emotion emotion, string[] options, int correct, string correctDialogue, string failDialogue)
    {
        QuestionDialogue dialogue = new QuestionDialogue(text, emotion, options, correct, correctDialogue, failDialogue);
        this.questionDialogue.Add(dialogue);
    }

    public void AddFailDialogue(string text, Emotion emotion)
    {
        Dialogue dialogue = new Dialogue(text,emotion);
        this.failDirectionDialogue.Add(dialogue);
    }

    public Dialogue GetFailDialogue()
    {
        Dialogue dialogue = null;
        int k = Random.Range(0, failDirectionDialogue.Count);
        dialogue = failDirectionDialogue[k];
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
        TurnDialogue turnDialogue = null;
        int k = Random.Range(0,2);
        switch (k) {
            case 0: turnDialogue = this.GetTurnLeftDialogue();  break;
            case 1: turnDialogue = this.GetTurnRightDialogue(); break;
        }
        Debug.Assert(turnDialogue != null, "Estas devolviendo un dialogo random nulo");
        return turnDialogue;
    }

    public Dialogue GetPossibleDialogue()
    {
        return this.possibleDialogue[Random.Range(0, this.possibleDialogue.Count)];
    }

    public TurnDialogue GetTurnLeftDialogue()
    {
        return this.turnLeftDialogue[Random.Range(0, this.turnLeftDialogue.Count)];
    }

    public TurnDialogue GetTurnRightDialogue()
    {
        return this.turnRightDialogue[Random.Range(0, this.turnRightDialogue.Count)];
    }
}
