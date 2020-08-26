using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace Buyer.MasterData
{
    public class ItemMaster
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ItemMaster()
        {

        }

        /// <summary>
        /// Parameterized Controller
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ItemMaster(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        /// <summary>
        /// Item Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Item Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get Master Data
        /// </summary>
        /// <returns></returns>
        public static List<ItemMaster> GetItems()
        {
            var items = new List<ItemMaster>();

            items.Add(new ItemMaster(1, "Item 1"));
            items.Add(new ItemMaster(1, "Item 2"));
            items.Add(new ItemMaster(1, "Item 3"));
            items.Add(new ItemMaster(1, "Item 4"));
            items.Add(new ItemMaster(1, "Item 5"));

            return items;
        }

    }
}
