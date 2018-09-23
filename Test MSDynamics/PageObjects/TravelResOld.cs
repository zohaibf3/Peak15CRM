using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Threading;
using NUnit.Framework;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using SeleniumExtras.PageObjects;
using Test_MSDynamics.Common_Methods;

namespace Test_MSDynamics.PageObjects
{
    public class TravelResOld
    {
        private readonly IWebDriver webDriver;        

        public TravelResOld(IWebDriver driver)
        {
            webDriver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "p15_name_cl")]
        public IWebElement RecordLocLbl { get; set; }

        [FindsBy(How = How.Id, Using = "p15_name_i")]
        public IWebElement RecordLoc { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id = 'Company']")]
        public IWebElement Company { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@name='Number']")]
        public IWebElement Number { get; set; }

        [FindsBy(How = How.Id, Using = "DepartureDate")]
        public IWebElement DepartDate { get; set; }


        [FindsBy(How = How.Id, Using = "DepartureIATA")]
        public IWebElement Depart { get; set; }


        [FindsBy(How = How.Id, Using = "DepartureTime")]
        public IWebElement DepartTime { get; set; }


        [FindsBy(How = How.Id, Using = "ArrivalDate")]
        public IWebElement ArrivalDate { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@name = 'ArrivalIATA']")]
        public IWebElement Arrival { get; set; }

        [FindsBy(How = How.Id, Using = "ArrivalTime")]
        public IWebElement ArrivalTime { get; set; }

        [FindsBy(How = How.XPath, Using = "//select[@name='modeList']")]
        public IWebElement TravelMode { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@id='guestsubgrid_addImageButton']")]
        public IWebElement AddGuest { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='lookup_guestsubgrid_ledit']")]
        public IWebElement GuestId { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='Dialog_lookup_guestsubgrid_i_IMenu']/*/*/li[1]")]
        public IWebElement GuestIdValueSelect { get; set; }

        [FindsBy(How = How.Id, Using = "crmGrid_findCriteria")]
        public IWebElement GuestSearch { get; set; }

        public void selectTravelMode(int number)
        {
            SelectElement travelMode = new SelectElement(TravelMode);
            travelMode.SelectByValue(Convert.ToString(number));
        }

        public void setDate(string Date, IWebElement webElement)
        {                        
            webElement.SendKeys(Date);
            Company.Click();
        }

        public void createNewOtherTR(string rcValue)
        {
            MainPage mainPage = new MainPage(webDriver);

            WDEx.click(webDriver, mainPage.New);
            Thread.Sleep(WDEx.delayAvg);
            mainPage.SwitchToIFrame(webDriver, "contentIFrame1");

            Thread.Sleep(WDEx.delaySml);
            RecordLocLbl.Click();
            RecordLoc.SendKeys(rcValue);

            webDriver.SwitchTo().DefaultContent();
            WDEx.click(webDriver, mainPage.Save);
            Thread.Sleep(WDEx.delaySml);
        }

        public void addTravelGuest(List<string> travelGuest)
        {
            MainPage mainPage = new MainPage(webDriver);
            Thread.Sleep(WDEx.delaySml);
            mainPage.SwitchToIFrame(webDriver, "contentIFrame1");
            for (int i = 0; i < travelGuest.Count; i++)
            {
                RecordLocLbl.Click();

                AddGuest.Click();
                GuestId.SendKeys(travelGuest[i]);
                GuestId.SendKeys(Keys.Enter);
                GuestIdValueSelect.Click();
            }
            webDriver.SwitchTo().DefaultContent();
        }
        public void addTravelSegment(List<List<string>> travelSegment)
        {
            MainPage mainPage = new MainPage(webDriver);
            
            for (int i = 0; i < travelSegment.Count; i++)
            {
                mainPage.SwitchToIFrame(webDriver, "contentIFrame1");
                RecordLocLbl.Click();
                mainPage.SwitchToIFrame(webDriver, "IFRAME_TravelSegments");

                Company.Clear();
                Company.SendKeys(travelSegment[i][0]);

                selectTravelMode(Convert.ToInt32(travelSegment[i][1]));

                Number.Clear();
                Number.SendKeys(travelSegment[i][2]);

                DepartDate.Clear();
                setDate(travelSegment[i][3], DepartDate);

                Depart.Clear();
                Depart.SendKeys(travelSegment[i][4]);


                DepartTime.SendKeys(travelSegment[i][5]);
                setDate(travelSegment[i][6], ArrivalDate);
                Arrival.SendKeys(travelSegment[i][7]);
                ArrivalTime.SendKeys(travelSegment[i][8]);

                webDriver.SwitchTo().DefaultContent();
                WDEx.click(webDriver, mainPage.Save);

                Thread.Sleep(WDEx.delaySml);
            }
        }
    }
}
