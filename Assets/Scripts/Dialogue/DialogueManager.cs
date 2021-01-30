using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public  class NewDialogueEvent:UnityEvent<string>{}

public class DialogueManager : MonoBehaviour
{
    static int MAX_LENGH = 183;
    static float TEXT_SPEED = .03f;
    [SerializeField] private TextMeshProUGUI textDialogue;
    public bool canShowNormalialogue,canShowUrgentDialogue,canShowQuestion;
    public bool needsUrgentDialogue;
    public Pasajero pasajero;
    public NewDialogueEvent normalDialogueEvent,turnDialogueEvent;
    public UnityEvent turnLeftEvent,turnRightEvent,questionEvent;

    [SerializeField] NormalDialogue normalDialogueManager;
    [SerializeField] TurnDialogueManager turnDialogueManager;
    [SerializeField] QuestionDialogueManager questionDialogueManager;
    
    public static DialogueManager I;

    public void Start()
    {
        canShowNormalialogue=true;
        canShowUrgentDialogue=true;
        canShowQuestion=false;
        textDialogue.text="";
        turnLeftEvent=new UnityEvent();
        turnRightEvent=new UnityEvent();
        questionEvent = new UnityEvent();
        normalDialogueEvent=new NewDialogueEvent();
        normalDialogueEvent.AddListener(NormalDialogue);
        turnLeftEvent.AddListener(turnLeft);
        turnRightEvent.AddListener(turnRight);
        questionEvent.AddListener(askQuestion);
        //StartCoroutine(showNormalDialogue());
    }

    private void Awake()
    {
        I=this;
        this.pasajero = new Pasajero1();
    }

    public void test()
    {
        Debug.Log("test");
    }

    public IEnumerator showDialogue(string newDialogue,bool isUrgent)
    {
        string actualText =textDialogue.text;
        int lastSpace=0;
        //newDialogueEvent.Invoke(pasajero,newDialogue);
        int i=0;
        if(!isUrgent)
            canShowNormalialogue=false;
        else
            canShowUrgentDialogue=false;
        while(i < newDialogue.Length)
        {
            if(newDialogue[i]==' ')
                {
                    lastSpace=i;
                }
                if(actualText.Length>MAX_LENGH)
                {
                    actualText="";
                    i=lastSpace;
                }
                if(newDialogue[i]=='*')
                {
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    actualText+= newDialogue[i];
                    textDialogue.text=actualText;
                    yield return new WaitForSeconds(TEXT_SPEED);
                }
                i++;
                if(!isUrgent && needsUrgentDialogue)
                {
                    yield return new WaitUntil(()=>!needsUrgentDialogue);
                    actualText=textDialogue.text;
                    i=lastSpace;
                }
        }
        actualText+=" ";
        if(isUrgent)
        {
            canShowUrgentDialogue=true;
        }
        else
        {
            canShowNormalialogue=true;
        }
    }


    public void NormalDialogue(string dialogue)
    {
        StartCoroutine(showDialogue(dialogue,false));
    }

    public void turnLeft()
    {
        if(canShowUrgentDialogue)
        {
            needsUrgentDialogue=true;
            TurnDialogue turnDialogue = pasajero.getTurnLeftDialogue();
            StartCoroutine(turnDialogueManager.showTurnDialogueRoutine(turnDialogue));
        }

    }

     public void turnRight()
    {
        if(canShowUrgentDialogue)
        {
            needsUrgentDialogue=true;
            TurnDialogue turnDialogue = pasajero.getTurnLeftDialogue();
            StartCoroutine(turnDialogueManager.showTurnDialogueRoutine(turnDialogue));
        }
        
    }

    public void askQuestion()
    {
        if(canShowUrgentDialogue)
        {
            needsUrgentDialogue=true;
            canShowQuestion=true;
            QuestionDialogue questionDialogue = pasajero.getRandomQuestionDialogue();
            StartCoroutine(questionDialogueManager.showQuestionRoutine(questionDialogue));
        }
    }


}


