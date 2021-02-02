using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
   [SerializeField] public float stars;

   [SerializeField] private Image[] startImage;

   public static ScoreManager Instance;

   public void Awake()
   {
       Instance = this;
   }

   public void Start()
   {
       this.stars = 4;
       SetStartSprite();
   }

   public void Restart()
   {
       this.stars = 4;
       SetStartSprite();
   }

   public void AddStar(float amount)
   {
        if(stars + amount <= 5) {
            this.stars += stars;
        } else {
            stars=5;
        }
       SetStartSprite();
   }

   public void RemoveStar(float amount)
   {
       if(stars - amount == 0) {
           DialogueManager.Instance.NextCharacter();
           Restart();
       } else {
           this.stars -= amount;
           SetStartSprite();
       }
   }

   public void SetStartSprite()
   {
       int whole = (int) Mathf.Round(stars);
       for(int i = 0; i<5; i++) {
           startImage[i].enabled=true;
       }
       for(int i = 4; i >= whole; i--) {
           startImage[i].enabled = false;
       }
   }
}
