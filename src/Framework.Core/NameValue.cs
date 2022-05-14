using System;

namespace Framework.Core
{
    [Serializable]
    public class NameValue : NameValue<string>
    {
        public NameValue()
        {

        }

        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    [Serializable]
    public class NameValue<T>
    {
        public string Name { get; set; }

        public T Value { get; set; }

        public NameValue()
        {

        }

        public NameValue(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}
