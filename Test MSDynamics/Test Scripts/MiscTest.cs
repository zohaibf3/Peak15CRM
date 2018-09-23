using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System.Threading;
using NUnit.Framework;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using Test_MSDynamics.DataHelper;
using Test_MSDynamics.Common_Methods;
using Test_MSDynamics.PageObjects;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;

namespace Test_MSDynamics.Test_Scripts
{
    [TestFixture]
    public class LoginTest : Environment.EnvironmentConfig
    {

        private ExtentReports extent;
        private ExtentTest test;
        private ExtentHtmlReporter htmlReporter;

        [OneTimeSetUp]
        public void StartReport()
        {
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;

            string reportPath = projectPath + "Reports\\MyOwnReport.html";
            htmlReporter = new ExtentHtmlReporter(reportPath);

            htmlReporter.Configuration().DocumentTitle = "Hotel Creation Report";
            htmlReporter.Configuration().ReportName = "Hotel Booking Report";
            htmlReporter.Configuration().Theme = Theme.Standard;

            //extent.AddSystemInfo("Host Name", "ALPHABOLD");
            //extent.AddSystemInfo("Environment", "QA");
            //extent.AddSystemInfo("UserName", "ALPHABOLD");
           
            extent = new ExtentReports();            
            extent.AttachReporter(htmlReporter);
        }

        [SetUp]
        public void Setup()
        {
            //User Defined Browser
            if ( Browser.ToLower() == "ie")
            {                
                webDriver = new InternetExplorerDriver(WDEx.SetBrowser(webDriver));
                WDEx.SetBrowser(webDriver);
            }
            else if (Browser.ToLower() == "chrome")
            {
                webDriver = new ChromeDriver();
                WDEx.SetBrowser(webDriver);
            }            
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(3);
            webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(3);
            webDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMinutes(3);
            
            //for desired URL mentioned in baseURL variable. 
            webDriver.Navigate().GoToUrl(baseURL);
            webDriver.Manage().Window.Maximize();

        }

        [TestCase]
         public void a_LoginTest()
         {
            try
            {
                test = extent.CreateTest("LoginTest");
                LoginPage loginPage = new LoginPage(webDriver);
                MainPage mainPage = new MainPage(webDriver);
                
                test.Log(Status.Pass, "Screenshot Below:" + test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "LogIn")));
                loginPage.UserLogin(baseURL,webDriver, userName, passKey);  
                
                test.Log(Status.Pass, "Screenshot Below:" + test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "LogIn2")));
                mainPage.UserSignOut(webDriver);
                test.Log(Status.Pass, "Screenshot Below:" + test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "SignOut")));    
                
            }
            catch(Exception e)
            {
                
                test.Log(Status.Fail, "Screenshot Below:" + test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "Failing Scenario")));                
                Console.WriteLine(e.Message);
            }
         }

        [TestCase]

        public void HotelBooking()
        {
            try
            {
                test = extent.CreateTest("HotelBooking");
                LoginPage loginPage = new LoginPage(webDriver);
                MainPage mainPage = new MainPage(webDriver);
                BookHotel bookHotel = new BookHotel(webDriver);

                loginPage.UserLogin(baseURL,webDriver, userName, passKey);


                //mainPage.TabPlan.Click(); same isn't working sometimes on IE, so this is workaround;
                WDEx.JSExeClick(webDriver, mainPage.TabPlan);
                //mainPage.TabPlanClick(webDriver);
                
                //new Actions(webDriver).DoubleClick(columns[2].FindElement(By.TagName("span"))).Perform();
                mainPage.Departure.Click();

                Thread.Sleep(WDEx.delaySml);

                test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "Departures"));
                mainPage.SwitchToIFrame(webDriver, "contentIFrame0");                
                mainPage.OpenBookHotel(3);

                WDEx.SwitchWindows(webDriver, 1);

                DataInput.initializeHotel();

                bookHotel.PropertyBook(DataInput.hotelDictionary, Browser);

                bookHotel.NumBookRoom(DataInput.roomNumDictionary.Values.Sum());
                
                bookHotel.RoomDetails(webDriver, DataInput.roomNumDictionary, DataInput.adultCountDictionary, DataInput.childCountDictionary, DataInput.childAge);

                test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "HotelDetails"));

                //Simple click isn't working in IE, that's why JSExecutor is used to Click;                
                //bookHotel.Search.Click();
                WDEx.JSExeClick(webDriver, bookHotel.Search);

                bookHotel.SelectRoomOptions(webDriver, DataInput.roomNumDictionary, DataInput.adultCountDictionary, DataInput.childCountDictionary);

                test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "RoomAllocation"));

                //Simple click isn't working in IE, that's why JSExecutor is used to Click;
                //bookHotel.ContinueToPayment.Click();
                WDEx.JSExeClick(webDriver, bookHotel.ContinueToPayment);

                bookHotel.GuestInformation(webDriver, DataInput.roomNumDictionary, DataInput.adultCountDictionary, DataInput.childCountDictionary);

                test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "GuestInformation"));

                //Simple click isn't working in IE, that's why JSExecutor is used to Click;
                //bookHotel.Continue.Click();
                WDEx.JSExeClick(webDriver, bookHotel.Continue);

                bookHotel.FillCardInfo(webDriver, DataInput.roomNumDictionary, DataInput.cardInfo);

                test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "FillCardInfo"));

                //Simple click isn't working in IE, that's why JSExecutor is used to Click;
                //bookHotel.SubmitOrder.Click();
                WDEx.JSExeClick(webDriver, bookHotel.SubmitOrder);

                bookHotel.FillBookInfo(webDriver, DataInput.bookInfo);

                test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "BookInfo"));

                //Simple click isn't working in IE, that's why JSExecutor is used to Click;
                //bookHotel.Reserve.Click();
                WDEx.JSExeClick(webDriver, bookHotel.Reserve);

                test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "Reserve"));

                //Simple click isn't working in IE, that's why JSExecutor is used to Click;
                //bookHotel.Done.Click();
                WDEx.JSExeClick(webDriver, bookHotel.Done);

                Thread.Sleep(WDEx.delayAvg);                

                mainPage.UserSignOut(webDriver);
                test.Log(Status.Pass, "Screenshot Below:" + test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "Failing Scenario")));

            }
            catch (Exception e)
            {
                test.Log(Status.Fail, "Screenshot Below:" + test.AddScreenCaptureFromPath(WDEx.Capture(webDriver, "Failing Scenario")));
                Console.WriteLine(e.Message);
            }
        }


        [TestCase]
        public void TravelReservationOld()
        {
            try
            {
                test = extent.CreateTest("HotelBooking");
                LoginPage loginPage = new LoginPage(webDriver);
                MainPage mainPage = new MainPage(webDriver);
                TravelResOld travelResOld = new TravelResOld(webDriver);
                DataInput.initializeOldTravelReservation();

                loginPage.UserLogin(baseURL,webDriver, userName, passKey);

                //mainPage.TabPlan.Click(); same isn't working sometimes to this is workaround;
                WDEx.JSExeClick(webDriver, mainPage.TabPlan);
                //mainPage.TabPlanClick(webDriver);

                mainPage.TabOperator.Click();
                mainPage.TravelReservation.Click();

                // To Edit the Existing Travel Reservation Both statements needs to be executed;
                /*mainPage.SwitchToIFrame(webDriver, "contentIFrame0");
                mainPage.OpenRecord(5); */

                travelResOld.createNewOtherTR(DataInput.rcTRValue);

                travelResOld.addTravelGuest(DataInput.travelGuest);

                Thread.Sleep(WDEx.delaySml);
                                
                travelResOld.addTravelSegment(DataInput.travelSegment);

                
                Thread.Sleep(WDEx.delaySml);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Thread.Sleep(WDEx.delaySml);
        }

        [TestCase]
        public void TravelReservationAir()
        {
            try
            {
                test = extent.CreateTest("HotelBooking");
                LoginPage loginPage = new LoginPage(webDriver);
                MainPage mainPage = new MainPage(webDriver);
                TravelResOld travelResOld = new TravelResOld(webDriver);

                loginPage.UserLogin(baseURL,webDriver, userName, passKey);

                //mainPage.TabPlan.Click(); same isn't working sometimes to this is workaround;
                WDEx.JSExeClick(webDriver, mainPage.TabPlan);
                //mainPage.TabPlanClick(webDriver);

                mainPage.TabOperator.Click();
                mainPage.TravelReservation.Click();

                mainPage.SwitchToIFrame(webDriver, "contentIFrame0");
                mainPage.OpenRecord(5);
                //WDEx.click(webDriver, mainPage.New);


                Thread.Sleep(WDEx.delayAvg);

                mainPage.SwitchToIFrame(webDriver, "contentIFrame1");
                travelResOld.RecordLocLbl.Click();

                travelResOld.AddGuest.Click();
                webDriver.SwitchTo().DefaultContent();
                Thread.Sleep(WDEx.delaySml);
                mainPage.SwitchToIFrame(webDriver, "InlineDialog_Iframe");
                travelResOld.GuestSearch.SendKeys("Chip");



                webDriver.SwitchTo().DefaultContent();
                WDEx.click(webDriver, mainPage.Save);
                Thread.Sleep(WDEx.delaySml);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Thread.Sleep(WDEx.delaySml);
        }

        [TearDown]
        public void CleanUp()
        {

            WDEx.SwitchWindows(webDriver, 0);
            webDriver.Close();
            webDriver.Quit();
        }

        [OneTimeTearDown]
        public void GenerateReport()
        {
            extent.Flush();
        }
    }
}
