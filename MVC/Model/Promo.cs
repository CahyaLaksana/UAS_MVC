using System;
using System.Collections.Generic;
using System.Text;

namespace MVC.Model
{
    public class Promo
    {
        public string nama { get; set; }
        public string promo { get; set; }
  
        public Promo(string title, string promo)
        {
            this.nama = title;
            this.promo = promo;
        }
    }
}
