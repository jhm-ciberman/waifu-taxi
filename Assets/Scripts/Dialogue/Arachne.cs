using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arachne : Pasajero
{
    public override void addDialogue()
    {
        addIntroduction("Hello. **");
        addIntroduction("Good afternoon. **");
        addIntroduction("Good evening. **");
        addIntroduction("Hello. **");
        addIntroduction("How are you? **");

        addPossibleDialogue("I don’t come to this town very often, I don’t even like it, *It was better when I was a child. **");
        addPossibleDialogue("I have a very busy day. **");
        addPossibleDialogue("My father said he would come to get me but I know he will forget so I prefer not to wait for him in vain and ruin my day. **");
        addPossibleDialogue("You are surely the person I hate the least today, for the simple fact of not knowing you. **");
        addPossibleDialogue("I know that many people tell you about their day all the time, I don't think you really listen to what they say but I have nothing to lose and I'm used to it. **");
        addPossibleDialogue("Today the day started well, I went to a restaurant that I attended as a child after school. *They serve the best cheddar fries I've ever had, I also had some good beer. **");
        addPossibleDialogue("It seems that my life is pretty boring but the truth is I had lots of fun, *although a little emotion or something new would not hurt. **");
        addPossibleDialogue("I really enjoy alternative rock, all the time, life without music is pointless. **");
        addPossibleDialogue("I love my father very much but he has too many unsolved problems.. *Hope one day he can find meaning in his life back. **");
        addPossibleDialogue("Today is the anniversary of my mother's death, *yeah, a bit of a strong topic to talk to a taxi driver right? hope you don’t mind. **");
        addPossibleDialogue("My father used to cook a lot, now he only eats frozen food, *hope I can replicate some recipe from my mother to remind him how nice it is to cook. **");
        addPossibleDialogue("I think I have a good feeling for today. **");
        addPossibleDialogue("I don’t hang out very much. **");
        addPossibleDialogue("I have some messages from my dad hmm.. *yes.. as I guessed he forgot, glad I decided to take a taxi. **");
        addPossibleDialogue("I actually love cute stuff like.. dunno.. *kuromi? my melody? the aesthetic is very sweet. **");
        addPossibleDialogue("I would apologize for the inconvenience but they pay you for this so I'll keep talking and venting. **");
        addPossibleDialogue("Actually.. you look pretty interested in what I’m talking about, that’s nice I guess. **");
        addPossibleDialogue("I will go to a party tomorrow, hope I can meet someone cool. **");
        addPossibleDialogue("I don’t mind if you take your time driving, I have sooo much to say. **");
        addPossibleDialogue("I wish I was as good with directions as with poems, *but hey, if there weren't lost people like me, you wouldn't eat, right? **");
        addPossibleDialogue("I like your car, pretty cool yea. **");
        addPossibleDialogue("No, my favorite color isn’t black, black is not even a color. **");
        addPossibleDialogue("There is so much transit today, better, more time to vent. **");
        addPossibleDialogue("So.. my dad is waiting for me at home now, hope he’s feeling ok. **");
        addPossibleDialogue("I'm not the only one to come here after a long time to visit my family, *maybe I can meet other people at the party and not the same people as always, *this town never has new people. **");
        addPossibleDialogue("My favorite color is violet tho, I think it’s perfect. **");
        addPossibleDialogue("I am older than I appear, but not that old either, 21 years is a very confusing age and it’s full of changes. **");
        addPossibleDialogue("Will there be girls my age? hope so.. **");

        addIndicationDialogue("[dir]");
        addIndicationDialogue("[dir] please");
        addIndicationDialogue("[dir]!");
        addIndicationDialogue("we have to go [dir] in the next turn.");
        addIndicationDialogue("[dir] we go.");
        addIndicationDialogue("Turn [dir].");
        addIndicationDialogue("Turn [dir]!");
        addIndicationDialogue("Turn [dir] please.");
        addIndicationDialogue("Turn [dir] in the next one.");


        this.addFailDialogue("Are you listening? *Turn [dir] now! **", Emotion.angry);
        this.addFailDialogue("Do you know how to follow directions? *Left. **", Emotion.angry);
        this.addFailDialogue("I said right, well then turn [dir]. **", Emotion.angry);
        this.addFailDialogue("It was the other right! *Turn [dir]. **", Emotion.angry);
        this.addFailDialogue("It was the other way around. *Turn [dir]. **", Emotion.angry);
        this.addFailDialogue("We are never going to reach my destination this way. *[dir]. **", Emotion.angry);
        this.addFailDialogue("I didn’t know I was on a city tour. *Turn [dir]. **", Emotion.angry);

        string[] opciones = new string[3] {
            "(1) *Yes",
            "(2) *No",
            "(3) *I like almost every music genre.",
        };
        
        this.addQuestionDialogue("So, do you like pop music?", Emotion.asking, opciones, 1, "Great, I like it too","Oh, I see…",
        "aa");

        string[] opciones2 = new string[3] {
            "(1) *K-pop",
            "(2) *Rock",
            "(3) *I like almost every music genre",
        };
        
        this.addQuestionDialogue("What is your favorite music genre?", Emotion.asking, opciones2, 2, "Great, I like it too. ","Oh, I see…",
        "aa");
    }
}
