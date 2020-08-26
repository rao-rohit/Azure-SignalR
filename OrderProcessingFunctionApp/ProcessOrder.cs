using System;
using System.Data.SqlTypes;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Configuration;

namespace OrderProcessingFunctionApp
{
    public static class ProcessOrder
    {

        #region New Order Submitted
        /// <summary>
        /// Azure function to execute when an item will get added in Service bus queue
        /// </summary>
        /// <param name="order">new order details</param>
        /// <param name="log"></param>
        [FunctionName("NewOrderSubmitted")]
        public static void NewOrderSubmitted([ServiceBusTrigger("buyerOrders", Connection = "ServiceBusConnectionString")]  ActiveOrder order,
            [SignalR(HubName ="notifications")] IAsyncCollector<SignalRMessage> signalRMessage ,ILogger log)
        {
            
            log.LogInformation($"Saving Order Details to Database -  Item Name:{order.ItemName}, Buyer Name:{order.BuyerName}");
            SaveToDataBase(order);

            log.LogInformation($"Passing order details to SignalR -  Item Name:{order.ItemName}, Buyer Name:{order.BuyerName}");
            signalRMessage.AddAsync(new SignalRMessage
            {
                Target = "newOrderSubmitted",
                Arguments = new[] { order }
            });
        }

        private static void SaveToDataBase(ActiveOrder order)
        {

            using SqlConnection con = new SqlConnection(Environment.GetEnvironmentVariable("SQLConnectionString"));
            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "usp_buyer_order_insert";
                cmd.Parameters.AddWithValue("@BuyerName", order.BuyerName);
                cmd.Parameters.AddWithValue("@ItemName", order.ItemName);
                cmd.Parameters.AddWithValue("@Quantity", order.Quantity);

                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Supplier Response to a buyer order


        /// <summary>
        /// Azure function to execute when an response from supplier has been submitted for an requested item by buyer
        /// </summary>
        /// <param name="message">message to buyer - Supplier arroved his request or rejected it</param>
        /// <param name="log"></param>
        [FunctionName("SupplierResponse")]
        public static void SupplierResponse([ServiceBusTrigger("supplierResponses", Connection = "ServiceBusConnectionString")] string  message,
            [SignalR(HubName = "notifications")] IAsyncCollector<SignalRMessage> signalRMessage, ILogger log)
        {
            log.LogInformation($"Sending supplier responses to buyer: {message}");
            signalRMessage.AddAsync(new SignalRMessage
            {
                Target = "supplierresponse",
                Arguments = new[] { message }
            });
        }


        #endregion

        #region Negotiate
        /// <summary>
        /// Negotiation - This will talk to azure SignalR service and provides a API key that will be used in further communication in sending messages.
        /// It handshake with Azure SignalR service and sends us the connection information that we will use in further communications.
        /// It will get called when we load the Buyer or Supplier application - On document.ready event.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="connectionInfo"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
       [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
       [SignalRConnectionInfo(HubName = "notifications")] SignalRConnectionInfo connectionInfo, ILogger log)
        {
            log.LogInformation(connectionInfo.Url + ",," + connectionInfo.AccessToken);
           
            return connectionInfo;
        }
        #endregion
    }
}
