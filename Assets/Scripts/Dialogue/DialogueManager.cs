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
    static int MAX_LENGH = 40;
    static float TEXT_SPEED = .03f / 3f;
    static float WAIT_SPEED=0.07f;
    [SerializeField] private int ronda;
    [SerializeField] private TextMeshProUGUI textDialogue;
    [SerializeField] private TextMeshProUGUI[] textDialogueArray;
    public bool canShowNormalialogue,canShowUrgentDialogue,canShowQuestion;
    public bool needsUrgentDialogue,isFinished;
    public Pasajero pasajero;
    public NewDialogueEvent normalDialogueEvent,turnDialogueEvent,changeSprite;
    public UnityEvent turnLeftEvent,turnRightEvent,questionEvent,changePasajero;
    [SerializeField] public int actualLine;
    [SerializeField] private Pasajero[] pasajeros;

    [SerializeField] NormalDialogue normalDialogue;
    [SerializeField] TurnDialogueManager turnDialogueManager;
    [SerializeField] QuestionDialogueManager questionDialogueManager;
    
    public static DialogueManager I;

    public void Start()
    {
        canShowNormalialogue=true;
        canShowUrgentDialogue=true;
        canShowQuestion=false;
        isFinished=false;
        actualLine=0;
        textDialogue=textDialogueArray[0];
        turnLeftEvent=new UnityEvent();
        turnRightEvent=new UnityEvent();
        questionEvent = new UnityEvent();
        turnLeftEvent.AddListener(turnLeft);
        turnRightEvent.AddListener(turnRight);
        questionEvent.AddListener(askQuestion);
        enterPasajero();
    }

    private void Awake()
    {
        pasajero=pasajeros[0];
        I=this;
        normalDialogueEvent=new NewDialogueEvent();
        turnDialogueEvent= new NewDialogueEvent();
        changeSprite=new NewDialogueEvent();
        changePasajero=new UnityEvent();
        normalDialogueEvent.AddListener(NormalDialogue);
    }

    public void test()
    {
        Debug.Log("test");
    }

    public IEnumerator showDialogue(string newDialogue,bool isUrgent,bool clear)
    {
        string actualText =textDialogue.text;
        float actualSpeed =pasajero.getSpeed();
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
        {
            canShowUrgentDialogue=false;
            textDialogue.text+=" ";
        }
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
                yield return new WaitForSeconds(WAIT_SPEED);
            }
            else
            {
                actualText+= newDialogue[i];
                textDialogue.text=actualText;
                yield return new WaitForSeconds(actualSpeed);
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

    public void clearAllText()
    {
        for(int i=0;i<textDialogueArray.Length;i++)
        {
            textDialogueArray[i].text="";
            actualLine=0;
            textDialogue = textDialogueArray[0];
        }
    }

    public void failDialogue()
    {
        StartCoroutine(mostrarUrgente(pasajero.getFailDialogue().Text));
    }

    public IEnumerator mostrarUrgente(string texto)
    {
        yield return new WaitUntil(()=>DialogueManager.I.canShowUrgentDialogue);
        needsUrgentDialogue=true;
        StartCoroutine(showDialogue(texto,true,false));
        yield return new WaitUntil(()=>DialogueManager.I.canShowUrgentDialogue);
        needsUrgentDialogue=false;
    }

    public void NormalDialogue(Dialogue dialogue)
    {
        changeSprite.Invoke(dialogue);
        StartCoroutine(showDialogue(dialogue.Text,false,false));
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

    private void turnLeft()
    {
        if(canShowUrgentDialogue)
        {
            needsUrgentDialogue=true;
            TurnDialogue turnDialogue = pasajero.getTurnLeftDialogue();
            changeSprite.Invoke(turnDialogue);
            StartCoroutine(turnDialogueManager.showTurnDialogueRoutine(turnDialogue));
        }

    }

    private void turnRight()
    {
        if(canShowUrgentDialogue)
        {
            needsUrgentDialogue=true;
            TurnDialogue turnDialogue = pasajero.getTurnRightDialogue();
            changeSprite.Invoke(turnDialogue);
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

    public void nextPasajero()
    {
        StopAllCoroutines();
        if(ronda!=2)
        {
            ronda++;
        }
        else
            ronda=0;
        enterPasajero();
        changePasajero.Invoke();
    }

    public void enterPasajero()
    {
        clearAllText();
        this.pasajero=pasajeros[ronda];
        isFinished=false;
        canShowNormalialogue=true;
        StartCoroutine(normalDialogue.showNormalDialogue());
    }


}


