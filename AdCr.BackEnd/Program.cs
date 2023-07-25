using AdCr.BackEnd;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

class Program
{
     
    public delegate void ProgressEventHandler(int currentProgress, int total);

    // Event to report the progress of the download
    public static event ProgressEventHandler UpdateProcessHandler;

    

    static void DownloadImage(string url, string folder, string filename, int imageNumber)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, Path.Combine(folder, filename));
                UpdateProcessHandlerChanged(imageNumber, totalImages); // Report progress
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while downloading {filename}: {e.Message}");
        }
    }

    static void OrganizeImages(int count, int parallelism,int width = 200, int height = 300 )
    {
        string url = $"https://picsum.photos/{width}/{height}";  // Değiştirilecek, indirilecek resim URL'si
        totalImages = count;

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        List<Task> tasks = new List<Task>();

        for (int i = 1; i <= count; i++)
        {
            string filename = $"{i}.png";  // Resimleri sırayla numaralandırıyoruz
            Task task = Task.Run(() => DownloadImage(url, folder, filename, i));
            tasks.Add(task);

            if (tasks.Count % parallelism == 0 || i == count)
            {
                Task.WhenAll(tasks).Wait();
                tasks.Clear();
            }
        }

        Console.WriteLine("All images downloaded and organized successfully.");
    }
    static void UpdateProcessHandlerChanged(int currentProgress, int total)
    {
        UpdateProcessHandler?.Invoke(currentProgress, total);
    }

    static int totalImages;
    static string folder;
    static void Main(string[] args)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;

        string filePath = $@"{basePath}\RequestFolder\Input.json";

        string fileContent = File.ReadAllText(filePath);

        var request = JsonConvert.DeserializeObject<JsonRequest>(fileContent);
        folder = request.SavePath;
        
        UpdateProcessHandler += (currentProgress, total) =>
        {
            Console.WriteLine($"Downloading {currentProgress} out of {total} images.");
        };

        OrganizeImages(request.Count, request.Parallelism);
    }
}
