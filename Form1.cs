using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Azure.Messaging.EventHubs.Consumer;

namespace SmartVendingMachine
{
    public partial class Form1 : Form
    {
        string product1 = "chocolatebar";
        string product2 = "popcorn";
        string product3 = "twix";
        string product4 = "gummybear";

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            
            itemOne.Text = product1;
            itemTwo.Text = product2;
            itemThree.Text = product3;
            itemFour.Text = product4;
            
            await ReceiveMessagesFromDeviceAsync();
        }

        private readonly static string connectionString = "Endpoint=sb://ihsuprodblres097dednamespace.servicebus.windows.net/;SharedAccessKeyName=service;SharedAccessKey=gNqSq6J7eegEm4ARlm2Uv/dIbK4QeE7xFGcVgJT1l8g=";
        private readonly static string EventHubName = "iothub-ehub-hellingahu-3461014-e34ca78374";

        
        private async Task ReceiveMessagesFromDeviceAsync()
        {
            // Create the event consumer using the default consumer group using a direct connection to the service. 
            await using EventHubConsumerClient consumer = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, connectionString, EventHubName);
            Console.WriteLine("Listening for messages on all partitions");


            // Begin reading events for all partitions, starting with the first event in each partition and waiting indefinitely for events to become available 
            await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync())
            {
                partitionEvent.Data.SystemProperties.TryGetValue("iothub-connection-device-id", out object deviceID);  // read event message from Event Hub partition 


                string device = "wchs001";
                if ((string)deviceID == device)
                {
                    string jsonData = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());

                    Product data = JsonConvert.DeserializeObject<Product>(jsonData);

                   
                    if (data.Name == itemOne.Text.ToLower())
                    {
                        stockOne.Text = data.Number.ToString();
                    }

                    if (data.Name == itemTwo.Text.ToLower())
                    {
                        stockTwo.Text = data.Number.ToString();
                    }

                    if (data.Name == itemThree.Text.ToLower())
                    {
                        stockThree.Text = data.Number.ToString();
                    }

                    if (data.Name == itemFour.Text.ToLower())
                    {
                        stockFour.Text = data.Number.ToString();
                    }
                    
                }
            }
        }
    }
}

