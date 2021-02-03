using WaifuTaxi;

public class Dialogue
{
    private readonly string _text;

    public readonly Emotion emotion;

    public Dialogue(string text, Emotion emotion)
    {
        this._text = text;
        this.emotion = emotion;
    }

    public Dialogue(string text)
    {
        this._text = text;
        this.emotion = Emotion.Normal;
    }

    private string _IndicationToString(Indication indication)
    {
        string s = "...";
        switch (indication) {
            case Indication.TurnLeft: s = "left";  break;
            case Indication.TurnRight: s = "right"; break;
            case Indication.TurnU: s = "in u"; break;
        }
        return s;
    }

    private string _Replace(string str, Indication indication, string lowercaseToken, string uppercaseToken)
    {
        var currStringLower = this._IndicationToString(indication);
        var currStringUpper = char.ToUpper(currStringLower[0]) + currStringLower.Substring(1);
        str = str.Replace(lowercaseToken, currStringLower);
        str = str.Replace(uppercaseToken, currStringUpper);
        return str;
    }

    public string GetText(Indication currentIndication, Indication prevIndication)
    {
        return this._Replace(this.GetText(currentIndication), prevIndication, "[prev_dir]", "[Prev_dir]");
    }

    public string GetText(Indication currentIndication)
    {
        return this._Replace(this._text, currentIndication, "[dir]", "[Dir]");
    }

    public string GetText() => this._text;
}
