using Buyer.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.DTO
{

    /// <summary>
    /// We will send this object over Service Bus Queue
    /// </summary>
    public class BuyerRequestDTO
    {

        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string BuyerName { get; set; }


    }
}
