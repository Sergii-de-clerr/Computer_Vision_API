using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Computer_Vision_API
{
    class Program
    {
        private static string subscriptionKey = "e55411ec4e3140a78f1f47b55c6517d0";
        private static string endpoint = "https://sergii-nazarchuk-computer-vision-api1.cognitiveservices.azure.com/";

        static void Main(string[] args)
        {
            var client = Authenticate(endpoint, subscriptionKey);

            //Description
            //var imageUrl = "https://static.posters.cz/image/750/art-photo/alfred-hitchcock-the-birds-1963-directed-by-alfred-hitchcock-i143902.jpg";
            //var imageUrl = "https://a.travel-assets.com/findyours-php/viewfinder/images/res70/480000/480443-Hollywood.jpg";
            //var imageUrl = "https://images.amcnetworks.com/ifccenter.com/wp-content/uploads/2017/01/kubrick-retro.jpg";
            //var imageUrl = "https://cdn.britannica.com/20/177220-050-05241222/Stanley-Kubrick-filming-Barry-Lyndon.jpg";

            //Objects

            //Text
            //var imageUrl = "https://images.ctfassets.net/8cjpn0bwx327/50s494quPJWJW3OnCyMsGJ/4f18e80f13306f5b438b9e313de0be81/Exploring_Microsoft-s_Computer_Vision_API.jpg";
            //var imageUrl = "https://www.wikihow.com/images/thumb/f/f8/Write-a-Letter-of-Request-Step-12-Version-3.jpg/v4-460px-Write-a-Letter-of-Request-Step-12-Version-3.jpg";
            //var imageUrl = "https://i.pinimg.com/474x/1d/a5/ea/1da5ea66af7acb3f12685871f8828afc.jpg";


            //var imageUrl = "https://cdn.britannica.com/20/177220-050-05241222/Stanley-Kubrick-filming-Barry-Lyndon.jpg";
            var imageUrl = "https://cdn.britannica.com/40/75640-050-F894DD85/tiger-Siberian.jpg";

            ProcessStartInfo startInfo = new ProcessStartInfo(imageUrl)
            {
                UseShellExecute = true
            };
            Process.Start(startInfo);

            AnalyzeImageUrlObjects(client, imageUrl).Wait();

            Console.ReadKey();
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };
            return client;
        }

        public static async Task AnalyzeImageUrlDescription(ComputerVisionClient client, string imageUrl)
        {
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description, VisualFeatureTypes.Objects
            };

            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, features);

            Console.WriteLine("Description of the image:");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            }
        }

        public static async Task AnalyzeImageUrlObjects(ComputerVisionClient client, string imageUrl)
        {
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description, VisualFeatureTypes.Objects
            };

            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, features);

            Console.WriteLine("Detected objects:");
            foreach (var obj in results.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} on a position {obj.Rectangle.X},{obj.Rectangle.Y} with size {obj.Rectangle.W}x{obj.Rectangle.H}");
            }
        }

        public static async Task ReadTextFromImage(ComputerVisionClient client, string imageUrl)
        {
            OcrResult results = await client.RecognizePrintedTextAsync(true, imageUrl);

            Console.WriteLine("Recognized text:");
            foreach (var region in results.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        Console.Write($"{word.Text} ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
