using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDialogue : Dialogue
{

    private string[] options;
    private int correct;
    private string afterDialogue,correctDialogue,failDialogue;

    public string[] Options => options;
    public int Correct => correct;
    public string AfterDialogue => afterDialogue;
    public string CorrectDialogue => correctDialogue;
    public string FailDialogue => failDialogue;

    public QuestionDialogue(string text, Emotion emotion, string[] options, int correct, string correctDialogue, string failDialogue) : base(text, emotion)
    {
        this.options = options;
        this.correctDialogue = correctDialogue;
        this.failDialogue = failDialogue;
        this.correct = correct;
    }
}
