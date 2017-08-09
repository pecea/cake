namespace Cake
{
    internal class Argument
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

        public string[] Names { get; }
        public string Value { get; }
        public bool HasValue { get; }
    }
}