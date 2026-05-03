using System;
using System.Collections.Generic;
using System.Globalization;

namespace Runtime.ANTLR.Value
{
    public class BoolValue : Value
    {
        public bool Data { get; }

        public BoolValue(bool value)
        {
            Data = value;
        }

        public override ValueType valueType => ValueType.Boolean;

        public override int CompareTo(Value other)
        {
            if (other is not BoolValue o)
                throw new NotImplementedException(); // TODO

            return Data.CompareTo(o.Data);
        }

        public override string ToString() => Data.ToString();
        

        public override string Collapse(Story story, StoryNode storyNode) => ToString();
        
        protected override string AsString() => ToString();
        protected override int AsInt() => Data ?  1 : 0;
        protected override float AsFloat() => Data ?  1f : 0f;
        protected override bool AsBool() => Data;
    }
}
