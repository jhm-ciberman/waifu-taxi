using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public  class NewDialogueEvent:UnityEvent<Pasajero,Dialogue>{}

public class DialogueManager : MonoBehaviour
{
    static int MAX_LENGH = 183;
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
        string actualText = "";
        int lastSpace=0;
        while(true)
        {
            yield return new WaitUntil(()=>canShowNormalialogue);
            {
                Dialogue newDialogue = pasajero.getPossibleDialogue();
                string fullDialogue = newDialogue.Text;
                newDialogueEvent.Invoke(pasajero,newDialogue);
                int i=0;
                while(i < fullDialogue.Length)
                {
                    if(fullDialogue[i]==' ')
                    {
                        lastSpace=i;
                    }
                    if(actualText.Length>MAX_LENGH)
                    {
                        actualText="";
                        i=lastSpace;
                    }
                    if(fullDialogue[i]=='*')
                    {
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        actualText+= fullDialogue[i];
                        textDialogue.text=actualText;
                        yield return new WaitForSeconds(0.02f);
                    }
                    i++;
                    if(needsUrgentDialogue)
                    {
                        yield return new WaitUntil(()=>!needsUrgentDialogue);
                        i=lastSpace;
                    }
                }
                actualText+=" ";
            }

        }
    }

}
