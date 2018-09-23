using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System.Threading;
using NUnit.Framework;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using Test_MSDynamics.Common_Methods;
using SeleniumExtras.PageObjects;

namespace Test_MSDynamics.PageObjects
{
    public class BookHotel
    {
        private readonly IWebDriver webDriver;


        public BookHotel(IWebDriver driver)
        {
            webDriver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "property_code")]
        public IWebElement PropertyCode { get; set; }

        [FindsBy(How = How.Id, Using = "check-in")]
        public IWebElement CheckIn { get; set; }

        [FindsBy(How = How.Id, Using = "check-out")]
        public IWebElement CheckOut { get; set; }

        [FindsBy(How = How.Id, Using = "room_count")]
        public IWebElement BookRoom { get; set; }

        [FindsBy(How = How.XPath, Using = ".//button[@type='submit']")]
        public IWebElement Search { get; set; }

        [FindsBy(How = How.XPath, Using = ".//button[text()='Continue to Payment']")]
        public IWebElement ContinueToPayment { get; set; }

        [FindsBy(How = How.XPath, Using = ".//button[@type='submit']")]
        public IWebElement ContinueToBook { get; set; }

        [FindsBy(How = How.XPath, Using = ".//button[@type='submit']")]
        public IWebElement Continue { get; set; }

        [FindsBy(How = How.XPath, Using = ".//button[@type='submit']")]
        public IWebElement SubmitOrder { get; set; }

        [FindsBy(How = How.XPath, Using = ".//button[@type='submit']")]
        public IWebElement Reserve { get; set; }


        [FindsBy(How = How.XPath, Using = ".//button[text()='Done']")]
        public IWebElement Done { get; set; }


        private int totalAdult(Dictionary<string, int> roomNumDictionary, Dictionary<string, int> adultCountDictionary)
        {
            int totalAdult = 0;
            for (int i = 0; i < roomNumDictionary.Count; i++)
            {
                totalAdult = totalAdult + (roomNumDictionary.Values.ElementAt(i) * adultCountDictionary.Values.ElementAt(i));
            }
            return totalAdult;
        }

        private int totalChild(Dictionary<string, int> roomNumDictionary, Dictionary<string, int> childCountDictionary)
        {
            int totalChild = 0;
            for (int i = 0; i < roomNumDictionary.Count; i++)
            {
                totalChild = totalChild + (roomNumDictionary.Values.ElementAt(i) * childCountDictionary.Values.ElementAt(i));
            }
            return totalChild;
        }

        private List<List<string>> adultInfo(int num)
        {
            List<List<string>> adultInfo1 = new List<List<string>>();
            for (int i = 1; i <= num; i++)
            {
                adultInfo1.Add(new List<string> { "First Name Test", "Last Name Test" });

            }
            return adultInfo1;

        }

        private List<List<string>> childtInfo(int num)
        {
            List<List<string>> childInfo1 = new List<List<string>>();
            for (int i = 1; i <= num; i++)
            {
                childInfo1.Add(new List<string> { "First Name Test", "Last Name Test" });

            }
            return childInfo1;

        }

        private string name(string name, int count)
        {
            if (count > 1 && name == "Adult")
                return name + "s";
            else if (count > 1 && name == "Child")
                return "Children";
            else
                return name;
        }

        public void PropertyBook(Dictionary<string, string> hotelDictionary, string Browser)
        {
            Thread.Sleep(WDEx.delaySml);
            PropertyCode.SendKeys(hotelDictionary["HotelName"]);
            WDEx.dateSelector(webDriver, CheckIn, Browser, hotelDictionary["FromDate"]);
            WDEx.dateSelector(webDriver, CheckOut, Browser, hotelDictionary["ToDate"]);            
        }

        public void NumBookRoom(int number)
        {
            SelectElement bookRoom = new SelectElement(BookRoom);
            bookRoom.SelectByValue(Convert.ToString(number));
        }

        public void RoomDetails(IWebDriver driver, Dictionary<string, int> roomNumDictionary, Dictionary<string, int> adultCountDictionary, Dictionary<string, int> childCountDictionary, List<int> childAge)
        {
            int childAgeIndex = 0; //pick from the list of ChildAge on the basis of index    
            int bookedRoom = 0;
            for (int i = 1; i <= roomNumDictionary.Values.Sum(); i++)
            {
                int sameRoom = (roomNumDictionary.Values.Take(i).Sum() - bookedRoom);
                for (int j = 1; j <= sameRoom; j++)
                {
                    new SelectElement(driver.FindElement(By.Id((bookedRoom + 1) + "adult_count"))).SelectByValue(Convert.ToString(adultCountDictionary.Values.ElementAt(i - 1)));
                    new SelectElement(driver.FindElement(By.Id((bookedRoom + 1) + "children_count"))).SelectByValue(Convert.ToString(childCountDictionary.Values.ElementAt(i - 1)));

                    for (int k = 1; k <= childCountDictionary.Values.ElementAt(i - 1); k++)
                    {
                        new SelectElement(driver.FindElement(By.Id((bookedRoom + 1) + "child_age" + k))).SelectByValue(Convert.ToString(childAge[childAgeIndex]));
                        childAgeIndex++;
                    }
                    bookedRoom++;
                }
                if (bookedRoom == roomNumDictionary.Values.Sum())
                    break;
                else
                    continue;
            }
        }

        public void SelectRoomOptions(IWebDriver driver, Dictionary<string, int> roomNumDictionary, Dictionary<string, int> adultCountDictionary, Dictionary<string, int> childCountDictionary)
        {
            Thread.Sleep(WDEx.delaySml);

            int selected = 0;
            for (int i = 1; i <= roomNumDictionary.Values.Sum(); i++)
            {
                int random = 0;

                IList<IWebElement> accommodates = null;
                IList<IWebElement> numRooms = null;

                if (childCountDictionary.Values.ElementAt(i - 1) <= 0)
                {
                    try
                    {
                        //Select Random available Room for the Booking
                        accommodates = webDriver.FindElements(By.XPath(".//div[text()='" + adultCountDictionary.Values.ElementAt(i - 1) + " "
                            + name("Adult", adultCountDictionary.Values.ElementAt(i - 1)) + "']"));
                        random = WDEx.rnd.Next(0, accommodates.Count);
                        accommodates[random].FindElement(By.XPath("..")).Click();
                        Thread.Sleep(WDEx.delaySml);


                        //Book the Quantity through the opened Popup
                        numRooms = webDriver.FindElements(By.XPath(".//select[@id='room_count']"));
                        random = WDEx.rnd.Next(0, numRooms.Count);
                        new SelectElement(numRooms[random]).SelectByText(Convert.ToString(roomNumDictionary.Values.ElementAt(i - 1))); // Select Rooms from the given Options

                        webDriver.FindElement(By.XPath(".//button[text()='Submit']")).Click();
                        for (int k = 1; k <= roomNumDictionary.Values.ElementAt(i - 1); k++)
                        {
                            new SelectElement(webDriver.FindElement(By.Id((selected) + "select"))).SelectByIndex(k);
                            selected++;
                        }
                        if (selected == roomNumDictionary.Values.Sum())
                            break;
                        continue;
                    }
                    catch
                    {
                        continue;
                    }
                }
                else if (childCountDictionary.Values.ElementAt(i - 1) > 0)
                {
                    try
                    {
                        accommodates = webDriver.FindElements(By.XPath(".//div[text()='" + adultCountDictionary.Values.ElementAt(i - 1) + " " +
                            name("Adult", adultCountDictionary.Values.ElementAt(i - 1)) + ", " + childCountDictionary.Values.ElementAt(i - 1) + " " +
                            name("Child", childCountDictionary.Values.ElementAt(i - 1)) + "']"));
                        random = WDEx.rnd.Next(0, accommodates.Count);
                        accommodates[random].FindElement(By.XPath("..")).Click();
                        Thread.Sleep(WDEx.delaySml);

                        //Book the Quantity through the opened Popup
                        numRooms = webDriver.FindElements(By.XPath(".//select[@id='room_count']"));
                        random = WDEx.rnd.Next(0, numRooms.Count);
                        new SelectElement(numRooms[random]).SelectByText(Convert.ToString(roomNumDictionary.Values.ElementAt(i - 1)));

                        webDriver.FindElement(By.XPath(".//button[text()='Submit']")).Click();
                        for (int k = 1; k <= roomNumDictionary.Values.ElementAt(i - 1); k++)
                        {
                            new SelectElement(webDriver.FindElement(By.Id((selected) + "select"))).SelectByIndex(k);
                            selected++;
                        }
                        if (selected == roomNumDictionary.Values.Sum())
                            break;
                        continue;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

        }

        public void GuestInformation(IWebDriver driver, Dictionary<string, int> roomNumDictionary, Dictionary<string, int> adultCountDictionary, Dictionary<string, int> childCountDictionary)
        {
            Thread.Sleep(WDEx.delaySml);
            List<List<string>> adultInfoList = null;
            List<List<string>> childInfoList = null;

            adultInfoList = adultInfo(totalAdult(roomNumDictionary, adultCountDictionary));
            childInfoList = childtInfo(totalChild(roomNumDictionary, childCountDictionary));

            //Filling the required information on Form
            int infoFilled = 0;
            bool flag = false;
            for (int i = 1; i <= roomNumDictionary.Values.Sum(); i++)
            {
                for (int k = 1; k <= roomNumDictionary.Values.ElementAt(i - 1); k++)
                {

                    for (int j = 0; j < adultCountDictionary.Values.ElementAt(i - 1); j++)
                    {
                        driver.FindElement(By.Id((infoFilled) + "first_name" + j)).SendKeys(Convert.ToString(adultInfoList[i - 1][0]));
                        driver.FindElement(By.Id((infoFilled) + "last_name" + j)).SendKeys(Convert.ToString(adultInfoList[i - 1][1]));
                    }
                    for (int j = 0; j < childCountDictionary.Values.ElementAt(i - 1); j++)
                    {
                        driver.FindElement(By.Id((infoFilled) + "child_first_name" + j)).SendKeys(Convert.ToString(childInfoList[i - 1][0]));
                        driver.FindElement(By.Id((infoFilled) + "child_last_name" + j)).SendKeys(Convert.ToString(childInfoList[i - 1][1]));
                    }
                    infoFilled++;
                    if (infoFilled == roomNumDictionary.Values.Sum())
                    {
                        flag = true;
                        break;
                    }
                    continue;
                }
                if (flag == true)
                    break;
                continue;
            }
        }

        public void FillCardInfo(IWebDriver driver, Dictionary<string, int> roomNumDictionary, List<string> cardInfo)
        {
            Thread.Sleep(WDEx.delaySml);
            IList<IWebElement> expireMonth = driver.FindElements(By.Id("expiration_month"));
            IList<IWebElement> expireYear = driver.FindElements(By.Id("expiration_year"));
            for (int i = 0; i < roomNumDictionary.Values.Sum(); i++)
            {
                new SelectElement(driver.FindElement(By.Id("adult_count" + i))).SelectByValue(cardInfo[0]); 

                driver.FindElement(By.Id("cardholder_name" + i)).SendKeys(cardInfo[1]);
                driver.FindElement(By.Id("cardholder_number" + i)).SendKeys(cardInfo[2]);
                new SelectElement(expireMonth[i]).SelectByValue(cardInfo[3]);
                new SelectElement(expireYear[i]).SelectByValue(cardInfo[4]);
                driver.FindElement(By.Id("security_code" + i)).SendKeys(cardInfo[5]);
            }
        }

        public void FillBookInfo(IWebDriver driver, List<string> bookInfo)
        {
            Thread.Sleep(WDEx.delaySml);
            driver.FindElement(By.Id("first_name1")).SendKeys(bookInfo[0]);
            driver.FindElement(By.Id("last_name1")).SendKeys(bookInfo[1]);
            driver.FindElement(By.Id("city")).SendKeys(bookInfo[2]);
            new SelectElement(driver.FindElement(By.Id("country"))).SelectByValue(bookInfo[3]);
            Thread.Sleep(WDEx.delaySml);
            new SelectElement(driver.FindElement(By.Id("state"))).SelectByValue(bookInfo[4]);
            driver.FindElement(By.Id("telephone")).SendKeys(bookInfo[5]);
            driver.FindElement(By.Id("email")).SendKeys(bookInfo[6]);
        }
    }
}