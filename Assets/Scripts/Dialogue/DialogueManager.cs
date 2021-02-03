using System.Collections;
using UnityEngine;
using TMPro;

namespace WaifuDriver
{
    public class DialogueManager : MonoBehaviour
    {
        static int MAX_LENGH = 55;
        static float WAIT_SPEED = 0.10f;

        public event System.Action<Emotion> onEmotionChanged;
        public event System.Action<Character> onCharacterChanged;
        public event System.Action onAnswerCorrect;
        public event System.Action onAnswerIncorrect;

        [SerializeField] private TextMeshProUGUI[] _textLines;
        private TextMeshProUGUI _currentTextLine;

        private int _actualLineIndex = 0;
        private int _characterIndex;

        private bool _canShowNormalialogue = true;
        private bool _canShowUrgentDialogue = false;
        private bool _needsUrgentDialogue = false;
        private bool _needsQuestion = false;

        public Character character;

        //TODO: Unecesary event. It's only here to support coroutines. Remove corroutines and remove this. Replace with a regular game loop
        public System.Action<Dialogue> normalDialogueEvent; 

        private Character[] _characters;
        
        private int _correctAnswerIndex = 0;
        private int _userInputAnswerIndex = 0;

        public static DialogueManager Instance; // TODO: Remove singleton

        public Portrait portraitCrissy;
        public Portrait portraitMelody;
        public Portrait portraitArachne;

        private float textFadeSpeed = 2f;

        public void Start()
        {
            this._EnterCharacter();
        }

        private void Awake()
        {
            this._characters = new Character[] {
                WaifuCrissy.Make(this.portraitCrissy),
                WaifuMelody.Make(this.portraitMelody),
                WaifuArachne.Make(this.portraitArachne),
            };

            this.character = this._characters[0];
            DialogueManager.Instance = this;
            this.normalDialogueEvent += this._NormalDialogue;
            this._currentTextLine = this._textLines[0];
        }

        public void AskQuestion(int correct)
        {
            this._correctAnswerIndex = correct;
            this._userInputAnswerIndex = 0;
        }

        private bool _AnsweredCorrectly()
        {
            return this._userInputAnswerIndex == this._correctAnswerIndex;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                this._userInputAnswerIndex = 1;
            } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
                this._userInputAnswerIndex = 2;
            } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
                this._userInputAnswerIndex = 3;
            }
        }

        private IEnumerator _ShowDialogue(string newDialogue, bool isUrgent)
        {
            string actualText = _currentTextLine.text;
            float actualSpeed = character.GetSpeed(isUrgent);

            int lastSpace = 0;
            int i = 0;

            if (!isUrgent) {
                this._canShowNormalialogue=false;
            } else {
                actualText += "- ";
            }

            while (i < newDialogue.Length) {
                if (newDialogue[i] == ' ') {
                    lastSpace = i;
                    int aux = i;
                    while (aux < newDialogue.Length && newDialogue[aux] != ' ') {
                        aux++;
                    }
                    if (i - aux + _currentTextLine.text.Length > MAX_LENGH) {
                        actualText = "";
                        this._actualLineIndex = (this._actualLineIndex + 1) % _textLines.Length;
                        var nextLineIndex = (this._actualLineIndex + 1) % _textLines.Length;
                        _currentTextLine = _textLines[this._actualLineIndex];
                        _currentTextLine.alpha = 1f;
                        StartCoroutine(_FadeOutText(_textLines[nextLineIndex], _currentTextLine));
                    }
                }
                var currentCharacter = newDialogue[i];
                if (currentCharacter == '*') {
                    actualSpeed = character.GetSpeed(isUrgent);
                    var waitSpeed = WAIT_SPEED;
                    if (i < newDialogue.Length - 1 && newDialogue[i + 1] == '*') {
                        i++;
                        waitSpeed *= 2;
                    } 
                    yield return new WaitForSeconds(waitSpeed);
                } else {
                    if (_currentTextLine.text != "" || currentCharacter != ' ') { // Prevent spaces at the start of new line
                        actualText += currentCharacter;
                    }
                    _currentTextLine.text = actualText;
                    yield return new WaitForSeconds(actualSpeed);
                }
                i++;
                
                if (!isUrgent && _needsUrgentDialogue) {
                    if (_currentTextLine.text != "") _currentTextLine.text += " ";
                    this._canShowUrgentDialogue = true;
                    yield return new WaitUntil(() => !_needsUrgentDialogue);
                    actualText = _currentTextLine.text;
                    i = lastSpace;
                }
            }

            if (_currentTextLine.text != "") _currentTextLine.text+=" ";

            if (isUrgent) {
                this._canShowUrgentDialogue = true;
            } else {
                this._canShowNormalialogue = true;
            }
        }

        private IEnumerator _FadeOutText(TextMeshProUGUI lineToFade, TextMeshProUGUI lineInProgress)
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

        private void _ClearAllText()
        {
            for(int i = 0; i < this._textLines.Length; i++) {
                this._textLines[i].text = "";
                this._actualLineIndex = 0;
                this._currentTextLine = this._textLines[0];
            }
        }

        public void FailDialogue(Indication indication, Indication prevIndication)
        {
            var text = character.GetFailDialogue().GetText(indication, prevIndication);
            this.StartCoroutine(this._ShowUrgent(text));
        }

        private IEnumerator _ShowUrgent(string text)
        {
            this._needsUrgentDialogue = true;
            yield return new WaitUntil(() => this._canShowUrgentDialogue);
            this._canShowUrgentDialogue = false;
            this.StartCoroutine(this._ShowDialogue(text, true));
            yield return new WaitUntil(() => this._canShowUrgentDialogue);
            this._canShowUrgentDialogue = false;
            this._needsUrgentDialogue = false;
        }

        private void _NormalDialogue(Dialogue dialogue)
        {
            this.onEmotionChanged?.Invoke(dialogue.emotion);
            StartCoroutine(this._ShowDialogue(dialogue.GetText(), false));
        }

        private IEnumerator _AskQuestions()
        {
            while (true) {
                yield return new WaitForSeconds(Random.Range(20,30));
                var dialogue = character.GetQuestionDialogue();
                StartCoroutine(this._ShowQuestionRoutine(dialogue));
            }
        }

        public void GiveIndication(Indication indication)
        {
            if (indication == Indication.Continue) return;

            var dialogue = character.GetIndication();
            this.onEmotionChanged?.Invoke(dialogue.emotion);
            this.StartCoroutine(this._ShowTurnDialogueRoutine(dialogue, indication));
        }

        private IEnumerator _ShowQuestionRoutine(Question dialogue)
        {
            int k = dialogue.Options.Length;
            string fullDialogue = " ";
            this.AskQuestion(dialogue.Correct);
            this._needsQuestion = true;

            for(int i = 0; i <= k; i++) {
                fullDialogue += (i == 0) ? dialogue.GetText() : dialogue.Options[i - 1];
            }
            
            yield return new WaitUntil(() => this._canShowNormalialogue);
            
            this.StartCoroutine(this._ShowDialogue(fullDialogue,false));
            this._needsQuestion = false;
            yield return new WaitForSeconds(2);
            
            if (this._AnsweredCorrectly()) {
                fullDialogue = dialogue.CorrectDialogue;
                this.onAnswerCorrect?.Invoke();
            } else {
                fullDialogue = dialogue.FailDialogue;
                this.onAnswerIncorrect?.Invoke();
            }

            this.StartCoroutine(this._ShowUrgent(fullDialogue));
            yield return new WaitUntil(() => this._canShowNormalialogue);
        }

        private IEnumerator _ShowTurnDialogueRoutine(Dialogue newDialogue, Indication indication)
        {
            string fullDialogue = newDialogue.GetText(indication);
            fullDialogue += "..As I was saying before; ";
            this.StartCoroutine(this._ShowUrgent(fullDialogue));
            yield return new WaitForSeconds(0.1f);
        }

        public void NextCharacter()
        {
            this.StopAllCoroutines();
            this._characterIndex = (this._characterIndex + 1) % this._characters.Length;
            this._EnterCharacter();
            this.onCharacterChanged?.Invoke(this.character);
        }

        private void _EnterCharacter()
        {
            this._ClearAllText();
            this.character = this._characters[this._characterIndex];
            this._NormalDialogue(this.character.GetIntroduction());
            this.StartCoroutine(this._ShowNormalDialogue());
            this.StartCoroutine(this._AskQuestions());
        }

        private IEnumerator _ShowNormalDialogue()
        {
            while (true) {
                yield return new WaitUntil(() => this._canShowNormalialogue && !_needsQuestion);
                Dialogue dialogue = this.character.GetPossibleDialogue();
                this.normalDialogueEvent?.Invoke(dialogue);
            }
        }
    }
}
