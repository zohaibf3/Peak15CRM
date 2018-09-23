using System;
using System.Linq;
using OpenQA.Selenium;

namespace Test_MSDynamics.Environment
{
    public class EnvironmentConfig
    {
        
     // FOR Internet Explorer    = ie
     // FOR Google Chrome        = chrome                                             
        protected string Browser = "chrome";

        protected IWebDriver webDriver;
        protected string userName = @"tripsync\tali";
        protected string passKey = "Alphabold!";
        protected string baseURL = @"https://stagedataqaee.peak15systems.com/main.aspx#645106239";
        protected string title = "Dashboards: Microsoft Dynamics 365 Social Overview - Microsoft Dynamics 365";
    }
}
