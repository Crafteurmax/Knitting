using System;
using System.Collections.Generic;
using System.Globalization;

namespace Runtime.ANTLR.Value
{
    public class FloatValue : Value
    {
        public float Data { get; }

        public FloatValue(float value)
        {
            Data = value;
        }

        public override ValueType valueType => ValueType.Floating;

        public override int CompareTo(Value other)
        {
            if (other is not FloatValue o)
                throw new NotImplementedException(); // TODO

            return Data.CompareTo(o.Data);
        }

        public override string ToString() => Data.ToString(CultureInfo.InvariantCulture);

        public override string Collapse(Story story, StoryNode storyNode) => ToString();
        
        protected override string AsString()  => ToString();
        protected override int AsInt() => (int) Data;
        protected override float AsFloat() => Data;
    }
}
