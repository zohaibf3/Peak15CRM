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
using Test_MSDynamics.Common_Methods;
using SeleniumExtras.PageObjects;

namespace Test_MSDynamics.PageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver webDriver;
        
        public LoginPage(IWebDriver driver)
        {
            webDriver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//input[@id='userNameInput']")]
        public IWebElement UserName { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='passwordInput']")]
        public IWebElement Password { get; set; }

        public IWebElement LoginButton
        {
            get
            {
                return webDriver.FindElement(By.XPath("//span[@id='submitButton']"));
            }
        }        
                
        public void UserLogin(string baseURL,IWebDriver webDriver, string userName, string passkey)
        {
            UserName.SendKeys(userName);
            Password.SendKeys(passkey);
            WDEx.JSExeClick(webDriver, LoginButton);            
            Thread.Sleep(WDEx.delayAvg);
        }
    }
}
