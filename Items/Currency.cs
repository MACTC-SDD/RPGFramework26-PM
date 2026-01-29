using RPGFramework.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RPGFramework.Items
{
    internal class Currency : Item
    {
        public int Amount { get; set; } = 0;
        public int MaxAmount { get; set; } = 500;

        public CurrencyType CurrencyType { get; set; }

        public void CurrencyAmount()
        {
            switch (this.CurrencyType)
            {
                case CurrencyType.Coins:
                    Amount = 10;
                    if (Amount > MaxAmount)
                    {
                        Amount = MaxAmount;
                    }
                    break;


            }
        }
    }
}
