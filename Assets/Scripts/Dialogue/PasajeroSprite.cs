using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasajeroSprite : MonoBehaviour
{
    [SerializeField] private Image pasajeroSprite;
    [SerializeField] private Pasajero pasajero;

    public static PasajeroSprite I;

    private void Awake()
    {
        I=this;
    }

    public void Start()
    {
        DialogueManager.I.normalDialogueEvent.AddListener(changeExpression);
        DialogueManager.I.turnDialogueEvent.AddListener(changeExpression);
    }


    void changeExpression(Dialogue dialogue)
    {
        Sprite sprite=null;
        Dialogue.emotions emotion =dialogue.Emotion;
        if(emotion == Dialogue.emotions.angry)
        {
            sprite =pasajero.spriteAngry;
        }
        else if(emotion == Dialogue.emotions.asking)
        {
            sprite =pasajero.spriteAsking;
        }
        else if(emotion == Dialogue.emotions.blush)
        {
            sprite =pasajero.spriteBlush;
        }
        else if(emotion == Dialogue.emotions.normal)
        {
            sprite =pasajero.spriteNormal;
        }
        Debug.Assert(sprite!=null,"No obtuvo el sprite");
        pasajeroSprite.sprite=sprite;
    }
}
