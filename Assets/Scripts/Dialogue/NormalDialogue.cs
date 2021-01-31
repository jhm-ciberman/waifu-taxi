using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDialogue : MonoBehaviour

{
    private Pasajero pasajero;
    // Start is called before the first frame update
    void Start()
    {
        pasajero=DialogueManager.I.pasajero;
    }

    public IEnumerator showNormalDialogue()
    {
        while(true)
        {
            yield return new WaitUntil(()=>DialogueManager.I.canShowNormalialogue);
            Dialogue dialogue= DialogueManager.I.pasajero.getPossibleDialogue();
            DialogueManager.I.normalDialogueEvent.Invoke(dialogue);
        }
    }


}
