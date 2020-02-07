using System;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace HackerNewsScraper
{
    class HackerNewsScraper
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hacker News Crapler!");
            Console.WriteLine("Please enter the number of posts you want to read: ");
            string input = Console.ReadLine();

              // check user input
              int posts;
              if (Int32.TryParse(input, out posts) && posts<=100 && posts>0)
              {
                   var scraper = new Scraper().Crawl(posts);
                   //print result
                   string jsonFormatted = JToken.Parse(scraper).ToString((Newtonsoft.Json.Formatting)Formatting.Indented);
                   Console.WriteLine(jsonFormatted);
           
              }
              else
              {
                  Console.WriteLine("Please enter an input between 1 and 100");
              }
              
        }
    }
}


