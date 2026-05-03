using System;
using System.Collections.Generic;

namespace Runtime.ANTLR.Value
{
    public class StringValue : Value
    {
        public string Data { get; }

        public StringValue(string value)
        {
            Data = value;
        }

        public override ValueType valueType => ValueType.String;

        public override int CompareTo(Value other)
        {
            if (other is not StringValue o)
                throw new NotImplementedException(); // TODO

            return String.Compare(Data, o.Data, StringComparison.InvariantCulture);
        }

        public override string ToString() => $"\"{Data}\"";

        public override string Collapse(Story story, StoryNode storyNode) => Data;
        
        protected override string AsString()  => Data;

        protected override int AsInt()
        {
            int.TryParse(Data, out int value);
            return value;
        }

        protected override float AsFloat()
        {
            float.TryParse(Data, out float value);
            return value;
        }

        protected override bool AsBool()
        {
            if(Data == "true") return true;
            if(Data == "false") return false;
            throw new NotImplementedException();
        }
        public static implicit operator List<Value>(StringValue v) => throw new NotImplementedException(); // TODO
    }
}
