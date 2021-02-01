public class Dialogue
{
    private Emotion emotion;

    public string Text {get; set;}
    
    public Emotion Emotion => emotion;

    public Dialogue(string text, Emotion emotion)
    {
        this.Text = text;
        this.emotion = emotion;
    }

    public Dialogue(string text)
    {
        this.Text = text;
        this.emotion = Emotion.Normal;
    }
}
