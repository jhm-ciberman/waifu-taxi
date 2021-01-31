using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaifuTaxi;

public class TurnDialogueManager : MonoBehaviour
{

    private Indication lastIndication;

    public IEnumerator showTurnDialogueRoutine(TurnDialogue newDialogue,Indication indication)
    {
        bool canShowUrgentDialogue=DialogueManager.I.canShowUrgentDialogue;
        int numeroDeDialogosFalsos =Random.Range(0,1);
        string fullDialogue;
        for(int j=0;j<=numeroDeDialogosFalsos;j++)
        {
            fullDialogue="";
            //Al principio
            if(j==0)
            {
                fullDialogue+=" ";
            }
            //Direccion de verdad
            if(j==numeroDeDialogosFalsos)
            {
                if(newDialogue.indication ==lastIndication)
                {
                    
                }
                fullDialogue+=newDialogue.Text;
                fullDialogue+="..As I was saying before;";
            }
            //Dialogos falsos
            else
            {
                //WIP
            }
            string direction = DialogueManager.indicationToString(indication);
            var directionUpper= char.ToUpper(direction[0]) +direction.Substring(1);
            fullDialogue = fullDialogue.Replace("[dir]",direction);
            fullDialogue = fullDialogue.Replace("[Dir]",directionUpper);
            yield return new WaitUntil(()=>DialogueManager.I.canShowUrgentDialogue);
            IEnumerator newRoutine = DialogueManager.I.showDialogue(fullDialogue,true);
            StartCoroutine(newRoutine);
        }
        yield return new WaitUntil(()=>DialogueManager.I.canShowUrgentDialogue);
        DialogueManager.I.needsUrgentDialogue=false;
        DialogueManager.I.isAskingDirections=false;
    }
}
