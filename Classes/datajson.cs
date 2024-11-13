using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HotelApp.Classes
{
    public class datajson
    {
        // Load data from JSON file
        public static List<Room> LoadRooms(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Room>>(jsonData);
            }
            else
            {
                Console.WriteLine("File not found. Returning an empty list.");
                return new List<Room>();
            }
        }

        // Save data to JSON file
        public static void SaveRooms(string filePath, List<Room> rooms)
        {
            string jsonData = JsonSerializer.Serialize(rooms, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }
    }
}
