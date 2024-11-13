using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Classes
{
    public class Hotel
    {
        public List<Room> Rooms { get; set; }
        public List<Guest> Guests { get; set; }
        private string filePath = "HotelData.json";

        public Hotel()
        {
            Rooms = datajson.LoadRooms(filePath);
        }

        public void SaveRooms()
        {
            datajson.SaveRooms(filePath, Rooms);
        }

        public void CheckIn()
        {
            Guest guest = AddGuest();

            var roomNumber = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange rumsnummer du vill boka:"));

            var room = Rooms.FirstOrDefault(room => room.RoomNumber == roomNumber);

            if (room == null) 
            { 
                AnsiConsole.MarkupLine("[bold red]Det finns inget rum med det rumsnummer[/]"); 
            }
            else if (room.isRoomTaken) 
            { 
                AnsiConsole.MarkupLine("[bold red]Rummet är redan bokat[/]"); 
            }
            else
            {
                room.isRoomTaken = true;
                room.CurrentGuest = guest;
                AnsiConsole.MarkupLine("[bold green]Rummet är ledigt och bokas nu![/]");
            }

        }

        private static Guest AddGuest()
        {
            var firstName = AnsiConsole.Prompt(
                            new TextPrompt<string>("Vad är ditt förnamn?"));
            var lastName = AnsiConsole.Prompt(
                new TextPrompt<string>("Vad är ditt efternamn?"));
            var phoneNumber = AnsiConsole.Prompt(
                new TextPrompt<int>("Vad är ditt telefonnummer?"));
            Guest guest = new Guest(firstName, lastName, phoneNumber);
            return guest;
        }

        public void ShowRooms()
        {
            var table = new Table()
                .BorderColor(Color.Silver)
                .Border(TableBorder.Rounded);

            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    table.AddColumn(new TableColumn("Rum nr"));
                    table.AddColumn(new TableColumn("Typ av rum"));
                    table.AddColumn(new TableColumn("Kostnad per natt"));
                    table.AddColumn(new TableColumn("Ledigt"));
                    table.AddColumn(new TableColumn("Gäst"));

                    foreach (var room in Rooms)
                    {
                        var isRoomTakenColor = room.isRoomTaken == false ? Color.Green : Color.Red;

                        table.AddRow(
                            new Markup($"[bold yellow]{room.RoomNumber}[/]"),
                            new Markup($"[bold yellow]{room.TypeOfRoom}[/]"),
                            new Markup($"[bold yellow]{room.PricePerNight}[/]"),
                            new Markup($"[bold {isRoomTakenColor}]{(room.isRoomTaken ? "Nej" : "Ja")}[/]"),
                            room.isRoomTaken ? new Markup($"[bold yellow]{room.CurrentGuest?.Name}[/]") : new Markup("[italic grey]Ingen gäst[/]") // Visa gästens namn eller "Inget gäst" om rummet inte är bokat
                        );
                    }
                });
        }
    }
}
