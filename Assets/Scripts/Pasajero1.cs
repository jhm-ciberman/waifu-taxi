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

        this.turnLeftDialogue.Add(new Dialogue("Podes girar a la izquierda plz? Como te estaba diciendo antes,",Dialogue.emotions.happy));
        this.turnRightDialogue.Add(new Dialogue("Podes girar a la derecha plz? Como te estaba diciendo antes,",Dialogue.emotions.happy));

        this.angrySprite= PasajeroSprite.I.placeHolderAngry;
        this.happySprite= PasajeroSprite.I.placeHolderHappy;
        this.embarrassedSprite=PasajeroSprite.I.placeHolderEmbarrassed;


    }



    
}
