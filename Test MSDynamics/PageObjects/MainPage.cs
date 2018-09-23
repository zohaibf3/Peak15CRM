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
using SeleniumExtras.PageObjects;
using Test_MSDynamics.Common_Methods;

namespace Test_MSDynamics.PageObjects
{
    public class MainPage
    {
        private readonly IWebDriver webDriver;

        public MainPage(IWebDriver driver)
        {
            webDriver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "navTabButtonChangeProfileImageLink")]
        public IWebElement UserIcon { get; set; }

        [FindsBy(How = How.XPath, Using = ".//a[@id='navTabButtonUserInfoSignOutId']")]
        public IWebElement SignOut { get; set; }

        [FindsBy(How = How.XPath, Using = @".//span[@class='ms-crm-CommandBar-Menu' and contains(text(),'New')]")]
        public IWebElement New { get; set; }

        [FindsBy(How = How.XPath, Using = @".//span[@class='ms-crm-CommandBar-Menu' and contains(text(),'Save')]")]
        public IWebElement Save { get; set; }

        [FindsBy(How = How.XPath, Using = @".//span[@class='ms-crm-CommandBar-Menu' and contains(text(),'Save & Close')]")]
        public IWebElement SaveClose { get; set; }

        public IWebElement TabPlan
        {
            get
            {
                return webDriver.FindElement(By.Id("TabPlan"), WDEx.delayAvg);
            }
        }

        public IWebElement TabOperator
        {
            get
            {
                return webDriver.FindElement(By.XPath("//span[text()='Operate']"), WDEx.delayAvg);
            }
        }

        public IWebElement TravelReservation
        {
            get
            {
                return webDriver.FindElement(By.XPath("//span[text()='Travel Reservations']"), WDEx.delayAvg);
            }
        }

        public IWebElement Departure
        {
            get
            {
                return webDriver.FindElement(By.XPath(@".//span[text()='Departures']"), WDEx.delayAvg);
            }
        }

        public IWebElement DepartureSubMenu
        {
            get
            {
                return webDriver.FindElement(By.XPath(".//span[@id='TabNode_tab0Tab']"), WDEx.delayAvg);
            }
        }

        public IWebElement Planner
        {
            get
            {
                return webDriver.FindElement(By.XPath(@".//span[text()='Planner']"));
            }
        }

        public IWebElement BookHotel
        {
            get
            {
                return webDriver.FindElement(By.TagName("body")).FindElement(By.Id("topActionsContainerExpandCollapse")).
                FindElement(By.XPath(".//span[@id='buttonHotelText']"));
            }
        }

        //without JavaScriptExecutor it was only working on Chrome and not on IE. That's why added this generic for both
        public void UserSignOut(IWebDriver driver)
        {
            WDEx.SwitchWindows(driver, 0);
            WDEx.JSExeClick(driver, UserIcon);
            Thread.Sleep(WDEx.delaySml);
            WDEx.JSExeClick(driver, SignOut);
        }

        public void SwitchToIFrame(IWebDriver driver,string iFrameId)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(WDEx.delayBig)).Until(drv =>
                driver.SwitchTo().Frame(drv.FindElement(By.XPath(".//iframe[@id='"+ iFrameId +"']"))));
            //Thread.Sleep(WDEx.delaySml);
        }

        public void OpenBookHotel(int departureNum)
        {
            OpenPlanner(departureNum);


            SwitchToIFrame(webDriver, "contentIFrame1");
            SwitchToIFrame(webDriver, "navLink{5b90c057-ba81-b2d1-c45c-bdbaac5a7c63}AreaFrame");
            Thread.Sleep(WDEx.delaySml);
            //webDriver.FindElement(By.TagName("body")).FindElement(By.Id("topActionsContainerExpandCollapse")).
            //    FindElement(By.XPath(".//span[@id='buttonHotelText']")).Click();            
            //Upper Statement Replaced with Following
            WDEx.JSExeClick(webDriver, BookHotel);
        }
        public void OpenPlanner(int departureNum)
        {
            //Thread.Sleep(WDEx.delaySml);
            OpenRecord(departureNum);

            //Open Planner for Selected Departure
            //WDEx.click(webDriver, By.XPath(".//span[@id='TabNode_tab0Tab']"));
            WDEx.JSExeClick(webDriver, DepartureSubMenu);

            //webDriver.FindElement(By.XPath(@".//span[text()='Planner']"), WDEx.delayAvg).Click();
            WDEx.JSExeClick(webDriver, Planner);

            //Wait to load the Planner: Commented following line because giving error during debug
            Thread.Sleep(WDEx.delaySml);
        }

        public void OpenRecord(int recordNum)
        {
            Thread.Sleep(WDEx.delaySml);
            //Identify total number of Records



            //Identify total number of Records
            IList<IWebElement> rows = webDriver.FindElements(By.XPath(".//table[@id='gridBodyTable']/tbody/tr"));
            //IList<IWebElement> rows = webDriver.FindElement(By.TagName("body")).FindElement(By.Id("crmGrid_divDataBody")).
            //    FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));

            //Identify and Click on From Data for particular 
            IList<IWebElement> columns = rows[recordNum - 1].FindElements(By.TagName("td"));

            //Double Click the date to open Departure
            IWebElement clickable = columns[2].FindElement(By.TagName("span"));
            WDEx.JSExeClick(webDriver, clickable);
            clickable.SendKeys(Keys.Enter);

            //Switch to Main window from iFrame
            webDriver.SwitchTo().DefaultContent();
            Thread.Sleep(WDEx.delayAvg);
        }
    }
}
