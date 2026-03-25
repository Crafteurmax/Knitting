using Runtime.ANTLR.TwineValue;

namespace Runtime.ANTLR.Expression
{
    public abstract class Expression : Statement
    {

        public abstract Value.Value Evaluate(Story story, StoryNode storyNode);
    
    }
}
