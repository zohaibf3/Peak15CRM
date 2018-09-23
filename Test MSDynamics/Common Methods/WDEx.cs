using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AventStack.ExtentReports;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Test_MSDynamics.Common_Methods
{
    public static class WDEx 
    {
        public static int delayBig = 40000;
        public static int delayAvg = 20000;
        public static int delaySml = 4000;

        public static Random rnd = new Random();

        public static void SwitchWindows(IWebDriver driver, int windows)
        {
            driver.SwitchTo().Window(driver.WindowHandles[windows]);
            driver.Manage().Window.Maximize();
        }

        public static IWebElement FindElement(this IWebDriver webDriver, By by, int timeoutInSeconds)
        {

            if (timeoutInSeconds > 0)
            {
                
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(by));
                return wait.Until(drv => drv.FindElement(by));
            }
            return webDriver.FindElement(by);

        }

        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver webDriver, By by, int timeoutInSeconds)
        {
            Thread.Sleep(delaySml);
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
            }
            return webDriver.FindElements(by);

        }

        public static void click(this IWebDriver webDriver, By by)
        {
            try
            {
                Thread.Sleep(delaySml);
                (new WebDriverWait(webDriver, TimeSpan.FromSeconds(20))).Until(ExpectedConditions.ElementToBeClickable(by));

                webDriver.FindElement(by).Click();
            }
            catch (StaleElementReferenceException e)
            {
                webDriver.FindElement(by).Click();
            }
        }

        public static void JSExeClick(this IWebDriver webDriver, IWebElement webElement)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
                js.ExecuteScript("arguments[0].click()", webElement);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void click(this IWebDriver webDriver, IWebElement webElement)
        {
            try
            {
                Thread.Sleep(delaySml);
                (new WebDriverWait(webDriver, TimeSpan.FromSeconds(20))).Until(ExpectedConditions.ElementToBeClickable(webElement));
                webElement.Click();                
            }
            catch (StaleElementReferenceException e)
            {
                webElement.Click();                
            }
        }
        public static void dateSelector(IWebDriver webDriver, IWebElement webElement, string Browser, string Date)
        {            
            webElement.Click();
            webElement.SendKeys(Keys.Control + 'a');
            webElement.SendKeys(Keys.Backspace);                       
            

            if (Browser.ToLower() == "ie")
            {
                //Thread.Sleep(WDEx.delaySml);
                for (int i = 0; i < Date.Length; i++)
                {
                    webElement.SendKeys(Date[i].ToString());
                }
            }
            else
                webElement.SendKeys(Date);

            webElement.SendKeys(Keys.Tab);

        }

        public static string Capture(IWebDriver webDriver, string screenShotName)
        {
            Thread.Sleep(WDEx.delaySml);
            ITakesScreenshot screenCapture = (ITakesScreenshot)webDriver;
            Screenshot screenShot = screenCapture.GetScreenshot();


            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string finalPath = pth.Substring(0, pth.LastIndexOf("bin")) + "Screenshots\\" + screenShotName + ".png";
            string localPath = new Uri(finalPath).LocalPath;
            screenShot.SaveAsFile(localPath, ScreenshotImageFormat.Png);
            return localPath;
        }

        public static InternetExplorerOptions SetBrowser(IWebDriver webDriver)
        {                

            InternetExplorerOptions options = new InternetExplorerOptions();
            options.IgnoreZoomLevel = true;
            return options;
        }
    }  
}