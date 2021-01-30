using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PasajeroSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer pasajeroSprite;

    public Sprite placeHolderHappy;
    public Sprite placeHolderAngry;
    public Sprite placeHolderEmbarrassed;

    public static PasajeroSprite I;

    private void Awake()
    {
        I=this;
    }

    public void Start()
    {
        ///DialogueManager.I.newDialogueEvent.AddListener(changeExpression);
    }


    void changeExpression(Pasajero pasajero,Dialogue dialogue)
    {
        pasajeroSprite.sprite=pasajero.GetSprite(dialogue.Emotion);

    }
}
