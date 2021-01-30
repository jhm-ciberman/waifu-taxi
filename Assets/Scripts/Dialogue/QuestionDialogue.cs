using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDialogue : Dialogue
{

    private string[] options;
    private int correct;
    private string afterDialogue;

    public string[] Options{get{return options;}}
    public int Correct{get{return correct;}}
    public string AfterDialogue{get{return afterDialogue;}}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public QuestionDialogue(string text,emotions emotion,string[] options,int correct,string afterDialogue):base(text,emotion)
    {
        this.options=options;
        this.correct=correct;
        this.afterDialogue=afterDialogue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
