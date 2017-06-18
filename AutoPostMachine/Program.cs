using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoPostMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeDriver cd = new ChromeDriver(@"C:\CsharpProjects\AutoPostMachine\");
            cd.Url = @"https://passport.skykiwi.com/v1/login/bbslogon.do";
            cd.Navigate();
            IWebElement e = cd.FindElementById("username");
            e.SendKeys("xxxxxxxxxxxxxxxxxxx");
            e = cd.FindElementById("password");
            e.SendKeys("xxxxxxxxxxxxxxxxxxxxx");
            e = cd.FindElementById("btnLoginAct");            
            e.Click();

            //Get the cookies
            CookieContainer cc = new CookieContainer();
            foreach (OpenQA.Selenium.Cookie c in cd.Manage().Cookies.AllCookies)
            {
                string name = c.Name;
                string value = c.Value;
                cc.Add(new System.Net.Cookie(name, value, c.Path, c.Domain));
            }            

            //Fire off the request
            HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create("https://passport.skykiwi.com/v1/login/bbslogon.do");
       
            hwr.CookieContainer = cc;
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            
            StreamWriter swr = new StreamWriter(hwr.GetRequestStream());
            swr.Write("feeds=35");
            swr.Close();

            WebResponse wr = hwr.GetResponse();
            string s = new System.IO.StreamReader(wr.GetResponseStream()).ReadToEnd();

            cd.Url = @"http://bbs.skykiwi.com/forum.php?mod=viewthread&tid=3340790";            
            cd.Navigate();
            e = cd.FindElementById("fastpostmessage");
            e.SendKeys("ddddddddddddddddddddddddddddddddddddddddddddd");
            e = cd.FindElementById("fastpostsubmit");
            
            e.Click();
            cd.Close();
            cd.Quit();
        }
    }
}

//e = cd.FindElementByXPath(@"//*[@id=""main""]/div/div/div[2]/table/tbody/tr/td[1]/div/form/fieldset/table/tbody/tr[6]/td/button");
//HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create("http://bbs.skykiwi.com/forum.php?mod=viewthread&tid=3340790");
//string postData = "dddddddddddddddddddddddddddddddddddddddd";
//byte[] send = Encoding.Default.GetBytes(postData);
//hwr.ContentLength = send.Length;
//Stream swr = hwr.GetRequestStream();
//swr.Write(send, 0, send.Length);
