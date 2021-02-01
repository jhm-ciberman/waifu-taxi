using WaifuTaxi;

public static class Melody
{
    public static Pasajero Make(Portrait portrait)
    {
        var p = new Pasajero(portrait);

        p.AddIntroduction("Hello. **");
        p.AddIntroduction("Good afternoon. **");
        p.AddIntroduction("Good evening. **");
        p.AddIntroduction("Hello. **");
        p.AddIntroduction("How are you? **");

        p.AddDialogue("I don’t come to this town very often.* When I was a kid my parents took me here so many times. *I still remember how fun it was. **");
        p.AddDialogue("I have a very busy day. *I’m in charge of a few pets and I really care for them. *I have 3 dogs, 2 cats, a rabbit and a hamster. *I’m like their mother at this point and they are my babies. **");
        p.AddDialogue("My mother said today will be rainy* so I kept my little babies inside the house. *I hope they didn’t make a mess... **");
        p.AddDialogue("I don’t hang out very much. *I have all I want inside my house, that is my babies and Netflix. **");
        p.AddDialogue("Hahahaha, this meme is soooooo good. *The jojos did it again. *I love Dio, when he is on a meme I know it’ll be great. **");
        p.AddDialogue("I hope we will get to our destination quickly. *I’m sure my babies miss me already. *I miss them too and I have to get my daily dose of Netflix. **");
        p.AddDialogue("Sorry I didn’t get the address but my directions are the best. *You will get used to it quickly. *I know the road to my destination like the back of my hand. **");
        p.AddDialogue("My Instagram is full of memes right now. *I can keep scrolling all day, it seems I’m addicted to it. *I don’t understand how people lived before social media. **");
        p.AddDialogue("I really like this car, someday I will get one too. *I would like some small and cute car. **");
        p.AddDialogue("No hard feelings but I can’t listen to dubstep anymore, it’s so outdated. *Now it’s the K-pop era, I can’t stop listening to it. *We have so much to learn from asian culture. **");
        p.AddDialogue("There is so much transit today. *I got to see so many cars. *It seems like car sales were a really good deal in this town. **");
        p.AddDialogue("My little babies are waiting for me at home. *Yes, I’m talking about my pets. **");
        p.AddDialogue("I don’t understand people who hate pets. *They are the cutest thing in the world and they give unconditional love. **");
        p.AddDialogue("There is so much fun in the little things. **");
        p.AddDialogue("I really like hamsters. *They are so fluffy and cute. *I have one in my house and it is the fastest there are, it runs about 2 hours a day. **");
        p.AddDialogue("I got to find a new anime to start watching. *I watched 5 anime already this season. *Shingeki no Kyojin was astounding, it is so sad that it reached its end. **");
        p.AddDialogue("There are so many good animes on Netflix. *Although I miss some of them so *I’m subscribing to CrunchyRoll next month to have more variety. **");

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
        p.AddFailDialogue("Do you know how to follow directions? *[dir]. **", Emotion.Angry);
        p.AddFailDialogue("I said [dir], well then turn [dir]. **", Emotion.Angry);
        p.AddFailDialogue("It was the other [dir]! *Turn [dir]. **", Emotion.Angry);
        p.AddFailDialogue("It was the other way around. *Turn [dir]. **", Emotion.Angry);
        p.AddFailDialogue("We are never going to reach my destination this way. *[dir]. **", Emotion.Angry);
        p.AddFailDialogue("I didn’t know I was on a city tour. *Turn [dir]. **", Emotion.Angry);

        string[] opciones = new string[3] {
            "(1) *Yes", 
            "(2) *No", 
            "(3) *I like almost every music genre.",
        };

        p.AddQuestion("So, do you like pop music?", Emotion.Asking,opciones,1,"Great, I like it too","Oh, I see…",
        "aa");

        string[] opciones2 = new string[3] {
            "(1) *K-pop",
            "(2) *Rock",
            "(3) *I like almost every music genre",
        };       
        
        p.AddQuestion("What is your favorite music genre?", Emotion.Asking,opciones2,2,"Great, I like it too. ","Oh, I see…",
        "aa");

        return p;
    }
}
