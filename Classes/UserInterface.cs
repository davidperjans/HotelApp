using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Figgle;

namespace HotelApp.Classes
{
    public class UserInterface
    {
        private Hotel hotel;

        public bool isAdmin = false;

        public UserInterface(Hotel hotel)
        {
            this.hotel = hotel;
        }

        public void RunAppAsGuestOrAdmin()
        {
            while (true)
            {
                var options = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Navigera genom menyn")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
                        "Logga in som gäst", "Logga in som admin", "Avsluta program",
                    }));

                switch(options)
                {
                    case "Logga in som gäst":
                        isAdmin = false;
                        RunApplication();
                        break;
                    case "Logga in som admin":
                        isAdmin = true;
                        RunApplication();
                        break;
                    case "Avsluta program":
                        AnsiConsole.MarkupLine("[bold green]Sparar och avslutar program...[/]");
                        return;
                }
            }
        }

        public void RunApplication()
        {
            while (true)
            {
                //Skapa lista med alternativ för selectionprompten
                var choices = new List<string> {
                    "Checka in gäst",
                    "Checka ut gäst",
                    "Visa rum status",
                    "Visa fakturor",
                    "Avsluta & Spara"
                };

                // Om användaren är admin, lägg till alternativet "Lägg till nytt rum"
                if (isAdmin)
                {
                    choices.Remove("Visa rum status");
                    choices.Remove("Visa fakturor");

                    choices.Add("Hantera rum");
                    choices.Add("Hantera fakturor");
                }

                // Ta bort "Avsluta & Spara" om det finns för att sätta det sist
                choices.Remove("Avsluta & Spara");

                // Lägg till "Avsluta & Spara" sist i listan
                choices.Add("Avsluta & Spara");

                // Skapa SelectionPrompt och lägg till alternativen
                var options = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Navigera genom menyn")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(choices)
                );

                switch (options)
                {
                    case "Checka in gäst":
                        hotel.CheckIn();
                        break;
                    case "Checka ut gäst":
                        hotel.CheckOut();
                        break;
                    case "Visa rum status":
                        hotel.ShowRooms();
                        break;
                    case "Visa fakturor":
                        hotel.ShowInvoices();
                        break;
                    case "Hantera rum":
                        ManageRooms();
                        break;
                    case "Hantera fakturor":
                        ManageInvoices();
                        break;
                    case "Avsluta & Spara":
                        hotel.SaveAllData();
                        AnsiConsole.MarkupLine("[bold green]Sparar och avslutar program...[/]");
                        return;
                    default:
                        AnsiConsole.MarkupLine("[bold red]Ogiltligt val, försök igen[/]");
                        break;
                }

            }
        }

        // Metod för att hantera rum (endast för admin)
        private void ManageRooms()
        {
            if (!isAdmin) return;

            var roomOptions = new List<string> {
                "Lägg till nytt rum",
                "Ta bort ett rum",
                "Visa rum status",
                "Gå tillbaka"
            };

            var roomChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hantera rum")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(roomOptions)
            );

            switch (roomChoice)
            {
                case "Lägg till nytt rum":
                    hotel.AddRoom();
                    break;
                case "Ta bort ett rum":
                    hotel.RemoveRoom();
                    break;
                case "Visa rum status":
                    hotel.ShowRooms();
                    break;
                case "Gå tillbaka":
                    return;
                default:
                    AnsiConsole.MarkupLine("[bold red]Ogiltligt val, försök igen[/]");
                    break;
            }
        }

        // Metod för att hantera fakturor (endast för admin)
        private void ManageInvoices()
        {
            if (!isAdmin) return;

            var invoiceOptions = new List<string> {
            "Visa fakturor",
            "Ta bort en faktura",
            "Gå tillbaka"
        };

            var invoiceChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hantera fakturor")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(invoiceOptions)
            );

            switch (invoiceChoice)
            {
                case "Visa fakturor":
                    hotel.ShowInvoices();
                    break;
                case "Ta bort en faktura":
                    hotel.RemoveInvoice();
                    break;
                case "Gå tillbaka":
                    return;
                default:
                    AnsiConsole.MarkupLine("[bold red]Ogiltligt val, försök igen[/]");
                    break;
            }
        }
    }
}
