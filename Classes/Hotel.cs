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
        public List<Invoice> Invoices { get; set; }
        private HotelData hotelData;
        private string filePath = "HotelData.json";

        public string TypeOfRoomString { get; set; }

        public Hotel()
        {
            hotelData = datajson.LoadData(filePath);
            Rooms = hotelData.Rooms;
            Invoices = hotelData.Invoices;

        }

        public void SaveAllData()
        {
            hotelData.Rooms = Rooms;
            hotelData.Invoices = Invoices;
            datajson.SaveData(filePath, hotelData);
        }

        public void CheckIn()
        {
            Guest guest = AddGuest();

            var roomNumber = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange rumsnummer du vill boka: "));

            var room = Rooms.FirstOrDefault(room => room.RoomNumber == roomNumber);

            if (room == null) 
            { 
                AnsiConsole.MarkupLine("[bold red]Det finns inget rum med det nummer.[/]"); 
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

        public void CheckOut()
        {
            var roomNumber = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange rumsnummer du vill checka ut: "));

            var room = Rooms.FirstOrDefault(room => room.RoomNumber == roomNumber);

            if (room == null)
            {
                AnsiConsole.MarkupLine("[bold red]Det finns inget rum det nummer.[/]");
            }
            else if (!room.isRoomTaken)
            {
                AnsiConsole.MarkupLine("[bold red]Rummet är inte bokat[/]");
            }
            else
            {

                DateTime incheckning = DateTime.Now.AddDays(-3); // Exempel på incheckningstid
                DateTime utcheckning = DateTime.Now;

                var kostnad = (utcheckning - incheckning).Days * room.PricePerNight;

                Invoice faktura = new Invoice
                {
                    FakturaId = Invoices.Count + 1,
                    GästNamn = room.CurrentGuest.Name,
                    RumsNummer = room.RoomNumber,
                    Incheckning = incheckning,
                    Utcheckning = utcheckning,
                    TotalKostnad = kostnad
                };

                Invoices.Add(faktura);

                room.isRoomTaken = false;
                room.CurrentGuest = null;
                AnsiConsole.MarkupLine("[bold green]Du är nu utcheckad och rummet är ledigt för att bokas på nytt![/]");
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

        public void ShowInvoices()
        {
            if (Invoices.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold red]Inga fakturor hittades.[/]");
                return;
            }

            var table = new Table()
                .BorderColor(Color.Silver)
                .Border(TableBorder.Rounded);

            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    table.AddColumn(new TableColumn("Faktura ID"));
                    table.AddColumn(new TableColumn("Gäst"));
                    table.AddColumn(new TableColumn("Rusmnummer"));
                    table.AddColumn(new TableColumn("Incheckning"));
                    table.AddColumn(new TableColumn("Utcheckning"));
                    table.AddColumn(new TableColumn("Total kostnad"));

                    foreach (var invoice in Invoices)
                    {
                        table.AddRow(
                            new Markup($"[bold yellow]{invoice.FakturaId.ToString()}[/]"),
                            new Markup($"[bold yellow]{invoice.GästNamn}[/]"),
                            new Markup($"[bold yellow]{invoice.RumsNummer.ToString()}[/]"),
                            new Markup($"[bold yellow]{invoice.Incheckning.ToString("yyyy-MM-dd")}[/]"),
                            new Markup($"[bold yellow]{invoice.Utcheckning.ToString("yyyy-MM-dd")}[/]"),
                            new Markup($"[bold yellow]{invoice.TotalKostnad.ToString("C")}[/]")
                        );
                    }
                });

        }

        public void RemoveInvoice()
        {
            if (Invoices.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold red]Det finns inga fakturor[/]");
            }
            else
            {
                var invoiceToRemove = AnsiConsole.Prompt(
                            new TextPrompt<int>("Ange ID på fakturan du vill ta bort"));

                var doesInvoiceExist = Invoices.FirstOrDefault(invoice => invoice.FakturaId == invoiceToRemove);

                if (doesInvoiceExist != null)
                {
                    Invoices.Remove(doesInvoiceExist);
                    AnsiConsole.MarkupLine("[bold yellow]Fakturan är nu borttagen![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[bold red]Det finns inga faktura med ID {invoiceToRemove}[/]");
                }
            }
        }

        public void RemoveRoom()
        {
            var roomNumber = AnsiConsole.Prompt(
                            new TextPrompt<int>("Vad är det för rumsnummer du vill ta bort?"));

            var doesRoomExist = Rooms.FirstOrDefault(room => room.RoomNumber == roomNumber);

            if (doesRoomExist != null)
            {
                if(doesRoomExist.CurrentGuest == null)
                {
                    Rooms.Remove(doesRoomExist);
                    AnsiConsole.MarkupLine("[bold yellow]Rummet är nu borttaget![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Rummet är nu upptaget, checka ut gäst först![/]");
                }
                
            }
        }

        public void AddRoom()
        {
            var roomNumber = AnsiConsole.Prompt(
                            new TextPrompt<int>("Vad är det för rumsnummer?"));


            var typeOfRoomPrompt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj vilken typ rummet är")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
                        "Enkelrum", "Dubbelrum", "Svit",
                    }));

            switch (typeOfRoomPrompt)
            {
                case "Enkelrum":
                    TypeOfRoomString = "Enkelrum";
                    break;
                case "Dubbelrum":
                    TypeOfRoomString = "Dubbelrum";
                    break;
                case "Svit":
                    TypeOfRoomString = "Svit";
                    break;

            }

            var pricePerNight = AnsiConsole.Prompt(
                            new TextPrompt<int>("Vad är kostnaden per natt?"));

            Room newRoom = new Room(roomNumber, TypeOfRoomString, pricePerNight);
            Rooms.Add(newRoom);
            AnsiConsole.MarkupLine("[bold green]Rummet har lagts till[/]");
        }
        public void ShowRooms()
        {
            var sortedRooms = Rooms.OrderBy(room => room.RoomNumber).ToList();

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

                    foreach (var room in sortedRooms)
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
