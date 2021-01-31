using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
   public float starts{get; private set;}

   [SerializeField] private Image[] startImage;

   public static ScoreManager I;

   public void Awake()
   {
       I=this;
   }

   public void Start()
   {
       this.starts=3;
       setStartSprite();
   }

   public void addStar(float amount)
   {
        if(starts+amount<=5)
            this.starts+=starts;
        else if(starts+amount<=0)
        {
           //Nuevo pasajero
        }
        else
            starts=5;
       setStartSprite();
   }

   public void setStartSprite()
   {
       int whole = (int)Mathf.Round(starts);
       for(int i=4;i>=whole;i--)
       {
           startImage[i].enabled = false;
       }
   }

}
