using Buyer.DTO;
using Buyer.MasterData;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;
using System;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Buyer.Pages
{
    public class BuyerModel : PageModel
    {
        #region Private Properties
        private readonly ILogger<BuyerModel> _logger;
        private AppSettings _appSettings { get; set; }
        #endregion

        #region Public Properties

        public ItemMaster Item  { get; set; }
        public int Quantity { get; set; }
        public string BuyerName { get; set; }

    #endregion

        public BuyerModel(ILogger<BuyerModel> logger, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _appSettings = settings.Value;
        }

        /// <summary>
        /// It will get called on load of the page
        /// </summary>
        public void OnGet()
        {
            //Get the List Item options for UI
            ViewData["Items"] = ItemMaster.GetItems().Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name.ToString()
            }).ToList();

            //Get the List of Quatity options for UI
            ViewData["Quantity"] = Enumerable.Range(1, 4)
            .Select(n => new SelectListItem
            {
                Value = n.ToString(),
                Text = n.ToString()
            }).ToList();
        }

        /// <summary>
        /// Getting the posted values and put it into the Data Transfer Object (DTO) which will get cast into a Message to pass to Azure Service Bus
        /// </summary>
        public void OnPostSubmit()
        {
            //Get form values
            var quantity = Convert.ToInt32(Request.Form["Quantity"]);
            var itemName = ItemMaster.GetItems().Where(m => m.Id == Convert.ToInt32(Request.Form["Item"])).FirstOrDefault().Name;
            var buyerName = Request.Form["BuyerName"].ToString();

            //DTO for Service Bus
            BuyerRequestDTO buyerRequestDTO = new BuyerRequestDTO() { ItemName = itemName, Quantity = quantity, BuyerName = buyerName };

            //Read Service Bus Connection string and QueueName from app settings
            string serviceBusConnectionString = _appSettings.ServiceBusConnectionString;
            string queueName = _appSettings.ServiceBusQueueNameForBuyerRequests;

            //Create a service bus queue client using Service bus connection string and queue name
            IQueueClient sbQueueClient = new QueueClient(serviceBusConnectionString, queueName);

            //Create a Service Bus Message object that will be passed to queue
            Message sbMessageForQueue = new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(buyerRequestDTO)));
            sbMessageForQueue.ContentType = "application/json";

            //Send the message to service bus
            sbQueueClient.SendAsync(sbMessageForQueue);
        }

       
    }
}
