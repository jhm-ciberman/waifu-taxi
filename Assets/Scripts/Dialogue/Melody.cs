using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melody : Pasajero
{

    public override void addDialogue()
    {
        this.introduction=new Dialogue("Hello",Dialogue.emotions.normal);
        this.addPossibleDialogue("I don’t come to this town very often. When I was a kid my parents took me here so many times. I still remember how fun it was");
        this.addPossibleDialogue("I have a very busy day. I’m in charge of a few pets and I really care for them. I have 3 dogs, 2 cats, a rabbit and a hamster. I’m like their mother at this point and they are my babies");
        this.addPossibleDialogue("My mother said today will be rainy so I kept my little babies inside the house. I hope they didn’t make a mess...");
        this.addPossibleDialogue("I don’t hang out very much. I have all I want inside my house, that is my babies and Netflix");

        this.addTurnLeftDialogue("Cold you please turn left?",Dialogue.emotions.angry);
        this.addTurnRightDialogue("Cold you please turn right?",Dialogue.emotions.angry);
    }
}
