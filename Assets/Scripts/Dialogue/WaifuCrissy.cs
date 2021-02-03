using WaifuTaxi;

public static class WaifuCrissy
{
    public static Character Make(Portrait portrait)
    {
        var p = new Character(portrait);
        p.AddIntroduction("Hello. **");
        p.AddIntroduction("Good afternoon. **");
        p.AddIntroduction("Good evening. **");
        p.AddIntroduction("Hello. **");
        p.AddIntroduction("How are you? **");

        p.AddDialogue("I don’t come to this town very often. **");
        p.AddDialogue("I have a very busy day. **");
        p.AddDialogue("My mother said today will be rainy, *I don’t wanna ruin my new shoes! **");
        p.AddDialogue("I miss hanging out with my friends. **");
        p.AddDialogue("ALSÑFJALÑSFKJ, this meme is soooooo good. **");
        p.AddDialogue("I hope we will get home quickly. **");
        p.AddDialogue("Sorry I didn’t get the address but my directions are the best. **");
        p.AddDialogue("My TikTok is full of memes right now. **");
        p.AddDialogue("I really like this car, someday I will get one too… **");
        p.AddDialogue("I have some peculiar musical tastes, Girl in red is my favorite! **");
        p.AddDialogue("I miss my kitty, her name is Pocky! *she’s so pretty and loves ham! **");
        p.AddDialogue("There is so much transit today! I hope I'm not bothering. *I'm sooo excited to get home. **");
        p.AddDialogue("My mom is so nice and caring, someday I will be a fantastic mom like her! **");
        p.AddDialogue("Sorry if I'm talking too much I can't control my emotions! **");
        p.AddDialogue("Hope my best friend is home too! mom likes to surprise me! *even if I know what is going to happen it excites me just the same! *It's like spoilers, I'll be surprised as if I had never known, I can't help it ... *or is it that I forget things very quickly? **");
        p.AddDialogue("I want some chocolate cookies rn I’m so hungy. **");
        p.AddDialogue("Hope my hair is looking oki. **");
        p.AddDialogue("My mother is waiting for me at home, she made my favorite cookies!. **");
        p.AddDialogue("I don’t understand people who hate chocolate, my best friend does and I think it’s crazy! **");
        p.AddDialogue("Most of the TikToks are soo cringe, but.. *there are really cute girls there.. **");
        p.AddDialogue("Oh wow this girl dances so nicee she so cuteee. **");
        p.AddDialogue("Oh.. I think I’m sleepy too, but I’m also full of energy! what is this feeling aaaaaaa. **");
        p.AddDialogue("I miss when I was a cheerleader! I miss my old friends. **");
        p.AddDialogue("I need coffee. **");
        p.AddDialogue("I can't decide on a single type of aesthetic when I dress, sometimes I want to be retro, other times gothic, others I want to cosplay, *others use pastel colors and most of the time I just want to dress comfy with a very long-sleeved sweater.. **");
        p.AddDialogue("I have some gifts for everyone! even for my kitty! **");
        p.AddDialogue("I was on a diet for a long time, I think I'm going to ruin it just for this day, Mom cooks things too delicious to refuse. *Besides, she would kill me if I went to visit her after a long time and didn't eat the things she prepared especially for me! *I have to be considerate! so I'm going to sacrifice for her! **");
        p.AddDialogue("I'm 22 but my mom still treats me like her little girl, even now that I live alone! I love her so much. **");
        
        p.AddIndication("[Dir] ");
        p.AddIndication("[Dir] please ");
        p.AddIndication("[Dir]! ");
        p.AddIndication("we have to go [dir] in the next turn. ");
        p.AddIndication("[Dir] we go. ");
        p.AddIndication("Turn [dir]. ");
        p.AddIndication("Turn [dir]! ");
        p.AddIndication("Turn [dir] please.");
        p.AddIndication("Turn [dir] in the next one.");


        p.AddFailDialogue("Are you listening? *Turn [dir] now! **", Emotion.Angry);
        p.AddFailDialogue("Do you know how to follow directions? *Left. **", Emotion.Angry);
        p.AddFailDialogue("I said right, well then turn [dir]. **", Emotion.Angry);
        p.AddFailDialogue("It was the other right! *Turn [dir]. **", Emotion.Angry);
        p.AddFailDialogue("It was the other way around. *Turn [dir]. **", Emotion.Angry);
        p.AddFailDialogue("We are never going to reach my destination this way. *[dir]. **", Emotion.Angry);
        p.AddFailDialogue("I didn’t know I was on a city tour. *Turn [dir]. **", Emotion.Angry);

        string[] options = new string[] {
            "(1) *Yes.* ",
            "(2) *No.* ",
            "(3) *I like almost every music genre.**",
        };

        p.AddQuestion("So, do you like pop music? **", Emotion.Asking, options, 1, "Great, I like it too** ", "Oh, I see…** ");

        string[] options2 = new string[] {
            "(1) *K-pop",
            "(2) *Rock",
            "(3) *I like almost every music genre",
        };

        p.AddQuestion("What is your favorite music genre?", Emotion.Asking, options2, 2, "Great, I like it too. ","Oh, I see…");

        return p;
    }
}
