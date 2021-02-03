using UnityEngine;
using UnityEngine.UI;

namespace WaifuDriver
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] private Image _characterSprite;
        [SerializeField] private StarsGUI _starsGUI;

        private Character _character;


        public void ChangeCharacter(Character character)
        {
            this._character = character;
        }

        public void SetExpression(Emotion emotion)
        {
            switch (emotion) {
                case Emotion.Angry:  this._characterSprite.sprite = this._character.portrait.angry;  break;
                case Emotion.Asking: this._characterSprite.sprite = this._character.portrait.asking; break;
                case Emotion.Blush:  this._characterSprite.sprite = this._character.portrait.blush;  break;
                case Emotion.Normal: this._characterSprite.sprite = this._character.portrait.normal; break;
            }
        }

        public void SetStarsCount(int count)
        {
            this._starsGUI.SetStarsCount(count);
        }
    }
}