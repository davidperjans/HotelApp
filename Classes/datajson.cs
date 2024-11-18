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
        public static HotelData LoadData(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<HotelData>(jsonData) ?? new HotelData();
            }
            else
            {
                Console.WriteLine("File not found. Returning default HotelData.");
                return new HotelData();
            }
        }

        // Save data to JSON file
        public static void SaveData(string filePath, HotelData data)
        {
            string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }
    }

}
