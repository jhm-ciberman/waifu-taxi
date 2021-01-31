using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasajero1 : Pasajero
{
    public override void addDialogue()
    {
        this.introduction=new Dialogue("Hola, como estas?* Mi nombre es ",Dialogue.emotions.asking);

        this.possibleDialogue.Add(new Dialogue("Esto un dialogo de prueba.",Dialogue.emotions.angry));
        this.possibleDialogue.Add(new Dialogue("Este es otro dialogo de prueba.",Dialogue.emotions.blush));
        this.possibleDialogue.Add(new Dialogue("Mas dialogo random.",Dialogue.emotions.asking));

        this.addTurnLeftDialogue("podes girar a la izquierda plz",Dialogue.emotions.blush);
        this.addTurnRightDialogue("gira a derecha",Dialogue.emotions.angry);

        string[] opciones = new string[3];
        opciones[0]="(1) opcion 1 ";
        opciones[1]="(2) opcion 2 ";
        opciones[2]="(3) opcion 3 ";
        this.addQuestionDialogue("Pregunta de multiple choice,",Dialogue.emotions.angry,opciones,2,"Que vas responder?","Muy bien, respondiste correctamente",
        "Vos sos tarado?");
    }















    
}
