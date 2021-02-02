using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Image characterSprite;
    [SerializeField] private Character character;

    public void ChangeCharacter(Character character)
    {
        this.character = DialogueManager.Instance.character;
    }

    public void SetExpression(Emotion emotion)
    {
        switch (emotion) {
            case Emotion.Angry:  this.characterSprite.sprite = character.portrait.angry;  break;
            case Emotion.Asking: this.characterSprite.sprite = character.portrait.asking; break;
            case Emotion.Blush:  this.characterSprite.sprite = character.portrait.blush;  break;
            case Emotion.Normal: this.characterSprite.sprite = character.portrait.normal; break;
        }
    }
}
