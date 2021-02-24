using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace FlightReservationSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();

            var section = Configuration.GetSection("appSettings");
            var rows = int.Parse(section["numberOfRows"]);
            var adgSeats = int.Parse(section["adjacentSeatsCount"]);
            var middleSeats = int.Parse(section["middlesSeatsCount"]);

            var bookingManager = new BookingManager();
            bookingManager.InitializeList(rows, adgSeats, middleSeats);
            bookingManager.ListOverallBookingStatus();
            bookingManager.ShowAvailabilityStatusX();

            while (true)
            {
                Console.WriteLine("Enter the no of seats to book:");
                int seatsToBook = int.Parse(Console.ReadLine());

                if (seatsToBook == 0)
                    break;

                Console.WriteLine($"Booking {seatsToBook} seats");

                bookingManager.BookSeat(seatsToBook);

                bookingManager.ListOverallBookingStatus();
                bookingManager.ShowAvailabilityStatusX();
            }
        }
    }
}
