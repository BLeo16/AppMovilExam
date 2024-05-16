using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMovilExam.Models
{
    public class Post
    {
        public int id { get; set; }

     
        public string title { get; set; }
     
        public string content { get; set; }

        public int userId { get; set; }

        public User? user { get; set; }
    }
}
