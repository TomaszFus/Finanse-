using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finanse;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // arrange
            MainWindow mainWindow = new MainWindow("tfus", 2);
            double amount = 1000;
            int time = 5;
            double rateOFInterest = 2;
            double expected = 1006.75;

            // act
            double actual=mainWindow.Investment(amount, time, rateOFInterest);

            //Assert
            Assert.AreEqual(expected, actual);
            
        }
    }
}
