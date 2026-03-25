namespace Runtime.ANTLR
{
    public abstract class Statement
    {
        public abstract string ToString();
        
        public abstract string Collapse(Story story, StoryNode storyNode);
        
    }
}
