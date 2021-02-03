namespace WaifuDriver
{
    public class Question : Dialogue
    {
        private string[] options;
        private int correct;
        private string afterDialogue;
        private string correctDialogue;
        private string failDialogue;

        public string[] Options => options;
        public int Correct => correct;
        public string AfterDialogue => afterDialogue;
        public string CorrectDialogue => correctDialogue;
        public string FailDialogue => failDialogue;

        public Question(
            string text, 
            Emotion emotion, 
            string[] options, 
            int correct, 
            string correctDialogue, 
            string failDialogue
        ) : base(text, emotion)
        {
            this.options = options;
            this.correctDialogue = correctDialogue;
            this.failDialogue = failDialogue;
            this.correct = correct;
        }
    }
}