using System;
using System.ComponentModel.DataAnnotations;
namespace HackerNewsScraper
{
    public class NewsModel
    {
        [Required(ErrorMessage = "Title is Required")]
        [StringLength(256)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is Required")]
        [StringLength(256)]
        public string Author { get; set; }

        [Url]
        public string Uri { get; set; }

        [Range(0.0, Int32.MaxValue)]
        public int Points { get; set; }

        [Range(0.0, Int32.MaxValue)]
        public int Comments { get; set; }

        [Range(0.0, Int32.MaxValue)]
        public int Rank { get; set; }


    }
}
