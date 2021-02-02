using UnityEngine;
using UnityEngine.UI;

public class PasajeroSprite : MonoBehaviour
{
    [SerializeField] private Image pasajeroSprite;
    [SerializeField] private Pasajero pasajero;

    public void Start()
    {
        DialogueManager.Instance.changePasajero += changePasajero;
        DialogueManager.Instance.changeSprite += changeExpression;
        DialogueManager.Instance.changePasajero += changePasajero;
        this.pasajero = DialogueManager.Instance.pasajero;
    }

    private void changePasajero()
    {
        this.pasajero = DialogueManager.Instance.pasajero;
    }

    void changeExpression(Dialogue dialogue)
    {
        Sprite sprite = null;
        Emotion emotion = dialogue.emotion;

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
