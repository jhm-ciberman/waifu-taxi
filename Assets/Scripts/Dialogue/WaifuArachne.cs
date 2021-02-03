namespace WaifuDriver
{
    public static class WaifuArachne
    {
        public static Character Make(Portrait portrait)
        {
            var p = new Character(portrait);

            p.AddIntroduction("Hello. **");
            p.AddIntroduction("Good afternoon. **");
            p.AddIntroduction("Good evening. **");
            p.AddIntroduction("Hello. **");
            p.AddIntroduction("How are you? **");

            p.AddDialogue("I don’t come to this town very often, I don’t even like it, *It was better when I was a child. **");
            p.AddDialogue("I have a very busy day. **");
            p.AddDialogue("My father said he would come to get me but I know he will forget so I prefer not to wait for him in vain and ruin my day. **");
            p.AddDialogue("You are surely the person I hate the least today, for the simple fact of not knowing you. **");
            p.AddDialogue("I know that many people tell you about their day all the time, I don't think you really listen to what they say but I have nothing to lose and I'm used to it. **");
            p.AddDialogue("Today the day started well, I went to a restaurant that I attended as a child after school. *They serve the best cheddar fries I've ever had, I also had some good beer. **");
            p.AddDialogue("It seems that my life is pretty boring but the truth is I had lots of fun, *although a little emotion or something new would not hurt. **");
            p.AddDialogue("I really enjoy alternative rock, all the time, life without music is pointless. **");
            p.AddDialogue("I love my father very much but he has too many unsolved problems.. *Hope one day he can find meaning in his life back. **");
            p.AddDialogue("Today is the anniversary of my mother's death, *yeah, a bit of a strong topic to talk to a taxi driver right? hope you don’t mind. **");
            p.AddDialogue("My father used to cook a lot, now he only eats frozen food, *hope I can replicate some recipe from my mother to remind him how nice it is to cook. **");
            p.AddDialogue("I think I have a good feeling for today. **");
            p.AddDialogue("I don’t hang out very much. **");
            p.AddDialogue("I have some messages from my dad hmm.. *yes.. as I guessed he forgot, glad I decided to take a taxi. **");
            p.AddDialogue("I actually love cute stuff like.. dunno.. *kuromi? my melody? the aesthetic is very sweet. **");
            p.AddDialogue("I would apologize for the inconvenience but they pay you for this so I'll keep talking and venting. **");
            p.AddDialogue("Actually.. you look pretty interested in what I’m talking about, that’s nice I guess. **");
            p.AddDialogue("I will go to a party tomorrow, hope I can meet someone cool. **");
            p.AddDialogue("I don’t mind if you take your time driving, I have sooo much to say. **");
            p.AddDialogue("I wish I was as good with directions as with poems, *but hey, if there weren't lost people like me, you wouldn't eat, right? **");
            p.AddDialogue("I like your car, pretty cool yea. **");
            p.AddDialogue("No, my favorite color isn’t black, black is not even a color. **");
            p.AddDialogue("There is so much transit today, better, more time to vent. **");
            p.AddDialogue("So.. my dad is waiting for me at home now, hope he’s feeling ok. **");
            p.AddDialogue("I'm not the only one to come here after a long time to visit my family, *maybe I can meet other people at the party and not the same people as always, *this town never has new people. **");
            p.AddDialogue("My favorite color is violet tho, I think it’s perfect. **");
            p.AddDialogue("I am older than I appear, but not that old either, 21 years is a very confusing age and it’s full of changes. **");
            p.AddDialogue("Will there be girls my age? hope so.. **");

            p.AddIndication("[dir]");
            p.AddIndication("[dir] please");
            p.AddIndication("[dir]!");
            p.AddIndication("we have to go [dir] in the next turn.");
            p.AddIndication("[dir] we go.");
            p.AddIndication("Turn [dir].");
            p.AddIndication("Turn [dir]!");
            p.AddIndication("Turn [dir] please.");
            p.AddIndication("Turn [dir] in the next one.");


            p.AddFailDialogue("Are you listening? *Turn [dir] now! **", Emotion.Angry);
            p.AddFailDialogue("Do you know how to follow directions? *Left. **", Emotion.Angry);
            p.AddFailDialogue("I said right, well then turn [dir]. **", Emotion.Angry);
            p.AddFailDialogue("It was the other right! *Turn [dir]. **", Emotion.Angry);
            p.AddFailDialogue("It was the other way around. *Turn [dir]. **", Emotion.Angry);
            p.AddFailDialogue("We are never going to reach my destination this way. *[dir]. **", Emotion.Angry);
            p.AddFailDialogue("I didn’t know I was on a city tour. *Turn [dir]. **", Emotion.Angry);

            string[] options = new string[3] {
                "(1) *Yes",
                "(2) *No",
                "(3) *I like almost every music genre.",
            };
            
            p.AddQuestion("So, do you like pop music?", Emotion.Asking, options, 1, "Great, I like it too", "Oh, I see…");

            string[] options2 = new string[3] {
                "(1) *K-pop",
                "(2) *Rock",
                "(3) *I like almost every music genre",
            };
            
            p.AddQuestion("What is your favorite music genre?", Emotion.Asking, options2, 2, "Great, I like it too. ", "Oh, I see…");

            return p;
        }
    }
}