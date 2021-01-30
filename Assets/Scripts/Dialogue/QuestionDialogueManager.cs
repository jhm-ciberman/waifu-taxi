using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator showQuestionRoutine(QuestionDialogue dialogue)
    {
        while(true)
        {
            yield return new WaitUntil(()=>DialogueManager.I.canShowQuestion);
            int k=1+dialogue.Options.Length;
            string fullDialogue;
            for(int i=0;i<=k;i++)
            {
                fullDialogue=" ";
                //Dialogo inicial
                if(i==0)
                {
                    fullDialogue+=dialogue.Text;
                }
                //Dialogo final
                else if(i==k)
                {
                    fullDialogue+=dialogue.AfterDialogue;
                }
                //Las opciones
                else
                {
                    fullDialogue=dialogue.Options[i-1];
                }
                yield return new WaitUntil(()=>DialogueManager.I.canShowUrgentDialogue);
                IEnumerator newRoutine = DialogueManager.I.showDialogue(fullDialogue,true);
                StartCoroutine(newRoutine);
            }
            yield return new WaitUntil(()=>DialogueManager.I.canShowUrgentDialogue);
            DialogueManager.I.canShowQuestion=false;
            DialogueManager.I.needsUrgentDialogue=false;
        }
        
    }
}
