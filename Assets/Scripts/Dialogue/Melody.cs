using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melody : Pasajero
{

    public override void addDialogue()
    {
        this.introduction=new Dialogue("Hello****",Dialogue.emotions.normal);
        this.addPossibleDialogue("I don’t come to this town very often.* When I was a kid my parents took me here so many times. *I still remember how fun it was. **");
        this.addPossibleDialogue("I have a very busy day. *I’m in charge of a few pets and I really care for them. *I have 3 dogs, 2 cats, a rabbit and a hamster. *I’m like their mother at this point and they are my babies. **");
        this.addPossibleDialogue("My mother said today will be rainy* so I kept my little babies inside the house. *I hope they didn’t make a mess... **");
        this.addPossibleDialogue("I don’t hang out very much. *I have all I want inside my house, that is my babies and Netflix. **");

        this.addTurnLeftDialogue("W-Wait, could you please turn left? **",Dialogue.emotions.angry);
        this.addTurnRightDialogue("W-Wait, could you please turn right? **",Dialogue.emotions.angry);

        this.addFailDialogue("Really? You failed? **",Dialogue.emotions.angry);

        string[] opciones = new string[3];
        opciones[0]="(1) opcion 1*";
        opciones[1]="(2) opcion 2*";
        opciones[2]="(3) opcion 3*";
        this.addQuestionDialogue("Pregunta de multiple choice,*",Dialogue.emotions.angry,opciones,2,"Que vas responder?*","Muy bien, respondiste correctamente",
        "Vos sos tarado?");
    }
}
