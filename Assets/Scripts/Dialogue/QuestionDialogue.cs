using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDialogue : Dialogue
{

    private string[] options;
    private int correct;
    private string afterDialogue,correctDialogue,failDialogue;

    public string[] Options{get{return options;}}
    public int Correct{get{return correct;}}
    public string AfterDialogue{get{return afterDialogue;}}
    public string CorrectDialogue{get{return correctDialogue;}}
    public string FailDialogue{get{return failDialogue;}}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public QuestionDialogue(string text,emotions emotion,string[] options,int correct,string correctDialogue,string failDialogue):base(text,emotion)
    {
        this.options=options;
        this.correctDialogue=correctDialogue;
        this.failDialogue=failDialogue;
        this.correct=correct;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
