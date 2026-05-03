using System;
using System.Collections.Generic;

namespace Runtime.ANTLR.Value
{
    public class ArrayValue : Value
    {
        public List<Value> Data { get; }

        public ArrayValue(List<Value> value)
        {
            Data = value;
        }

        public override ValueType valueType => ValueType.Boolean;

        public override int CompareTo(Value other)
        {
            if (other is not BoolValue o)
                throw new NotImplementedException(); // TODO
            
            throw new NotImplementedException(); // TODO
        }

        public override string ToString()
        {
            string toReturn = "{";
            for (int i = 0; i < Data.Count; i++)
            {
                toReturn += Data[i].ToString() + ",";
            }
            toReturn += "}";
            return toReturn;
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            string toReturn = "{";
            for (int i = 0; i < Data.Count; i++)
            {
                toReturn += Data[i].Collapse(story, storyNode) + ",";
            }
            toReturn += "}";
            return toReturn;  
        }
        
    }
}
