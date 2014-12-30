namespace Cake
{
    public class Argument
    {
        public Argument(string[] names, bool hasValue)
        {
            Names = names;
            HasValue = hasValue;
        }

        public Argument(string[] names, string value, bool hasValue)
        {
            Names = names;
            Value = value;
            HasValue = hasValue;
        }

        public string[] Names { get; private set; }
        public string Value { get; private set; }
        public bool HasValue { get; private set; }
    }
}