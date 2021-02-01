public class Dialogue
{
    public string text;
    public Emotion emotion;

    public Dialogue(string text, Emotion emotion)
    {
        this.text = text;
        this.emotion = emotion;
    }

    public Dialogue(string text)
    {
        this.text = text;
        this.emotion = Emotion.Normal;
    }
}
