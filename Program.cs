using System.Net.Http.Json;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Lab9_Misyuro.Kirill_Serialization;

class Program
{
    static void Main(string[] args)
    {
        string path = "../../../files/";
        string jsonFilePath;

        Squad squadObject = new Squad("Name", "black");

        JsonSerializer serializer = new JsonSerializer();
        using (StreamWriter sw = new StreamWriter(path + "file.json"))
        {
            serializer.Serialize(sw, squadObject);
        }

        try
        {
            jsonFilePath = FindJsonDocument(path);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        Squad? deserializeSquadObject = JsonConvert.DeserializeObject<Squad>(ReadJsonDocument(jsonFilePath));

        var bf = new BinaryFormatter();
        using (Stream stream = new FileStream(path + "squad.bin", FileMode.Create, FileAccess.Write, FileShare.None))
        {
#pragma warning disable SYSLIB0011
            bf.Serialize(stream, deserializeSquadObject);
#pragma warning restore SYSLIB0011
        }
    }

    static string FindJsonDocument(string path)
    {
        string[] filesInDirectory = Directory.GetFiles(path);
        List<string> jsonDocuments = new List<string>();
        foreach (var file in filesInDirectory)
        {
            if (file.EndsWith(".json"))
            {
                jsonDocuments.Add(file);
            }
        }

        if (jsonDocuments.Count != 1)
        {
            throw new Exception("No json-files in folder, or too many json-files ");
        }

        return jsonDocuments[0];
    }

    static string ReadJsonDocument(string path)
    {
        string output;
        using (StreamReader sr = new StreamReader(path))
        {
            output = sr.ReadToEnd();
        }

        return output;
    }
}
[Serializable]
class Squad
{
    public string name;
    public string color;

    public Squad(string name, string color)
    {
        this.name = name;
        this.color = color;
    }
}