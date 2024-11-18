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

        public UserInterface(Hotel hotel)
        {
            this.hotel = hotel;
        }

        public void RunApplication()
        {
            while (true)
            {
                var options = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Navigera genom menyn")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
                        "Checka in gäst", "Checka ut gäst", "Visa rum status", "Visa fakturor",
                        "Avsluta & Spara",
                    }));

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
    }
}
