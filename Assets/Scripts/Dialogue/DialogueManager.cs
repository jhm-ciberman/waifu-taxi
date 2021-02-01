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
    static int MAX_LENGH = 55;
    static float WAIT_SPEED = 0.10f;

    [SerializeField] private int ronda;
    [SerializeField] private TextMeshProUGUI textDialogue;
    [SerializeField] private TextMeshProUGUI[] textDialogueArray;

    public bool canShowNormalialogue;
    public bool canShowUrgentDialogue;
    public bool canShowQuestion;

    public bool needsUrgentDialogue;
    public bool isFinished;
    public bool isAskingDirections;

    public Pasajero pasajero;
    public NewDialogueEvent normalDialogueEvent;
    public NewDialogueEvent turnDialogueEvent;
    public NewDialogueEvent changeSprite;

    public UnityEvent turnLeftEvent;
    public UnityEvent turnRightEvent;
    public UnityEvent questionEvent;
    public UnityEvent changePasajero;

    [SerializeField] public int actualLineIndex;
    [SerializeField] private Pasajero[] pasajeros;

    [SerializeField] QuestionDialogueManager questionDialogueManager;
    
    public static DialogueManager Instance;

    public void Start()
    {
        canShowNormalialogue = true;
        canShowUrgentDialogue = true;
        isFinished = false;
        isAskingDirections = false;
        canShowQuestion = true;
        actualLineIndex = 0;
        textDialogue = textDialogueArray[0];
        turnLeftEvent = new UnityEvent();
        turnRightEvent = new UnityEvent();
        questionEvent = new UnityEvent();
        enterPasajero();
    }

    private void Awake()
    {
        pasajero = pasajeros[0];
        Instance = this;
        normalDialogueEvent = new NewDialogueEvent();
        turnDialogueEvent = new NewDialogueEvent();
        changeSprite = new NewDialogueEvent();
        changePasajero = new UnityEvent();
        normalDialogueEvent.AddListener(NormalDialogue);
    }

    public IEnumerator showDialogue(string newDialogue, bool isUrgent)
    {
        string actualText = textDialogue.text;
        float actualSpeed = pasajero.getSpeed(isUrgent);

        int lastSpace = 0;
        int i = 0;

        if (!isUrgent) {
            canShowNormalialogue=false;
        } else {
            canShowUrgentDialogue=false;
            actualText += "- ";
        }

        while (i <newDialogue.Length) {
            if (newDialogue[i] == ' ') {
                lastSpace = i;
                int aux = i;
                while (aux<newDialogue.Length && newDialogue[aux] != ' ') {
                    aux++;
                }
                if (i - aux + textDialogue.text.Length > MAX_LENGH) {
                    actualText = "";
                    actualLineIndex = (actualLineIndex + 1) % textDialogueArray.Length;
                    var nextLineIndex = (actualLineIndex + 1) % textDialogueArray.Length;
                    textDialogue = textDialogueArray[actualLineIndex];
                    textDialogue.alpha = 1f;
                    StartCoroutine(FadeOutText(textDialogueArray[nextLineIndex], textDialogue));
                }
            }
            var currentCharacter = newDialogue[i];
            if (currentCharacter == '*') {
                actualSpeed = pasajero.getSpeed(isUrgent);
                var waitSpeed = WAIT_SPEED;
                if (i < newDialogue.Length - 1 && newDialogue[i + 1] == '*') {
                    i++;
                    waitSpeed *= 2;
                } 
                yield return new WaitForSeconds(waitSpeed);
            } else {
                if (textDialogue.text != "" || currentCharacter != ' ') { // Prevent spaces at the start of new line
                    actualText += currentCharacter;
                }
                textDialogue.text=actualText;
                yield return new WaitForSeconds(actualSpeed);
            }
            i++;
            
            if (!isUrgent && needsUrgentDialogue) {
                if (textDialogue.text != "") textDialogue.text+=" ";
                yield return new WaitUntil(()=>!needsUrgentDialogue);
                actualText = textDialogue.text;
                i = lastSpace;
            }
        }

        if (textDialogue.text != "") textDialogue.text+=" ";

        if(isUrgent) {
            canShowUrgentDialogue=true;
        } else {
            canShowNormalialogue=true;
        }
    }

    private float textFadeSpeed = 2f;

    private IEnumerator FadeOutText(TextMeshProUGUI lineToFade, TextMeshProUGUI lineInProgress)
    {
        float percent = 1f;

        while (percent > 0.01f) {
            var maxPercent = 1f - ((float) lineInProgress.text.Length / (float) MAX_LENGH);
            percent -= this.textFadeSpeed * Time.deltaTime;
            percent = Mathf.Min(percent, maxPercent);
            lineToFade.alpha = percent;
            yield return null;
        }

        lineToFade.text = "";
    }

    public void clearAllText()
    {
        for(int i = 0; i < textDialogueArray.Length; i++) {
            textDialogueArray[i].text = "";
            actualLineIndex = 0;
            textDialogue = textDialogueArray[0];
        }
    }

    public static string indicationToString(Indication indication)
    {
        string s = "...";
        switch (indication) {
            case Indication.TurnLeft: s = "left";  break;
            case Indication.TurnRight: s = "right"; break;
            case Indication.TurnU: s = "in u"; break;
        }
        return s;
    }

    public void failDialogue(Indication indication, Indication prevIndication)
    {
        var currString = indicationToString(indication);
        var prevString = indicationToString(prevIndication);
        var currStringUpper = char.ToUpper(currString[0]) + currString.Substring(1);
        var prevStringUpper = char.ToUpper(prevString[0]) + prevString.Substring(1);
        var text = pasajero.getFailDialogue().Text;
        text =  text.Replace("[dir]", currString);
        text = text.Replace("[prev_dir]", currString);
        text = text.Replace("[Dir]", currStringUpper);
        text = text.Replace("[Prev_dir]", prevStringUpper);
        ScoreManager.Instance.removeStar(1);

        AudioManager.Instance.PlaySound("wrong_answer");

        StartCoroutine(mostrarUrgente(text));
    }

    public IEnumerator mostrarUrgente(string texto)
    {
        yield return new WaitUntil(()=>DialogueManager.Instance.canShowUrgentDialogue);
        needsUrgentDialogue=true;
        StartCoroutine(showDialogue(texto,true));
        yield return new WaitUntil(()=>DialogueManager.Instance.canShowUrgentDialogue);
        needsUrgentDialogue=false;
    }

    public void NormalDialogue(Dialogue dialogue)
    {
        changeSprite.Invoke(dialogue);
        StartCoroutine(showDialogue(dialogue.Text,false));
    }

    public IEnumerator askQuestions()
    {
        while(true) {
            yield return new WaitUntil(() => canShowUrgentDialogue);
            yield return new WaitUntil(() => !isAskingDirections);
            yield return new WaitForSeconds(Random.Range(1, 4));
            needsUrgentDialogue = true;
            canShowQuestion = true;
            QuestionDialogue questionDialogue = pasajero.getRandomQuestionDialogue();
            StartCoroutine(questionDialogueManager.showQuestionRoutine(questionDialogue));
        }
    }

    public void GiveIndication(Indication indication)
    {
        this.isAskingDirections = true;
        if(canShowUrgentDialogue && indication != Indication.Continue) {
            needsUrgentDialogue = true;
            TurnDialogue turnDialogue = pasajero.getIndication();
            changeSprite.Invoke(turnDialogue);
            StartCoroutine(showTurnDialogueRoutine(turnDialogue, indication));
        }
    }

    public IEnumerator showTurnDialogueRoutine(TurnDialogue newDialogue, Indication indication)
    {
        bool canShowUrgentDialogue = DialogueManager.Instance.canShowUrgentDialogue;
        int numeroDeDialogosFalsos = Random.Range(0,1);
        string fullDialogue;
        for(int j = 0; j <= numeroDeDialogosFalsos; j++) {
            fullDialogue="";
            
            if (j==0) { //Al principio
                fullDialogue+=" ";
            }

            if(j == numeroDeDialogosFalsos) { //Direccion de verdad 
                fullDialogue+=newDialogue.Text;
                fullDialogue+="..As I was saying before;";
            }
            string direction = DialogueManager.indicationToString(indication);
            var directionUpper= char.ToUpper(direction[0]) + direction.Substring(1);
            fullDialogue = fullDialogue.Replace("[dir]", direction);
            fullDialogue = fullDialogue.Replace("[Dir]", directionUpper);
            yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);
            IEnumerator newRoutine = DialogueManager.Instance.showDialogue(fullDialogue, true);
            StartCoroutine(newRoutine);
        }
        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);
        DialogueManager.Instance.needsUrgentDialogue= false;
        DialogueManager.Instance.isAskingDirections = false;
    }

    public void nextPasajero()
    {
        AudioManager.Instance.PlaySound("correct_answer");
        StopAllCoroutines();
        if(ronda!=2) {
            ronda++;
        } else {
            ronda=0;
        }
        enterPasajero();
        changePasajero.Invoke();
    }

    public void enterPasajero()
    {
        clearAllText();
        this.pasajero=pasajeros[ronda];
        isFinished=false;
        canShowNormalialogue=true;
        StartCoroutine(mostrarUrgente(pasajero.getIntroduction().Text));
        StartCoroutine(showNormalDialogue());
        //StartCoroutine(askQuestions());
    }

    public IEnumerator showNormalDialogue()
    {
        yield return new WaitForSeconds(0.5f); //Esto esta muy feo
        while(true)
        {
            yield return new WaitUntil(() => DialogueManager.Instance.canShowNormalialogue);
            Dialogue dialogue= DialogueManager.Instance.pasajero.getPossibleDialogue();
            DialogueManager.Instance.normalDialogueEvent.Invoke(dialogue);
        }
    }
}


