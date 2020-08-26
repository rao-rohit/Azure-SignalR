<b>Introduction:</b>
 
In this article, I walk you through "Azure SignalR Service" using Buyer-Supplier sample application. Azure SignalR service provides a real time communIcation between your applicatrion/pages.
 
We will cover couple of more Azure Services like Azure Functions, Azure Service Bus & Azure SQL Database.
We will have one Buyer application in Asp.net core Razor Forms and ONE Supplier application in Asp.net core MVC.

 
<b>Prerequisites:</b>
Azure Account for setting up Azure Services (SignalR, Functions, Service Bus etc)
Visual Studio 2019 (For buyer and supplier application)

<b>Application Flow:</b>   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>Buyer Application</i> - Buyer will create an order for an item with quantity. <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>Supplier Application</i> - Supplier will get notified (A prompt message on his screen) that a buyer has added a new order.<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>Supplier Application</i> - Supplier will approve or reject the buyer orders.<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>Buyer Application</i> - Buyer will get notified on his screen that supplier has approved/rejected his order.<br/>
       
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;The notification(Buyer/Supplier) will get triggered by Azure SignalR Service. It means we will be registerting our application/pages to the SignalR service<br/>
 
<b>Project Structure: </b>


 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1. Entities - Contains a class "ActiveOrder" which is used for storing the active orders data from UI and Database
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2. Common - Here we have AppSettings.cs class, it used for storing the appSettings values from app.config files in #5 UI projects  <br/> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3. DB - DB Schema and Stored Procedures used in application  <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;4. OrderPRocessingFunctionApp - Azure Function application  <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;5. UI - Buyer and Supplier - Client applications, one is for buyer and one is for supplier  <br/>

<b>Design/Flow:</b>
 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Buyer: </b>
           
 
 
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Supplier:</b>
 
 
 
