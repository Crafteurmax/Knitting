namespace Runtime.ANTLR
{
    public class Text  : Statement
    {
        private string text;

        public Text(string text)
        {
            this.text = text;
        }
        public override string ToString()
        {
            return text;
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            return text;
        }
    }
}
