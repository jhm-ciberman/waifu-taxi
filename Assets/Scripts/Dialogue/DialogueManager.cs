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

    [SerializeField] private int characterIndex;
    [SerializeField] private TextMeshProUGUI textDialogue;
    [SerializeField] private TextMeshProUGUI[] textDialogueArray;

    public bool canShowNormalialogue;
    public bool canShowUrgentDialogue;
    public bool canShowQuestion;

    public bool needsUrgentDialogue;
    public bool isFinished;
    public bool isAskingDirections;

    public Character character;

    public System.Action<Dialogue> normalDialogueEvent;
    public System.Action turnDialogueEvent;
    public System.Action<Emotion> changeSprite;
    public System.Action<Character> changeCharacter;

    [SerializeField] public int actualLineIndex;
    
    private Character[] characters;
    
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
        EnterCharacter();
    }

    private void Awake()
    {
        this.characters = new Character[] {
            Crissy.Make(this.portraitCrissy),
            Melody.Make(this.portraitMelody),
            Arachne.Make(this.portraitArachne),
        };

        character = characters[0];
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
        float actualSpeed = character.getSpeed(isUrgent);

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
                actualSpeed = character.getSpeed(isUrgent);
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


    public void FailDialogue(Indication indication, Indication prevIndication)
    {
        var dialogue = character.GetFailDialogue();
        

        ScoreManager.Instance.RemoveStar(1);

        AudioManager.Instance.PlaySound("wrong_answer");

        this.StartCoroutine(this._ShowUrgent(dialogue.GetText(indication, prevIndication)));
    }

    private IEnumerator _ShowUrgent(string text)
    {
        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);
        this.needsUrgentDialogue = true;
        StartCoroutine(this.ShowDialogue(text, true));
        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);
        this.needsUrgentDialogue = false;
    }

    public void NormalDialogue(Dialogue dialogue)
    {
        this.changeSprite.Invoke(dialogue.emotion);
        StartCoroutine(this.ShowDialogue(dialogue.GetText(), false));
    }

    public IEnumerator AskQuestions()
    {
        while(true) {
            yield return new WaitUntil(() => canShowUrgentDialogue);
            yield return new WaitUntil(() => !isAskingDirections);
            yield return new WaitForSeconds(Random.Range(1, 4));
            this.needsUrgentDialogue = true;
            this.canShowQuestion = true;
            QuestionDialogue questionDialogue = character.getRandomQuestionDialogue();
            StartCoroutine(this._ShowQuestionRoutine(questionDialogue));
        }
    }

    public void GiveIndication(Indication indication)
    {
        this.isAskingDirections = true;
        if(canShowUrgentDialogue && indication != Indication.Continue) {
            needsUrgentDialogue = true;
            TurnDialogue turnDialogue = character.getIndication();
            changeSprite.Invoke(turnDialogue.emotion);
            StartCoroutine(this._ShowTurnDialogueRoutine(turnDialogue, indication));
        }
    }

    private IEnumerator _ShowQuestionRoutine(QuestionDialogue dialogue)
    {
        int k = dialogue.Options.Length;
        string fullDialogue = null;
        this.askQuestion(dialogue.Correct);

        for(int i = 0; i <= k; i++) {
            fullDialogue = " ";
            if (i == 0) { // Dialogo inicial
                fullDialogue += dialogue.GetText();
            } else if (i == k) { // Dialogo final
                yield return new WaitForSeconds(2);
            } else {
                fullDialogue = dialogue.Options[i - 1];
            }

            yield return new WaitUntil(() => this.canShowUrgentDialogue);

            this.StartCoroutine(this.ShowDialogue(fullDialogue, true));
        }

        yield return new WaitUntil(() => this.canShowUrgentDialogue);

        fullDialogue = this.answeredCorrectly() ? dialogue.CorrectDialogue : dialogue.FailDialogue;

        this.StartCoroutine(this.ShowDialogue(fullDialogue, true));

        yield return new WaitUntil(() => this.canShowUrgentDialogue);

        this.canShowQuestion = false;
        this.needsUrgentDialogue = false;
    }

    private IEnumerator _ShowTurnDialogueRoutine(TurnDialogue newDialogue, Indication indication)
    {
        bool canShowUrgentDialogue = this.canShowUrgentDialogue;
        string fullDialogue = newDialogue.GetText(indication);
        fullDialogue += "..As I was saying before; ";
        yield return new WaitUntil(() => this.canShowUrgentDialogue);
        this.StartCoroutine(this.ShowDialogue(fullDialogue, true));
        yield return new WaitUntil(() => this.canShowUrgentDialogue);
        this.needsUrgentDialogue = false;
        this.isAskingDirections = false;
    }

    public void NextCharacter()
    {
        AudioManager.Instance.PlaySound("correct_answer");
        this.StopAllCoroutines();

        this.characterIndex = (this.characterIndex + 1) % this.characters.Length;
        this.EnterCharacter();
        this.changeCharacter?.Invoke(this.character);
    }

    public void EnterCharacter()
    {
        this.ClearAllText();
        this.character = this.characters[characterIndex];
        this.isFinished = false;
        this.canShowNormalialogue = true;
        this.StartCoroutine(this._ShowUrgent(this.character.getIntroduction().GetText()));
        this.StartCoroutine(this.ShowNormalDialogue());
    }

    public IEnumerator ShowNormalDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        while (true) {
            yield return new WaitUntil(() => this.canShowNormalialogue);
            Dialogue dialogue = this.character.GetPossibleDialogue();
            this.normalDialogueEvent?.Invoke(dialogue);
        }
    }
}


