using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Threading;
using NUnit.Framework;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using Test_MSDynamics.DataHelper;
using Test_MSDynamics.Common_Methods;
using AventStack.ExtentReports;

namespace Test_MSDynamics.Test_Scripts
{
    [TestFixture]
    public class DepartureCreation
    {
        string Browser = "ie";
        IWebDriver webDriver;
        String userName = @"tripsync\tali";
        String passKey = "alphabold1!";
        String title = "Dashboards: Microsoft Dynamics 365 Social Overview - Microsoft Dynamics 365";

        [SetUp]
        public void Setup()
        {
            //User Defined Browser
            if (Browser.ToLower() == "ie")
            {
                webDriver = new InternetExplorerDriver();
            }
            else if (Browser.ToLower() == "chrome")
            {
                webDriver = new ChromeDriver();
            }
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(3);
            webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(3);
            webDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMinutes(3);
            //webDriver.Navigate().GoToUrl("https://stagedataqa2.peak15systems.com/");
            //for 5b URL
            webDriver.Navigate().GoToUrl("https://stagedata5b.peak15systems.com/");
            webDriver.Manage().Window.Maximize();

        }

        [TestCase]
        public void b_DepartureCreation()
        {
            
            try
            {
                
                DataInput.initializeDeparture();

                webDriver.FindElement(By.XPath("//input[@id='ctl00_ContentPlaceHolder1_UsernameTextBox']")).SendKeys(userName);

                webDriver.FindElement(By.XPath("//input[@id='ctl00_ContentPlaceHolder1_PasswordTextBox']")).SendKeys(passKey);

                webDriver.FindElement(By.XPath("//input[@id='ctl00_ContentPlaceHolder1_SubmitButton']")).Click();

                //Click Plan
                webDriver.FindElement(By.Id("TabPlan"), WDEx.delayAvg).Click();

                
                //Click Departures
                webDriver.FindElement(By.XPath(@".//span[text()='Departures']"), WDEx.delayAvg).Click();

                
                //Click New Departure
                WDEx.click(webDriver, By.XPath(@".//span[@class='ms-crm-CommandBar-Menu' and contains(text(),'New')]"));

                //Wait
                Thread.Sleep(WDEx.delayAvg);
                

                // Choose Grid
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(WDEx.delayBig)).Until(drv => 
                        webDriver.SwitchTo().Frame(drv.FindElement(By.XPath(".//iframe[@id='contentIFrame1']"))));

                // Choose Records
                IList<IWebElement> rows = webDriver.FindElement(By.TagName("body")).FindElement(By.Id("formContainer")).
                       FindElement(By.Id("areaForm")).FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));

                              
                List<int> checkValues = new List<int> { 1, 5, 7, 9, 10, 11, 12, 13, 15 };
                for (int i = 1; i < rows.Count; i++)
                {

                    if (checkValues.Contains(i))
                    {
                        
                        IList<IWebElement> columns = rows[i].FindElements(By.TagName("td"));
                                                
                        columns[1].FindElement(By.TagName("div")).Click();

                        if (i == 1)
                        {

                            webDriver.FindElement(By.Id("p15_tripid_ledit")).SendKeys("PS-265");

                        }
                        else if (i == 5)
                        {
                            webDriver.FindElement(By.Id("p15_startdate_iDateInput")).SendKeys("02/26/2019");

                        }
                        else if (i == 7)
                        {
                            webDriver.FindElement(By.Id("p15_enddate_iDateInput")).SendKeys("03/05/2019");

                        }
                        else if (i == 9)
                        {
                            webDriver.FindElement(By.Id("p15_minguests_i")).SendKeys("5");

                        }
                        else if (i == 10)
                        {
                            webDriver.FindElement(By.Id("p15_maxguests_i")).SendKeys("15");

                        }
                        else if (i == 11)
                        {
                            var childf = "";
                            var childt = "";
                            string text = "Adults";
                            var textTest = DataInput.guestType[text];

                            //new SelectElement(webDriver.FindElement(By.Id("p15_departure_guesttypes_i"))).SelectByValue(WDExt.guestType[text]);
                            //new SelectElement(webDriver.FindElement(By.Id("p15_departure_guesttypes_i"))).SelectByValue(text = (text == "Adults") ? "0" : "1");
                            webDriver.FindElement(By.XPath(@".//select[@id='p15_departure_guesttypes_i']/option[@value='"+ DataInput.guestType[text] + "']")).Click();
                            //var TextValue = webDriver.FindElement(By.XPath(@".//select[@id='p15_departure_guesttypes_i']"));


                            if ( text == "Adults & Children")
                            {
                                // Add Child Records - Currently it is 3
                                for (int j = 1; j <= DataInput.childNum; j++)
                                {
                                    if (j == 1)
                                    {
                                        childf = DataInput.firstChildf;
                                        childt = DataInput.firstChildt;
                                    }
                                    else if (j == 2)
                                    { 
                                        childf = DataInput.secChildf;
                                        childt = DataInput.secChildt;
                                    }
                                    else
                                    {
                                        childf = DataInput.thirChildf;
                                        childt = DataInput.thirChildt;
                                    }

                                    IWebElement childDiv = webDriver.FindElement(By.XPath(".//table[@name='Child" + j + "']")).FindElement(By.TagName("tbody"));
                                    childDiv.FindElement(By.XPath(".//div[@id='p15_child" + j + "agefrom']")).Click();

                                    childDiv.FindElement(By.XPath(".//div[@id='p15_child" + j + "agefrom']/div[2]/input[@id='p15_child" + j + "agefrom_i']")).SendKeys(childf);


                                    childDiv.FindElement(By.XPath(".//div[@id='p15_child" + j + "ageto']")).Click();

                                    childDiv.FindElement(By.XPath(".//div[@id='p15_child" + j + "ageto']/div[2]/input[@id='p15_child" + j + "ageto_i']")).SendKeys(childt);

                                    childDiv.FindElement(By.XPath(".//div[@id='p15_child" + j + "ageto']/div[2]/input[@id='p15_child" + j + "ageto_i']")).SendKeys(Keys.Enter);
                                }
                            }
                        }
                        else if (i == 12)
                        {

                            webDriver.FindElement(By.XPath(".//select[@id='p15_planforleaders_i']/option[@value='0']")).Click();

                        }
                        else if (i == 13)
                        {
                            webDriver.FindElement(By.Id("p15_paymentscheduleid_ledit")).SendKeys("Standard Scheduled");

                        }
                        else if (i == 15)
                        {
                            var text = "Quoting";
                            new SelectElement(webDriver.FindElement(By.XPath(@".//select[@id='statuscode_i']"))).SelectByText(Convert.ToString(text));
                        }
                    }

                    else
                        continue;
                }

                Thread.Sleep(WDEx.delaySml);
                webDriver.SwitchTo().DefaultContent();

                //Save & Close Departure;
                WDEx.click(webDriver, By.XPath(@".//span[@class='ms-crm-CommandBar-Menu' and contains(text(),'Save & Close')]"));
                //WDExt.click(webDriver, By.XPath(@".//span[@class='ms-crm-CommandBar-Menu' and contains(text(),'Add Items')]"));

                Thread.Sleep(WDEx.delayAvg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        [TearDown]
        public void CleanUp()
        {
            webDriver.Close();
        }
    }
}
