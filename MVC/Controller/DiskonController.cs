using MVC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVC.Controller
{
    class DiskonController
    {
        private List<Promo> promo;
        public DiskonController()
        {
            promo = new List<Promo>();
        }

        public void addPromo(Promo promo)
        {
            this.promo.Add(promo);
        }

        public List<Promo> getPromo()
        {
            return this.promo;
        }
    }
}
