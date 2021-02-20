using NUnit.Framework;
using FlightReservationSystem;

namespace UnitTests
{
    public class Tests
    {
        const int totalRows = 3;
        const int adgSeatsCount = 2;
        const int middleSeatsCount = 3;

        Program instance;

        [SetUp]
        public void Setup()
        {
            instance = new Program();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            instance.InitializeList(totalRows, adgSeatsCount, middleSeatsCount);
        }

        [Test]
        public void BooksFromAdjacentSeats()
        {
            var totalAdgacentBuckets = totalRows * 2; // 2 per row

            // ACT
            instance.BookSeat(2);

            var remainingAdjBuckets = totalAdgacentBuckets - 1;
            Assert.AreEqual(remainingAdjBuckets, instance.availabilityXList.Count);
            Assert.AreEqual(totalRows, instance.availabilityYList.Count); //unaffected
            Assert.AreEqual(0, instance.availabilityZList.Count); //unaffected
        }

        [Test]
        public void BooksFromMiddleSeats()
        {
            // book all the adjacent buckets
            // ACT
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);

            //books from middle buckets
            instance.BookSeat(2);

            Assert.AreEqual(0, instance.availabilityXList.Count); // all booked
            Assert.AreEqual(totalRows - 1, instance.availabilityYList.Count); // one used
            Assert.AreEqual(1, instance.availabilityZList.Count); //one added
        }

        [Test]
        public void BooksSingleSeatsFromRemainingMiddleSeats()
        {
            // book all from the adjacent buckets
            // ACT
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);

            //books all from the middle buckets
            instance.BookSeat(2);
            instance.BookSeat(2);
            instance.BookSeat(2);

            //books from single seats
            instance.BookSeat(2);

            Assert.AreEqual(0, instance.availabilityXList.Count); // all booked
            Assert.AreEqual(0, instance.availabilityYList.Count); // all booked
            Assert.AreEqual(1, instance.availabilityZList.Count); //one remains
        }
    }
}