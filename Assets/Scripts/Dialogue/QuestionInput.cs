using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionInput : MonoBehaviour
{
   [SerializeField] private bool canAnswer;
   [SerializeField] private int correct,answer;

   public void askQuestion(int correct)
   {
       this.correct=correct;
       canAnswer=true;
       answer=0;
   }

   public bool answeredCorrectly()
   {
       return answer==correct;
   }

   public void Update()
   {

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                answer=1;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                answer=2;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                answer=3;
            }
       
   }
}
