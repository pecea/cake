public class Result
{
    public string Result1 { get; set; }
    public bool Result2 { get; set; }
}
var first = new Job("1").Does(() => {
    if (second.Result.Success)
        return new Result
        {
            Result1 = "two",
            Result2 = true
        };
    else
        return false;
}).DependsOn("2");

var second = new Job("2").Does(() =>
{
    return new Result
    {
        Result1 = "one",
        Result2 = true
    };
});

var third = new VoidJob("3").DependsOn("1").Does(() => {

    foreach(var property in first.Result.ResultObject.GetType().GetProperties())
    {
        System.Console.WriteLine(property.Name);
        System.Console.WriteLine(property.PropertyType);
    }
});

JobManager.SetDefault("3");