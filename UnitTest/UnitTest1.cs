using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finanse;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        MainWindow mainWindow = new MainWindow("tfus", 2);
        [TestMethod]
        public void LokataTest()
        {
            // arrange
            
            double amount = 1000;
            int time = 5;
            double rateOFInterest = 2;
            double expected = 1006.75;

            // act
            double actual=mainWindow.Investment(amount, time, rateOFInterest);

            //Assert
            Assert.AreEqual(expected, actual);
            
        }

        [TestMethod]
        public void BilansTest()
        {
            //arrange
            double expected = -351;

            List<Transaction> list = new List<Transaction>()
            {
                new Transaction(){ID_Transaction=1, UserID=1, Name="zakupy", Amount=100, Date=new DateTime(2020,06,05), Type="wydatek"},
                new Transaction(){ID_Transaction=2, UserID=1, Name="zakupy", Amount=451, Date=new DateTime(2020,06,08), Type="wydatek"},
                new Transaction(){ID_Transaction=3, UserID=1, Name="sprzedaz", Amount=200, Date=new DateTime(2020,06,09), Type="przychod"}

            };

            //act
            double actual= mainWindow.Balance(list);

            //Assert
            Assert.AreEqual(expected, actual);

        }

    }
}
