using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// It will contain all the active orders on supplier screen
    /// </summary>
    public class ActiveOrder
    {
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string BuyerName { get; set; }

        public string Status { get; set; }
    }
}
