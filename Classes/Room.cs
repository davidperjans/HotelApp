using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Classes
{
    public class Room
    {
        public int RoomNumber { get; set; }
        public string TypeOfRoom { get; set; }
        public bool isRoomTaken { get; set; }
        public int PricePerNight { get; set; }
        public Guest? CurrentGuest { get; set; }

        public Room(int roomNumber, string typeOfRoom, int pricePerNight)
        {
            RoomNumber = roomNumber;
            TypeOfRoom = typeOfRoom;
            PricePerNight = pricePerNight;
            isRoomTaken = false;
            CurrentGuest = null;
        }
    }
}
