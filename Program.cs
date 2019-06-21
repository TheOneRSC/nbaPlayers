using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NbaStars
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // C:\...\NbaStars\NbaStars\nbaplayers.json
                Console.WriteLine("Enter the path to the input file :");
                string inputFilePath = Console.ReadLine();

                Console.WriteLine("\nEnter the maximum number of years the player has played in the league to qualify :");
                int maxNumberOfYears = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("\nEnter the minimum rating the player should have to qualify:");
                int minimumRating = Convert.ToInt32(Console.ReadLine());

                // C:\...\NbaStars\NbaStars\bin\Debug
                Console.WriteLine("Enter the path for the output file :");
                string outputFilePath = Console.ReadLine();

                var nbaPlayers = ReadJsonFile(inputFilePath);

                nbaPlayers = nbaPlayers
                    .Where(p => p.Rating >= minimumRating &&
                                DateTime.Now.Year - p.PlayingSince <= maxNumberOfYears)
                    .OrderByDescending(p => p.Rating)
                    .ToList();

                ExportPlayersToFile(outputFilePath, nbaPlayers);
                Console.WriteLine("Players exported successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static List<NbaPlayerData> ReadJsonFile(string inputFilePath)
        {
            List<NbaPlayerData> result = null;

            using (StreamReader r = new StreamReader(inputFilePath))
            {
                string json = r.ReadToEnd();
                result = JsonConvert.DeserializeObject<List<NbaPlayerData>>(json);
            }

            return result;
        }

        static void ExportPlayersToFile(string outputFilePath, List<NbaPlayerData> nbaPlayers)
        {
            string outputPath = Path.ChangeExtension(outputFilePath, ".csv");

            using (StreamWriter writeStream = new StreamWriter(outputPath))
            {
                foreach (var player in nbaPlayers)
                {
                    writeStream.WriteLine($"{player.Name}, {player.Rating}");
                }
            }
        }
    }

    public class NbaPlayerData
    {
        public string Name { get; set; }
        public int PlayingSince { get; set; }

        public string Position { get; set; }

        public int Rating { get; set; }
    }
}
