using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDialogueManager : MonoBehaviour
{
    public class QuestionInput
    {
        private int correct = 0;
        private int answer = 0;

        public void askQuestion(int correct)
        {
            this.correct = correct;
            answer = 0;
        }

        public bool answeredCorrectly()
        {
            return answer == correct;
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                answer = 1;
            } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
                answer = 2;
            } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
                answer = 3;
            }
        }
    }

    private QuestionInput questionInput = new QuestionInput();

    public IEnumerator showQuestionRoutine(QuestionDialogue dialogue)
    {
        int k = dialogue.Options.Length;
        string fullDialogue = null;
        questionInput.askQuestion(dialogue.Correct);

        for(int i = 0; i <= k; i++) {
            fullDialogue = " ";
            if (i == 0) { // Dialogo inicial
                fullDialogue += dialogue.Text;
            } else if (i == k) { // Dialogo final
                yield return new WaitForSeconds(2);
            } else {
                fullDialogue = dialogue.Options[i - 1];
            }

            yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);

            StartCoroutine(DialogueManager.Instance.showDialogue(fullDialogue, true));
        }

        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);

        if (questionInput.answeredCorrectly()) {
            fullDialogue = dialogue.CorrectDialogue;
        } else {
            fullDialogue = dialogue.FailDialogue;
        }

        StartCoroutine(DialogueManager.Instance.showDialogue(fullDialogue, true));

        yield return new WaitUntil(() => DialogueManager.Instance.canShowUrgentDialogue);

        DialogueManager.Instance.canShowQuestion = false;
        DialogueManager.Instance.needsUrgentDialogue = false;
    }
}