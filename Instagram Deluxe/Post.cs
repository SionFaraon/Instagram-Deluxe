using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram_Deluxe
{
    internal class Post
    {
        public String Image {  get; set; }
        public String Description { get; set; }
        public DateTime Date { get; set; }
        public int Likes { get; set; }

        public Post(String image, string description)
        {
            this.Image = image;
            this.Likes = 0;
            this.Date = DateTime.Now;
            Description = description;
        }

        public Post(String image, DateTime date, int Likes, string description)
        {
            this.Image = image;
            this.Date = date;
            this.Likes = Likes;
            this.Description = description;
        }

    }
}
