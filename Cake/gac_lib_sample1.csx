//Skrypt ładuje bibliotekę Newtonsoft.Json z GAC,
//wykonuje zapytanie przez sieć oraz wypisuje na konsolę wynik deserializacji rezultatu zapytania.

#r "Newtonsoft.Json.dll"

using Newtonsoft.Json;
using System.Net;

//zadanie pobiera obiekt przez sieć, deserializuje go i zwraca
var deserialized = new Job("Download").Does(() =>
{
    string baseAddress = "https://jsonplaceholder.typicode.com/";
    string method = "posts/1";

	Model result = new Model();
    using (var client = new WebClient())
    {
        client.BaseAddress = baseAddress;

        Logger.Log(LogLevel.Info, $"Executing GET {baseAddress}{method}");
        string response = client.DownloadString(method);
        
        Logger.Log(LogLevel.Info, $"Response:\n{response}");

        try
        {
            result = JsonConvert.DeserializeObject<Model>(response);
        }
        catch
        {
            Logger.Log(LogLevel.Error, "Couldn't deserialize response.");
        }
		
		return result;
    }
});

//zadanie wypisuje na konsolę wynik deserializacji z zadania "Download"
new VoidJob("Write").DependsOn("Download").Does(() =>
{
	var result = deserialized?.Result?.ResultObject;
	Logger.Log(LogLevel.Info, $"Deserialized response:\nUserId: {result.UserId}\nId: {result.Id}\nTitle: {result.Title}\nBody:\n{result.Body}");
});

//ustawienie zadania - punktu wejściowego w skrypcie
JobManager.SetDefault("Write");

//klasa używana do deserializacji odpowiedzi z sieci w zadaniu "Download"
class Model
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
	
	public Model()
	{
	
	}
}