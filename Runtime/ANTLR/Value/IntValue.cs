using System;
using System.Collections.Generic;

namespace Runtime.ANTLR.Value
{
    public class IntValue : Value
    {
        public int Data { get; }

        public IntValue(int value)
        {
            Data = value;
        }

        public override ValueType valueType => ValueType.Integer;

        public override int CompareTo(Value other)
        {
            if (other is not IntValue o)
                throw new NotImplementedException(); // TODO

            return Data.CompareTo(o.Data);
        }

        public override string ToString() => Data.ToString();

        public override string Collapse(Story story, StoryNode storyNode) => ToString();
        
        protected override string AsString()  => ToString();
        protected override int AsInt() => Data;
        protected override float AsFloat() => Data;
    }
}
