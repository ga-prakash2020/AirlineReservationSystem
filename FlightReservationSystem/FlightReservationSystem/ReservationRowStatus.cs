namespace FlightReservationSystem
{
    public class ReservationRowStatus
    {
        readonly int adgSeats = 0;
        readonly int middleSeats = 0;
        readonly int row = 0;

        public ReservationRowStatus(int row, int adgSeats, int middleSeats)
        {
            this.adgSeats = adgSeats;
            this.middleSeats = middleSeats;
            this.row = row;
        }

        public int Row { get; set; }

        public int[] Adgecent1 { get; set; }
        public int[] Middle { get; set; }
        public int[] Adgecent2 { get; set; }

        public void Initialize()
        {
            Row = row;
            Adgecent1 = new int[adgSeats];
            Adgecent2 = new int[adgSeats];
            Middle = new int[middleSeats];
        }
    }
}
