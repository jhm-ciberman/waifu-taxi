using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using WaifuTaxi;

public  class NewDialogueEvent:UnityEvent<Dialogue>{}

public class DialogueManager : MonoBehaviour
{
    static int MAX_LENGH = 33;
    static float TEXT_SPEED = .03f;
    [SerializeField] private TextMeshProUGUI textDialogue;
    [SerializeField] private TextMeshProUGUI[] textDialogueArray;
    public bool canShowNormalialogue,canShowUrgentDialogue,canShowQuestion;
    public bool needsUrgentDialogue;
    [SerializeField] public Pasajero pasajero;
    public NewDialogueEvent normalDialogueEvent,turnDialogueEvent;
    public UnityEvent turnLeftEvent,turnRightEvent,questionEvent;
    [SerializeField] public int actualLine;

    [SerializeField] NormalDialogue normalDialogueManager;
    [SerializeField] TurnDialogueManager turnDialogueManager;
    [SerializeField] QuestionDialogueManager questionDialogueManager;
    
    public static DialogueManager I;

    public void Start()
    {
        canShowNormalialogue=true;
        canShowUrgentDialogue=true;
        canShowQuestion=false;
        actualLine=0;
        textDialogue=textDialogueArray[0];
        turnLeftEvent=new UnityEvent();
        turnRightEvent=new UnityEvent();
        questionEvent = new UnityEvent();
        turnLeftEvent.AddListener(turnLeft);
        turnRightEvent.AddListener(turnRight);
        questionEvent.AddListener(askQuestion);
        //StartCoroutine(showNormalDialogue());
    }

    private void Awake()
    {
        I=this;
        normalDialogueEvent=new NewDialogueEvent();
        turnDialogueEvent= new NewDialogueEvent();
        normalDialogueEvent.AddListener(NormalDialogue);
    }

    public void test()
    {
        Debug.Log("test");
    }

    public IEnumerator showDialogue(string newDialogue,bool isUrgent,bool clear)
    {
        string actualText =textDialogue.text;
        int lastSpace=0;
        if(clear)
            {
                //actualText="";
                Debug.Log("clear");
        }
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
                int aux=i;
                while(aux<newDialogue.Length && newDialogue[aux]!=' ')
                {
                    aux++;
                }
                if(i-aux+textDialogue.text.Length>MAX_LENGH)
                {
                    actualText="";
                    if(actualLine!=3)
                    {
                        actualLine++;
                    }
                    else
                    {
                        actualLine=0;
                    }
                    textDialogue=textDialogueArray[actualLine];
                    if(actualLine==3)
                    {
                        textDialogueArray[0].text="";
                    }
                    else
                    {
                        textDialogueArray[actualLine+1].text="";
                    }
                    i++;
                }
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
                textDialogue.text+=" ";
                yield return new WaitUntil(()=>!needsUrgentDialogue);
                actualText=textDialogue.text;
                i=lastSpace;
            }
        }
        textDialogue.text+=" ";
        if(isUrgent)
        {
            canShowUrgentDialogue=true;
        }
        else
        {
            canShowNormalialogue=true;
        }
    }


    public void NormalDialogue(Dialogue dialogue)
    {
        StartCoroutine(showDialogue(dialogue.Text,false,false));
        //normalDialogueEvent.Invoke(dialogue);
    }

    public void GiveIndication(Indication indication)
    {
        switch(indication)
        {
            case Indication.TurnLeft:
                turnLeft();
                break;
            case Indication.TurnRight:
                turnRight();
                break;
        }
    }

    public void turnLeft()
    {
        if(canShowUrgentDialogue)
        {
            needsUrgentDialogue=true;
            TurnDialogue turnDialogue = pasajero.getTurnLeftDialogue();
            turnDialogueEvent.Invoke(turnDialogue);
            StartCoroutine(turnDialogueManager.showTurnDialogueRoutine(turnDialogue));
        }

    }

     public void turnRight()
    {
        if(canShowUrgentDialogue)
        {
            needsUrgentDialogue=true;
            TurnDialogue turnDialogue = pasajero.getTurnRightDialogue();
            turnDialogueEvent.Invoke(turnDialogue);
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


