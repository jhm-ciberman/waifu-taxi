using System.Collections;
using UnityEngine;

public class NormalDialogue : MonoBehaviour

{
    private Pasajero pasajero;
    // Start is called before the first frame update
    void Start()
    {
        pasajero=DialogueManager.Instance.pasajero;
    }

    public IEnumerator showNormalDialogue()
    {
        yield return new WaitForSeconds(0.5f); //Esto esta muy feo
        while(true)
        {
            yield return new WaitUntil(()=>DialogueManager.Instance.canShowNormalialogue);
            Dialogue dialogue= DialogueManager.Instance.pasajero.getPossibleDialogue();
            DialogueManager.Instance.normalDialogueEvent.Invoke(dialogue);
        }
    }


}
