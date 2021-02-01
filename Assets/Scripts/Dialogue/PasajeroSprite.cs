using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasajeroSprite : MonoBehaviour
{
    [SerializeField] private Image pasajeroSprite;
    [SerializeField] private Pasajero pasajero;

    public void Start()
    {
        DialogueManager.Instance.changeSprite.AddListener(changeExpression);
        DialogueManager.Instance.changePasajero.AddListener(changePasajero);
        this.pasajero = DialogueManager.Instance.pasajero;
    }

    private void changePasajero()
    {
        this.pasajero = DialogueManager.Instance.pasajero;
    }

    void changeExpression(Dialogue dialogue)
    {
        Sprite sprite = null;
        Emotion emotion = dialogue.Emotion;

        switch (emotion)
        {
            case Emotion.Angry:  sprite = pasajero.portrait.angry;  break;
            case Emotion.Asking: sprite = pasajero.portrait.asking; break;
            case Emotion.Blush:  sprite = pasajero.portrait.blush;  break;
            case Emotion.Normal: sprite = pasajero.portrait.normal; break;
        }

        Debug.Assert(sprite != null, "No obtuvo el sprite");
        pasajeroSprite.sprite = sprite;
    }
}
