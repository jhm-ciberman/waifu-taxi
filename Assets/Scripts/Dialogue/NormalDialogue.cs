using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(showNormalDialogue());
    }

    public IEnumerator showNormalDialogue()
    {
        Pasajero pasajero = DialogueManager.I.pasajero;
        while(true)
        {
            yield return new WaitUntil(()=>DialogueManager.I.canShowNormalialogue);
            Dialogue dialogue= pasajero.getPossibleDialogue();
            DialogueManager.I.normalDialogueEvent.Invoke(dialogue.Text);
        }
    }


}
