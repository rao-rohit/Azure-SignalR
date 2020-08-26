using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;
using Supplier.Models;

namespace Supplier.Controllers
{
    public class SupplierController : Controller
    {
        #region Private Properties
        private readonly ILogger<SupplierController> _logger;
        private AppSettings _appSettings { get; set; }
        #endregion

        public SupplierController(ILogger<SupplierController> logger, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _appSettings = settings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<ActiveOrder> lstActivOrders = new List<ActiveOrder>();

            lstActivOrders=  GetActiveOrdersFromDataBase();

            return View(lstActivOrders);
        }

        private List<ActiveOrder> GetActiveOrdersFromDataBase()
        {
            List<ActiveOrder> lstActiveOrder = new List<ActiveOrder>();

            using (SqlConnection con = new SqlConnection(_appSettings.SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_active_orders_get";
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            //Create ActiveOrder object
                            ActiveOrder ao = new ActiveOrder
                            {
                                BuyerName = dataReader["BuyerName"].ToString(),
                                ItemName = dataReader["ItemName"].ToString(),
                                OrderId = Convert.ToInt32(dataReader["OrderId"]),
                                Quantity = Convert.ToInt32(dataReader["Quantity"]),
                                Status = dataReader["Status"].ToString()
                            };

                            //Add Active order object to list
                            lstActiveOrder.Add(ao);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    dataReader.Close();
                }

            }

            return lstActiveOrder;
        }

        /// <summary>
        /// It will update the buyer order with suppliuer response - Status : Accepted / Rejected
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        public string SupplierResponse(int orderId, string status)
        {
            using (SqlConnection con = new SqlConnection(_appSettings.SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_supplier_response_to_buyer_order";
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.ExecuteNonQuery();
                }

            }

            //Read Service Bus Connection string and QueueName from app settings
            string serviceBusConnectionString = _appSettings.ServiceBusConnectionString;
            string queueName = _appSettings.ServiceBusQueueNameForSupplierResponse;

            //Create a service bus queue client using Service bus connection string and queue name
            IQueueClient sbQueueClient = new QueueClient(serviceBusConnectionString, queueName);

            //Create a Service Bus Message object that will be passed to queue : supplierResponses
            Message sbMessageForQueue = new Message(Encoding.UTF8.GetBytes("Your order id: " + orderId+", has been "+ status));
            sbMessageForQueue.ContentType = "application/json";

            //Send the message to service bus
            sbQueueClient.SendAsync(sbMessageForQueue);

            return "order status updated to buyer.";
        }


  
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
