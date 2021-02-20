using System;
using System.Collections.Generic;

namespace FlightReservationSystem
{
    public class Program
    {
        // due to time constraints :)

        public Dictionary<int, ReservationRowStatus> overallBookingStatus = new Dictionary<int, ReservationRowStatus>();
        public Dictionary<string, int> availabilityXList = new Dictionary<string, int>(); //adg seats
        public Dictionary<string, int> availabilityYList = new Dictionary<string, int>(); //middle seats
        public Dictionary<int, int> availabilityZList = new Dictionary<int, int>(); //available single seats

        const int totalRows = 3;
        const int adgSeatsCount = 2;
        const int middleSeatsCount = 3;

        static void Main(string[] args)
        {
            var instance = new Program();
            instance.InitializeList(totalRows, adgSeatsCount, middleSeatsCount);
            instance.ListOverallBookingStatus();
            instance.ShowAvailabilityStatusX();

            Console.WriteLine("Booking 2 seats");

            // book all from 2 seats bucket
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);

            // book from 3 seats bucket
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);

            // book from remaining single seats in 3 seats bucket
            instance.BookSeat(2);

            instance.ListOverallBookingStatus();
            instance.ShowAvailabilityStatusX();

            Console.ReadLine();
        }

        public void InitializeList(int rows, int adgSeatsCount, int middleSeatsCount)
        {
            for (int i = 1; i <= rows; i++)
            {
                var rowStatus = new ReservationRowStatus(i, adgSeatsCount, middleSeatsCount);
                rowStatus.Initialize();
                this.overallBookingStatus.Add(i, rowStatus);

                availabilityXList.Add($"{i},1", 2); //adg1
                availabilityYList.Add($"{i},2", 3); //middle
                availabilityXList.Add($"{i},3", 2); //adg2
            }
        }

        void ListOverallBookingStatus()
        {
            Console.WriteLine("Overall Booking Status: 0 -> Available; 1 -> Booked");

            foreach (var item in this.overallBookingStatus)
            {
                Console.WriteLine($"Row: {item.Key}");

                var reservationRowStatus = item.Value;

                foreach (var adg in reservationRowStatus.Adgecent1)
                {
                    Console.WriteLine($"Adgecent1 - Cell: {adg}");
                }

                foreach (var adg in reservationRowStatus.Middle)
                {
                    Console.WriteLine($"Middle - Cell: {adg}");
                }

                foreach (var adg in reservationRowStatus.Adgecent2)
                {
                    Console.WriteLine($"Adgecent2 - Cell: {adg}");
                }
            }
        }

        public void ShowAvailabilityStatusX()
        {
            Console.WriteLine("");
            Console.WriteLine("*************Available seats************* ");

            Console.WriteLine("Adj seats: ");
            foreach (var item in availabilityXList)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

            Console.WriteLine("Middle seats: ");
            foreach (var item in availabilityYList)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

            Console.WriteLine("Single seats: ");
            foreach (var item in availabilityZList)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
        }

        public void BookSeat(int seatCount)
        {
            if (seatCount == adgSeatsCount)
            {
                string keyToRemoveX = string.Empty;

                foreach (var item in availabilityXList)
                {
                    if (item.Value == seatCount)
                    {
                        var key = item.Key.Split(',');
                        int row = int.Parse(key[0]);
                        int cell = int.Parse(key[1]);

                        // mark as booked
                        var rowItem = overallBookingStatus[row];
                        if (cell == 3)
                        {
                            rowItem.Adgecent2[0] = 1;
                            rowItem.Adgecent2[1] = 1;
                        }
                        else
                        {
                            rowItem.Adgecent1[0] = 1;
                            rowItem.Adgecent1[1] = 1;
                        }

                        keyToRemoveX = item.Key;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(keyToRemoveX))
                    availabilityXList.Remove(keyToRemoveX);
                else
                {
                    string keyToRemoveY = string.Empty;
                    int remaining = middleSeatsCount;

                    foreach (var item in availabilityYList)
                    {
                        if (item.Value >= seatCount)
                        {
                            var key = item.Key.Split(',');
                            int row = int.Parse(key[0]);

                            // mark as booked
                            var rowItem = overallBookingStatus[row];
                            rowItem.Middle[0] = 1;
                            rowItem.Middle[1] = 1;

                            remaining -= 2;

                            keyToRemoveY = item.Key;
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(keyToRemoveY))
                    {
                        availabilityYList.Remove(keyToRemoveY);

                        var key = keyToRemoveY.Split(',');
                        int row = int.Parse(key[0]);

                        availabilityZList.Add(row, 1);
                    }
                    else
                    {
                        List<int> keysToRemoveZ = new List<int>();
                        int allocatedSeats = 0;

                        foreach (var item in availabilityZList)
                        {
                            allocatedSeats++;
                            int row = item.Key;
                            keysToRemoveZ.Add(row);
                            var rowItem = overallBookingStatus[row];
                            rowItem.Middle[2] = 1;

                            if (allocatedSeats == seatCount)
                            {
                                break;
                            }
                        }

                        foreach (var keyToRemoveZ in keysToRemoveZ)
                        {
                            availabilityZList.Remove(keyToRemoveZ);
                        }
                    }

                }
            }
        }
    }
}
