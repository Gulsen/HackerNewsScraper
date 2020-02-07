using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;
namespace HackerNewsScraper
{
    public class Scraper
    {
        private string base_url = "https://news.ycombinator.com/";

        public string Crawl(int number)
        {

            var web = new HtmlWeb();
            var url = base_url;

            List<NewsModel> list = new List<NewsModel>();

            try
            {       
           // iterate throught every item until the post number is reached.
            while (number > 0)
            {
                var doc = web.Load(url);
                var titles = doc.DocumentNode.SelectNodes("//tr[@class='athing']").ToList();
                var infos = doc.DocumentNode.SelectNodes("//td[@class='subtext']").ToList();

                // getting all the data and saving to model
                foreach (var item in titles.Zip(infos, Tuple.Create))
                {
                    NewsModel news = new NewsModel();

                    var node = item.Item1.SelectSingleNode(".//a[@class='storylink']");
                    news.Title = node != null ? node.InnerHtml : "";
                    
                    news.Uri= node.Attributes["href"].Value;

                    if (item.Item1.SelectSingleNode(".//span[@class='rank']") != null)
                        {
                            news.Rank = Int32.Parse(item.Item1.SelectSingleNode(".//span[@class='rank']").InnerText.Replace(".", ""));
                        }
                        
                    news.Author = item.Item2.SelectSingleNode(".//a[@class='hnuser']") != null ?
                                item.Item2.SelectSingleNode(".//a[@class='hnuser']").InnerText : "";

                    if (item.Item2.SelectSingleNode(".//span[@class='score']") != null)
                        {
                            var points = item.Item2.SelectSingleNode(".//span[@class='score']").InnerText.Split(' ');
                            news.Points = Int32.Parse(points[0].Trim());
                        }
                       
                       
                    var comments = item.Item2.SelectNodes(".//a").ToArray();
                    if (comments.Length >0 )
                           {
                               news.Comments = comments[comments.Length-1].InnerText.Contains("comments")
                                  ? Int32.Parse(comments[comments.Length-1].InnerText.Replace("&nbsp;comments", "").Trim()) : 0;
                           }
                        
                    // model validation check
                    if (IsValid(news))
                       list.Add(news);
                  

                    number--;
                    if (number == 0) break;
                }
                // go to next page
                if (number > 0)
                {
                    var more = doc.DocumentNode.SelectSingleNode("//a[@class='morelink']").Attributes["href"].Value;
                    url = base_url + '/' + more;
                }
            }

                if (list != null)
                {              
                    return JsonConvert.SerializeObject(list);
                }

                return String.Empty;
                

            }
            catch (Exception ex)
            {
                // log ex
                throw ex;
            }
           
        }

        // check if model is valid
        private bool IsValid(NewsModel model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, results, true);
            return valid;

        }
    }
}

