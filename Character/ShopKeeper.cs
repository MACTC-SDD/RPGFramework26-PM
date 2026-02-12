using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal class ShopKeeper : NonPlayer
    {
        public string Greeting { get; set; } = "";
        public double UpCharge { get; set; } = 1.5;
    }
}
