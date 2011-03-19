using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;
using System.Reflection;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}

        [Test()]
        public void TestThatCarGetsLocation()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();

            String location1 = "here";
            String location2 = "OVER NINE THOUSAND!";

            using (mocks.Record())
            {
                mockDatabase.getCarLocation(2);
                LastCall.Return(location1);
                mockDatabase.getCarLocation(9001);
                LastCall.Return(location2);
            }

            var target = new Car(10);
            target.Database = mockDatabase;

            String result;
            result = target.getCarLocation(2);
            Assert.AreEqual(result, location1);
            result = target.getCarLocation(9001);
            Assert.AreEqual(result, location2);
        }

        [Test()]
        public void TestThatCarDoesGetMileageFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int miles = 500;

            mockDatabase.Miles = miles;

            var target = new Car(10);
            target.Database = mockDatabase;

            int result = target.Mileage;
            Assert.AreEqual(result, miles);
        }

        [Test()]
        public void TestThatBMWHasCorrectRental()
        {
            Car bmw = ObjectMother.BMW();
            Assert.AreEqual(80, bmw.getBasePrice());
        }
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}
	}
}
