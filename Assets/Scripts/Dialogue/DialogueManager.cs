using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using WaifuTaxi;

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

    public System.Action<Dialogue> normalDialogueEvent;
    public System.Action turnDialogueEvent;
    public System.Action<Dialogue> changeSprite;
    public System.Action changePasajero;

    [SerializeField] public int actualLineIndex;
    
    private Pasajero[] pasajeros;
    
    private int correct = 0;
    private int answer = 0;

    public static DialogueManager Instance;

    public Portrait portraitCrissy;
    public Portrait portraitMelody;
    public Portrait portraitArachne;

    public void Start()
    {
        canShowNormalialogue = true;
        canShowUrgentDialogue = true;
        isFinished = false;
        isAskingDirections = false;
        canShowQuestion = true;
        actualLineIndex = 0;
        textDialogue = textDialogueArray[0];
        enterPasajero();
    }

    private void Awake()
    {
        this.pasajeros = new Pasajero[] {
            Crissy.Make(this.portraitCrissy),
            Melody.Make(this.portraitMelody),
            Arachne.Make(this.portraitArachne),
        };

        pasajero = pasajeros[0];
        Instance = this;
        normalDialogueEvent += NormalDialogue;
    }

    public void askQuestion(int correct)
    {
        this.correct = correct;
        answer = 0;
    }

    public bool answeredCorrectly()
    {
        return answer == correct;
    }

    public void SetAnswer(int answer)
    {
        this.answer = answer;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            this.answer = 1;
        } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            this.answer = 2;
        } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            this.answer = 3;
        }
    }

    private IEnumerator ShowDialogue(string newDialogue, bool isUrgent)
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

    public void ClearAllText()
    {
        for(int i = 0; i < textDialogueArray.Length; i++) {
            textDialogueArray[i].text = "";
            actualLineIndex = 0;
            textDialogue = textDialogueArray[0];
        }
    }

    private static string IndicationToString(Indication indication)
    {
        string s = "...";
        switch (indication) {
            case Indication.TurnLeft: s = "left";  break;
            case Indication.TurnRight: s = "right"; break;
            case Indication.TurnU: s = "in u"; break;
        }
        return s;
    }

    public void FailDialogue(Indication indication, Indication prevIndication)
    {
        var currString = IndicationToString(indication);
        var prevString = IndicationToString(prevIndication);
        var currStringUpper = char.ToUpper(currString[0]) + currString.Substring(1);
        var prevStringUpper = char.ToUpper(prevString[0]) + prevString.Substring(1);
        var text = pasajero.getFailDialogue().text;
        text = text.Replace("[dir]", currString);
        text = text.Replace("[prev_dir]", currString);
        text = text.Replace("[Dir]", currStringUpper);
        text = text.Replace("[Prev_dir]", prevStringUpper);
        ScoreManager.Instance.removeStar(1);

        AudioManager.Instance.PlaySound("wrong_answer");

        StartCoroutine(mostrarUrgente(text));
    }

    public IEnumerator mostrarUrgente(string texto)
    {
        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);
        needsUrgentDialogue = true;
        StartCoroutine(ShowDialogue(texto, true));
        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);
        needsUrgentDialogue = false;
    }

    public void NormalDialogue(Dialogue dialogue)
    {
        changeSprite.Invoke(dialogue);
        StartCoroutine(ShowDialogue(dialogue.text, false));
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
            StartCoroutine(showQuestionRoutine(questionDialogue));
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

    public IEnumerator showQuestionRoutine(QuestionDialogue dialogue)
    {
        int k = dialogue.Options.Length;
        string fullDialogue = null;
        this.askQuestion(dialogue.Correct);

        for(int i = 0; i <= k; i++) {
            fullDialogue = " ";
            if (i == 0) { // Dialogo inicial
                fullDialogue += dialogue.text;
            } else if (i == k) { // Dialogo final
                yield return new WaitForSeconds(2);
            } else {
                fullDialogue = dialogue.Options[i - 1];
            }

            yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);

            StartCoroutine(DialogueManager.Instance.ShowDialogue(fullDialogue, true));
        }

        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);

        if (this.answeredCorrectly()) {
            fullDialogue = dialogue.CorrectDialogue;
        } else {
            fullDialogue = dialogue.FailDialogue;
        }

        StartCoroutine(DialogueManager.Instance.ShowDialogue(fullDialogue, true));

        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);

        DialogueManager.Instance.canShowQuestion = false;
        DialogueManager.Instance.needsUrgentDialogue = false;
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
                fullDialogue+=newDialogue.text;
                fullDialogue+="..As I was saying before;";
            }
            string direction = DialogueManager.IndicationToString(indication);
            var directionUpper= char.ToUpper(direction[0]) + direction.Substring(1);
            fullDialogue = fullDialogue.Replace("[dir]", direction);
            fullDialogue = fullDialogue.Replace("[Dir]", directionUpper);
            yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);
            IEnumerator newRoutine = DialogueManager.Instance.ShowDialogue(fullDialogue, true);
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
        if (ronda!=2) {
            ronda++;
        } else {
            ronda=0;
        }
        enterPasajero();
        changePasajero.Invoke();
    }

    public void enterPasajero()
    {
        ClearAllText();
        this.pasajero=pasajeros[ronda];
        isFinished=false;
        canShowNormalialogue=true;
        StartCoroutine(mostrarUrgente(pasajero.getIntroduction().text));
        StartCoroutine(showNormalDialogue());
    }

    public IEnumerator showNormalDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        while(true) {
            yield return new WaitUntil(() => DialogueManager.Instance.canShowNormalialogue);
            Dialogue dialogue= DialogueManager.Instance.pasajero.getPossibleDialogue();
            this.normalDialogueEvent?.Invoke(dialogue);
        }
    }
}


