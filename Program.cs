using HotelApp.Classes;

namespace HotelApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hotel hotel = new Hotel();
            UserInterface ui = new UserInterface(hotel);

            ui.RunAppAsGuestOrAdmin();
        }
    }
}
