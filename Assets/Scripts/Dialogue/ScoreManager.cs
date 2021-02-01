using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
   [SerializeField] public float starts;

   [SerializeField] private Image[] startImage;

   public static ScoreManager Instance;

   public void Awake()
   {
       Instance = this;
   }

   public void Start()
   {
       this.starts = 4;
       setStartSprite();
   }

   public void reiniciar()
   {
       this.starts = 4;
       setStartSprite();
   }

   public void addStar(float amount)
   {
        if(starts + amount <= 5) {
            this.starts += starts;
        } else {
            starts=5;
        }
       setStartSprite();
   }

   public void removeStar(float amount)
   {
       if(starts - amount == 0) {
           DialogueManager.Instance.nextPasajero();
           reiniciar();
       } else {
           this.starts-=amount;
           setStartSprite();
       }
   }

   public void setStartSprite()
   {
       int whole = (int) Mathf.Round(starts);
       for(int i=0;i<5;i++) {
           startImage[i].enabled=true;
       }
       for(int i=4;i>=whole;i--) {
           startImage[i].enabled = false;
       }
   }
}
