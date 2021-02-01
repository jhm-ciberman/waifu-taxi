using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crissy : Pasajero
{
   public override void addDialogue()
    {
        addIntroduction("Hello. **");
        addIntroduction("Good afternoon. **");
        addIntroduction("Good evening. **");
        addIntroduction("Hello. **");
        addIntroduction("How are you? **");

        addPossibleDialogue("I don’t come to this town very often. **");
        addPossibleDialogue("I have a very busy day. **");
        addPossibleDialogue("My mother said today will be rainy, *I don’t wanna ruin my new shoes! **");
        addPossibleDialogue("I miss hanging out with my friends. **");
        addPossibleDialogue("ALSÑFJALÑSFKJ, this meme is soooooo good. **");
        addPossibleDialogue("I hope we will get home quickly. **");
        addPossibleDialogue("Sorry I didn’t get the address but my directions are the best. **");
        addPossibleDialogue("My TikTok is full of memes right now. **");
        addPossibleDialogue("I really like this car, someday I will get one too… **");
        addPossibleDialogue("I have some peculiar musical tastes, Girl in red is my favorite! **");
        addPossibleDialogue("I miss my kitty, her name is Pocky! *she’s so pretty and loves ham! **");
        addPossibleDialogue("There is so much transit today! I hope I'm not bothering. *I'm sooo excited to get home. **");
        addPossibleDialogue("My mom is so nice and caring, someday I will be a fantastic mom like her! **");
        addPossibleDialogue("Sorry if I'm talking too much I can't control my emotions! **");
        addPossibleDialogue("Hope my best friend is home too! mom likes to surprise me! *even if I know what is going to happen it excites me just the same! *It's like spoilers, I'll be surprised as if I had never known, I can't help it ... *or is it that I forget things very quickly? **");
        addPossibleDialogue("I want some chocolate cookies rn I’m so hungy. **");
        addPossibleDialogue("Hope my hair is looking oki. **");
        addPossibleDialogue("My mother is waiting for me at home, she made my favorite cookies!. **");
        addPossibleDialogue("I don’t understand people who hate chocolate, my best friend does and I think it’s crazy! **");
        addPossibleDialogue("Most of the TikToks are soo cringe, but.. *there are really cute girls there.. **");
        addPossibleDialogue("Oh wow this girl dances so nicee she so cuteee. **");
        addPossibleDialogue("Oh.. I think I’m sleepy too, but I’m also full of energy! what is this feeling aaaaaaa. **");
        addPossibleDialogue("I miss when I was a cheerleader! I miss my old friends. **");
        addPossibleDialogue("I need coffee. **");
        addPossibleDialogue("I can't decide on a single type of aesthetic when I dress, sometimes I want to be retro, other times gothic, others I want to cosplay, *others use pastel colors and most of the time I just want to dress comfy with a very long-sleeved sweater.. **");
        addPossibleDialogue("I have some gifts for everyone! even for my kitty! **");
        addPossibleDialogue("I was on a diet for a long time, I think I'm going to ruin it just for this day, Mom cooks things too delicious to refuse. *Besides, she would kill me if I went to visit her after a long time and didn't eat the things she prepared especially for me! *I have to be considerate! so I'm going to sacrifice for her! **");
        addPossibleDialogue("I'm 22 but my mom still treats me like her little girl, even now that I live alone! I love her so much. **");
        
        addIndicationDialogue("[Dir] ");
        addIndicationDialogue("[Dir] please ");
        addIndicationDialogue("[Dir]! ");
        addIndicationDialogue("we have to go [dir] in the next turn. ");
        addIndicationDialogue("[Dir] we go. ");
        addIndicationDialogue("Turn [dir]. ");
        addIndicationDialogue("Turn [dir]! ");
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
            "(1) *Yes.* ",
            "(2) *No.* ",
            "(3) *I like almost every music genre.**",
        };

        this.addQuestionDialogue("So, do you like pop music? **", Emotion.asking, opciones, 1, "Great, I like it too** ", "Oh, I see…** ", "aa");

        string[] opciones2 = new string[3] {
            "(1) *K-pop",
            "(2) *Rock",
            "(3) *I like almost every music genre",
        };

        this.addQuestionDialogue("What is your favorite music genre?", Emotion.asking, opciones2, 2, "Great, I like it too. ","Oh, I see…", "aa");

        /*string[] opciones3 = new string[3];
        opciones[0]="(1) aa";
        opciones[1]="(2) aa";
        opciones[2]="(3) aa";
        this.addQuestionDialogue("What is your favourite drink?",Dialogue.emotions.asking,opciones3,2,"aa","aa",
        "aa");
        */
    }
}
