public class Melody : Pasajero
{
    public override void addDialogue()
    {
        addIntroduction("Hello. **");
        addIntroduction("Good afternoon. **");
        addIntroduction("Good evening. **");
        addIntroduction("Hello. **");
        addIntroduction("How are you? **");

        addPossibleDialogue("I don’t come to this town very often.* When I was a kid my parents took me here so many times. *I still remember how fun it was. **");
        addPossibleDialogue("I have a very busy day. *I’m in charge of a few pets and I really care for them. *I have 3 dogs, 2 cats, a rabbit and a hamster. *I’m like their mother at this point and they are my babies. **");
        addPossibleDialogue("My mother said today will be rainy* so I kept my little babies inside the house. *I hope they didn’t make a mess... **");
        addPossibleDialogue("I don’t hang out very much. *I have all I want inside my house, that is my babies and Netflix. **");
        addPossibleDialogue("Hahahaha, this meme is soooooo good. *The jojos did it again. *I love Dio, when he is on a meme I know it’ll be great. **");
        addPossibleDialogue("I hope we will get to our destination quickly. *I’m sure my babies miss me already. *I miss them too and I have to get my daily dose of Netflix. **");
        addPossibleDialogue("Sorry I didn’t get the address but my directions are the best. *You will get used to it quickly. *I know the road to my destination like the back of my hand. **");
        addPossibleDialogue("My Instagram is full of memes right now. *I can keep scrolling all day, it seems I’m addicted to it. *I don’t understand how people lived before social media. **");
        addPossibleDialogue("I really like this car, someday I will get one too. *I would like some small and cute car.  **");
        addPossibleDialogue("No hard feelings but I can’t listen to dubstep anymore, it’s so outdated. *Now it’s the K-pop era, I can’t stop listening to it. *We have so much to learn from asian culture. **");
        addPossibleDialogue("There is so much transit today. *I got to see so many cars. *It seems like car sales were a really good deal in this town. **");
        addPossibleDialogue("My little babies are waiting for me at home. *Yes, I’m talking about my pets. **");
        addPossibleDialogue("I don’t understand people who hate pets. *They are the cutest thing in the world and they give unconditional love. **");
        addPossibleDialogue("There is so much fun in the little things. **");
        addPossibleDialogue("I really like hamsters. *They are so fluffy and cute. *I have one in my house and it is the fastest there are, it runs about 2 hours a day. **");
        addPossibleDialogue("I got to find a new anime to start watching. *I watched 5 anime already this season. *Shingeki no Kyojin was astounding, it is so sad that it reached its end. **");
        addPossibleDialogue("There are so many good animes on Netflix. *Although I miss some of them so *I’m subscribing to CrunchyRoll next month to have more variety. **");

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
        this.addFailDialogue("Do you know how to follow directions? *[dir]. **", Emotion.angry);
        this.addFailDialogue("I said [dir], well then turn [dir]. **", Emotion.angry);
        this.addFailDialogue("It was the other [dir]! *Turn [dir]. **", Emotion.angry);
        this.addFailDialogue("It was the other way around. *Turn [dir]. **", Emotion.angry);
        this.addFailDialogue("We are never going to reach my destination this way. *[dir]. **", Emotion.angry);
        this.addFailDialogue("I didn’t know I was on a city tour. *Turn [dir]. **", Emotion.angry);

        string[] opciones = new string[3] {
            "(1) *Yes", 
            "(2) *No", 
            "(3) *I like almost every music genre.",
        };

        this.addQuestionDialogue("So, do you like pop music?", Emotion.asking,opciones,1,"Great, I like it too","Oh, I see…",
        "aa");

        string[] opciones2 = new string[3] {
            "(1) *K-pop",
            "(2) *Rock",
            "(3) *I like almost every music genre",
        };       
        
        this.addQuestionDialogue("What is your favorite music genre?", Emotion.asking,opciones2,2,"Great, I like it too. ","Oh, I see…",
        "aa");
    }
}
