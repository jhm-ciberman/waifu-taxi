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
        yield return new WaitForSeconds(0.5f); //Esto esta muy feo
        while(true)
        {
            yield return new WaitUntil(()=>DialogueManager.I.canShowNormalialogue);
            Dialogue dialogue= DialogueManager.I.pasajero.getPossibleDialogue();
            DialogueManager.I.normalDialogueEvent.Invoke(dialogue);
        }
    }


}
