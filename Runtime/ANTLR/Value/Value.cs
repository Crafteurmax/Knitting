using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Runtime.ANTLR.Value
{
    public abstract class Value : Expression.Expression, IComparable<Value>
    {
        public abstract ValueType valueType { get; }

        public abstract int CompareTo(Value other);

        public abstract override string ToString();

        public override Value Evaluate(Story story, StoryNode storyNode)
        {
            return this;
        }
        
        public static implicit operator int(Value v) => v.AsInt();
        public static implicit operator float(Value v) => v.AsFloat();
        public static implicit operator string(Value v) => v.AsString();
        public static implicit operator bool(Value v) => v.AsBool();
        public static implicit operator List<Value>(Value v) => v.AsArray();
        
        protected virtual int AsInt() => throw new InvalidCastException();
        protected virtual float AsFloat() => throw new InvalidCastException();
        protected virtual string AsString() => throw new InvalidCastException();
        protected virtual bool AsBool() => throw new InvalidCastException();
        protected virtual List<Value> AsArray() => throw new InvalidCastException();
        
    }

    public enum ValueType
    {
        String,
        Integer,
        Floating,
        Boolean,
        Array,
        Variable
    }
}
