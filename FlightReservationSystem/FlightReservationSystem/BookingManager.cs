using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservationSystem
{
    public class BookingManager
    {
        private int rows;
        private int adgSeatsCount;
        private int middleSeatsCount;

        Dictionary<int, ReservationRowStatus> overallBookingStatus;
        Dictionary<string, int> availabilityXList; //adg seats
        Dictionary<string, int> availabilityYList; //middle seats
        Dictionary<int, int> availabilityZList; //available single seats

        public BookingManager()
        {
            overallBookingStatus = new Dictionary<int, ReservationRowStatus>();
            availabilityXList = new Dictionary<string, int>();
            availabilityYList = new Dictionary<string, int>();
            availabilityZList = new Dictionary<int, int>();
        }

        public void InitializeList(int rows, int adgSeatsCount, int middleSeatsCount)
        {
            this.rows = rows;
            this.adgSeatsCount = adgSeatsCount;
            this.middleSeatsCount = middleSeatsCount;

            for (int i = 1; i <= rows; i++)
            {
                var rowStatus = new ReservationRowStatus(i, adgSeatsCount, middleSeatsCount);
                rowStatus.Initialize();
                this.overallBookingStatus.Add(i, rowStatus);

                availabilityXList.Add($"{i},1", adgSeatsCount); //adg1
                availabilityYList.Add($"{i},2", middleSeatsCount); //middle
                availabilityXList.Add($"{i},3", adgSeatsCount); //adg2
            }
        }

        public void ListOverallBookingStatus()
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
                            for (int i = 0; i < seatCount; i++)
                                rowItem.Adgecent2[i] = 1;
                        }
                        else
                        {
                            for (int i = 0; i < seatCount; i++)
                                rowItem.Adgecent1[i] = 1;
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
                            for (int i = 0; i < seatCount; i++)
                                rowItem.Middle[i] = 1;

                            remaining -= seatCount;

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
                            rowItem.Middle[middleSeatsCount - 1] = 1;

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
