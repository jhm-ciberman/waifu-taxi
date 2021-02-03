using System.Collections.Generic;
using UnityEngine;
using WaifuTaxi;

public class Character
{
    private readonly List<Dialogue> _possibleDialogue      = new List<Dialogue>();
    private readonly List<Dialogue> _turnLeftDialogue      = new List<Dialogue>();
    private readonly List<Dialogue> _turnRightDialogue     = new List<Dialogue>();
    private readonly List<Dialogue> _introduction          = new List<Dialogue>();
    private readonly List<Dialogue> _indicationDialogue    = new List<Dialogue>();
    private readonly List<Dialogue> _failDirectionDialogue = new List<Dialogue>();
    private readonly List<Question> _questionDialogue      = new List<Question>();

    private readonly float _speedRandomFactor = 0.003f;
    private readonly float _fastTextSpeed     = 0.003f;
    private readonly float _slowTextSpeed     = 0.009f;

    public readonly Portrait portrait;

    private bool _useFastText = false;

    public Character(Portrait portrait)
    {
        this.portrait = portrait;
    }

    public float GetSpeed(bool forceFast = false)
    {
        if (forceFast) {
            this._useFastText = true;
        } else {
            this._useFastText = ! this._useFastText;
        }

        float speedDelta = Random.Range(0f, this._speedRandomFactor);
        return (this._useFastText ? this._fastTextSpeed : this._slowTextSpeed) + speedDelta;
    }

    public void AddIntroduction(string text)
    {
        this._introduction.Add(new Dialogue(text));
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

        this._possibleDialogue.Add(new Dialogue(text, emotion));
    }

    public void AddIndication(string text)
    {
        this._indicationDialogue.Add(new Dialogue(text, Emotion.Normal));
    }

    public void AddQuestion(string text, Emotion emotion, string[] options, int correct, string correctDialogue, string failDialogue)
    {
        this._questionDialogue.Add(new Question(text, emotion, options, correct, correctDialogue, failDialogue));
    }

    public void AddFailDialogue(string text, Emotion emotion)
    {
        this._failDirectionDialogue.Add(new Dialogue(text,emotion));
    }

    private T _Choose<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public Dialogue GetIndication() => this._Choose(this._indicationDialogue);

    public Dialogue GetFailDialogue() => this._Choose(this._failDirectionDialogue);

    public Dialogue GetIntroduction() => this._Choose(this._introduction);

    public Dialogue GetPossibleDialogue() => this._Choose(this._possibleDialogue);

    public Dialogue GetTurnLeftDialogue() => this._Choose(this._turnLeftDialogue);

    public Dialogue GetTurnRightDialogue() => this._Choose(this._turnRightDialogue);

    public Question GetQuestionDialogue() => this._Choose(this._questionDialogue);
}
