using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tizen.Network.Connection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using ArangoDB.client;
using TizenDotNet4.components; // Add the namespace where the Document class is defined
using MQTTnet;
using MQTTnet.Client;
using System.Threading;
using Tizen.NUI.BaseComponents;

namespace TizenDotNet4
{
    /// <summary>
    /// Represents the main page of the application.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        // <summary>
        /// Initializes a new instance of the MainPage class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            //ConnectAndPublish();
            //ConnectAndSubscribe();
        }

        // <summary>
        /// Event handler for the appearing event of the page.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            FetchDataFromAPI(null, EventArgs.Empty);
        }

        /// <summary>
        /// Fetches data from the API.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private async void FetchDataFromAPI(object sender, EventArgs e)
        {
            // Call the GetDocumentsAsync method to retrieve documents from ArangoDB
            var documents = await GetDocumentsAsync();
            Console.WriteLine(documents);

            // Process the retrieved documents as needed
            if (documents != null && documents.Any())
            {
                // Get the first document
                var firstDocument = documents.First();
                Console.WriteLine(firstDocument.ToString());

                // Access the "value" property of the document
                if (firstDocument.ProcessStep != "")
                {
                    var step = firstDocument.ProcessStep;
                    var value = firstDocument.MeasuredValue;

                    // Update the label text with the retrieved value
                    stepLabel.Text = step;
                    valueLabel.Text = value;
                    Console.WriteLine(value.ToString());
                }
            }
        }

        /// <summary>
        /// Gets the documents asynchronously.
        /// </summary>
        /// <returns>A list of documents.</returns>
        private async Task<List<Document>> GetDocumentsAsync()
        {
            try
            {

                using (var httpClient = new HttpClient())
                {

                    var response = await httpClient.GetAsync("https://smart-watch-app-node-js-server-cs1n-qavvof82o-lodhik9.vercel.app/documents");

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as JSON
                        var json = await response.Content.ReadAsStringAsync();


                        // Deserialize the JSON to a list of dictionaries
                        var documentData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, Document>>>(json);

                        // Extract the "document" objects from the response
                        var documents = documentData.Select(item => item["document"]).ToList();

                        return documents;
                    }
                    else
                    {
                        Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Connects and subscribes to an MQTT broker.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ConnectAndSubscribe()
        {
            try
            {
                var mqttFactory = new MqttFactory();

                using (var mqttClient = mqttFactory.CreateMqttClient())
                {
                    var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("test.mosquitto.org", 1883).Build();

                    mqttClient.ApplicationMessageReceivedAsync += async e =>
                    {
                        Console.WriteLine($"Received application message. - {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                        await Task.CompletedTask;
                    };

                    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                    var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                        .WithTopicFilter(f => f.WithTopic("/Lodhi"))
                        .Build();

                    await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                    // Disconnect the client
                    await mqttClient.DisconnectAsync();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

            }
        }

        // <summary>
        /// Connects and Publish to an MQTT broker.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ConnectAndPublish()
        {
            /*
             * This sample creates a simple MQTT client and connects to a public broker with enabled TLS encryption.
             * 
             * This is a modified version of the sample _Connect_Client_! See other sample for more details.
             */

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("test.mosquitto.org", 8883)
                    //.WithTls(
                    //    o =>
                    //    {
                    //        // The used public broker sometimes has invalid certificates. This sample accepts all
                    //        // certificates. This should not be used in live environments.
                    //        o.CertificateValidationHandler = _ => true;
                    //    })
                    .Build();

                // In MQTTv5 the response contains much more information.
                using (var timeout = new CancellationTokenSource(5000))
                {
                    // Establish connection using ConnectAsync
                    var response = await mqttClient.ConnectAsync(mqttClientOptions, timeout.Token);

                    // Publishing this msg to the broker
                    Console.WriteLine("The MQTT client is connected.");

                    Console.ReadLine();

                    // Publish the message
                    await PublishMessageAsync(mqttClient);

                    // Disconnect the client
                    await mqttClient.DisconnectAsync();

                    Console.WriteLine("MQTT application message is published.");
                }
            }
        }

        public static async Task PublishMessageAsync(IMqttClient client)
        {
            string messagePayload = "hello";
            // Build the message
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/Lodhi")
                .WithPayload(messagePayload)
                .Build();

            // Check for publishing
            if (client.IsConnected)
            {
                await client.PublishAsync(message);
            }
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to LandingPage.xaml
            Navigation.PopModalAsync();
        }
    }
}
