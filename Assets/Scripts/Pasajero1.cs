using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasajero1 : Pasajero
{
    public Pasajero1()
    {
        this.name="Juan";
        this.introduction=new Dialogue("Hola, como estas?* Mi nombre es "+this.name,Dialogue.emotions.happy);

        this.possibleDialogue.Add(new Dialogue("Esto un dialogo de prueba.",Dialogue.emotions.angry));
        this.possibleDialogue.Add(new Dialogue("Este es otro dialogo de prueba.",Dialogue.emotions.embarrased));
        this.possibleDialogue.Add(new Dialogue("Mas dialogo random.",Dialogue.emotions.happy));

        this.addTurnLeftDialogue("podes girar a la izquierda plz?",Dialogue.emotions.happy);
        this.addTurnRightDialogue("gira a derecha?",Dialogue.emotions.angry);

        string[] opciones = new string[3];
        opciones[0]="opcion 1";
        opciones[1]="opcion 2";
        opciones[2]="opcion 3";
        this.addQuestionDialogue("Pregunta de multiple choice,",Dialogue.emotions.angry,opciones,2,"la respueta correcta es la 2 btw");

        this.angrySprite= PasajeroSprite.I.placeHolderAngry;
        this.happySprite= PasajeroSprite.I.placeHolderHappy;
        this.embarrassedSprite=PasajeroSprite.I.placeHolderEmbarrassed;


    }



    
}
