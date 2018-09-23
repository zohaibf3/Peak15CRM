using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using NUnit.Framework;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AventStack.ExtentReports;

namespace Test_MSDynamics.DataHelper
{
    public static class DataInput
    {
        // ----------------------------------------------For New Departure Creation Variables---------------------------------------------//

        public static int childNum = 2;
        public static Dictionary<string, string> guestType = new Dictionary<string, string>();
        public static string firstChildf = "1";
        public static string firstChildt = "3";
        public static string secChildf = "4";
        public static string secChildt = "7";
        public static string thirChildf = "9";
        public static string thirChildt = "12";
        //---------------------------------------------------------------------------------------------------------------------------------//

        // -------------------------------------------------------For Book Hotel Variables-------------------------------------------------//

        public static Dictionary<string, string> hotelDictionary = new Dictionary<string, string>();
        public static Dictionary<string, int> roomNumDictionary = new Dictionary<string, int>();
        public static Dictionary<string, int> adultCountDictionary = new Dictionary<string, int>();
        public static Dictionary<string, int> childCountDictionary = new Dictionary<string, int>();

        public static List<int> childAge = new List<int>();        //{ 1, 1 ,2, 3, 1, 4, 4, 4, 4, 5  };
        public static Random rnd = new Random(); //Generate Random number;
        public static List<string> cardInfo = new List<string>();  //{ "MC", "5442981111111114", "12", "19", "079" };
        public static List<string> bookInfo = new List<string>();  //{ "First Name","Last Name","City","US","AK","16045027384","test@test.com"};

        //Excel File Path
        private static string filePath = @"C:\Users\ZOHAIB FARAN\source\repos\Test MSDynamics\Test MSDynamics\Excel File\BookHotel.xlsx";


        //---------------------------------------------------------------------------------------------------------------------------------//

        // ------------------------------------------For Old Travel Reservations Variables-------------------------------------------------//
        public static string rcTRValue = "123123TestScript";
        public static List<string> travelGuest = new List<string>() { "Adam Jones", "Brad Crume", "Abe Simpson" };
        public static List<List<string>> travelSegment = new List<List<string>>();

        //---------------------------------------------------------------------------------------------------------------------------------//
        public static void initializeDeparture()
        {
            guestType.Add("Adults", "0");
            guestType.Add("Adults & Children", "1");
        }
        public static void initializeHotel()
        {
            //Hotel Information Adding
            DataTable hotelInfo = ExcelDataReader.getDataFromSheet("hotelInfo", filePath);
            for (int i = 0; i < hotelInfo.Rows.Count; i++)
            {
                
                hotelDictionary.Add("HotelName", Convert.ToString(hotelInfo.Rows[i]["HotelName"]));
                hotelDictionary.Add("FromDate", Convert.ToString(hotelInfo.Rows[i]["FromDate"]));
                hotelDictionary.Add("ToDate", Convert.ToString(hotelInfo.Rows[i]["ToDate"]));
            }

            //Room per Adult and Child combination information added
            DataTable roomAdultChild = ExcelDataReader.getDataFromSheet("roomAdultChild", filePath);
            for (int i = 0; i < roomAdultChild.Rows.Count; i++)
            {
                roomNumDictionary.Add(Convert.ToString("RoomCount" + (i + 1)), Convert.ToInt32(roomAdultChild.Rows[i]["RoomCount"]));
                adultCountDictionary.Add(Convert.ToString("AdultCount" + (i + 1)), Convert.ToInt32(roomAdultChild.Rows[i]["AdultCount"]));
                childCountDictionary.Add(Convert.ToString("ChildCount" + (i + 1)), Convert.ToInt32(roomAdultChild.Rows[i]["ChildCount"]));
            }

            //Children Ages information added
            DataTable childAgeInfo = ExcelDataReader.getDataFromSheet("childAge", filePath);
            for (int i = 0; i < childAgeInfo.Rows.Count; i++)
            {
                childAge.Add(Convert.ToInt32(childAgeInfo.Rows[i]["ChildrenAge"]));
            }

            //Credit Card Information to be Added
            DataTable creditCardInfo = ExcelDataReader.getDataFromSheet("cardInformation", filePath);
            for (int i = 0; i < creditCardInfo.Rows.Count; i++)
            {
                cardInfo.Add(Convert.ToString(creditCardInfo.Rows[i]["CardType"]));
                cardInfo.Add(Convert.ToString(creditCardInfo.Rows[i]["CardHolderName"]));
                cardInfo.Add(Convert.ToString(creditCardInfo.Rows[i]["CardNumber"]));
                cardInfo.Add(Convert.ToString(creditCardInfo.Rows[i]["ExpiryMonth"]));
                cardInfo.Add(Convert.ToString(creditCardInfo.Rows[i]["ExpiryYear"]));
                cardInfo.Add(Convert.ToString(creditCardInfo.Rows[i]["SecurityCode"]));
            }

            //Booking Information to be Added
            DataTable bookingInfo = ExcelDataReader.getDataFromSheet("bookingInfo", filePath);
            for (int i = 0; i < creditCardInfo.Rows.Count; i++)
            {
                bookInfo.Add(Convert.ToString(bookingInfo.Rows[i]["FirstName"]));
                bookInfo.Add(Convert.ToString(bookingInfo.Rows[i]["LastName"]));
                bookInfo.Add(Convert.ToString(bookingInfo.Rows[i]["City"]));
                bookInfo.Add(Convert.ToString(bookingInfo.Rows[i]["Country"]));
                bookInfo.Add(Convert.ToString(bookingInfo.Rows[i]["State"]));
                bookInfo.Add(Convert.ToString(bookingInfo.Rows[i]["PhoneNumber"]));
                bookInfo.Add(Convert.ToString(bookingInfo.Rows[i]["Email"]));
            }
        }

        public static void initializeOldTravelReservation()
        {
            travelSegment.Add(new List<string> { "SUI", "0", "142", "4/28/2018", "NYC", "1:30 AM", "4/30/2018", "DTR", "1:59 PM" });
            travelSegment.Add(new List<string> { "SUI", "0", "142", "4/28/2018", "DTR", "1:30 AM", "4/30/2018", "WDC", "1:59 PM" });
            travelSegment.Add(new List<string> { "SUI", "0", "142", "4/28/2018", "WDC", "1:30 AM", "4/30/2018", "CGO", "1:59 PM" });
        }
    }
}
