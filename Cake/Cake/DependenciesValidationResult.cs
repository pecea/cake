namespace Cake
{
    internal class DependenciesValidationResult
    {
        public bool IsValid { get; set; }
        public string CycleSourceJobName { get; set; }
    }
}
