using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public  class NewDialogueEvent:UnityEvent<Pasajero,Dialogue>{}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDialogue;
    private bool canShowNormalialogue;
    private bool needsUrgentDialogue;
    private Pasajero pasajero;
    public NewDialogueEvent newDialogueEvent;
    
    public static DialogueManager I;

    public void Start()
    {
        canShowNormalialogue=true;
        this.pasajero = new Pasajero1();
        StartCoroutine(showNormalDialogue());
    }

    private void Awake()
    {
        I=this;
        newDialogueEvent=new NewDialogueEvent();
    }

    public void test()
    {
        Debug.Log("test");
    }

    public IEnumerator showNormalDialogue()
    {
        while(true)
        {
            yield return new WaitUntil(()=>canShowNormalialogue);
            {
                string actualText = "";
                Dialogue newDialogue = pasajero.getPossibleDialogue();
                string fullDialogue = newDialogue.Text;
                newDialogueEvent.Invoke(pasajero,newDialogue);
                for(int i=0;i<fullDialogue.Length;i++)
                {
                    if(fullDialogue[i]=='*')
                    {
                        Debug.Log("test");
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        actualText+= fullDialogue[i];
                        textDialogue.text=actualText;
                        yield return new WaitForSeconds(0.05f);
                    }
                    
                }
            }

        }
    }

}
