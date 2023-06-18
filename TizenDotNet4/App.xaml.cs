using ArangoDBNetStandard.Transport.Http;
using ArangoDBNetStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ArangoDBNetStandard.CollectionApi.Models;
using Tizen.Pims.Contacts.ContactsViews;
using ArangoDBNetStandard.AuthApi;
using ArangoDBNetStandard.DatabaseApi;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using ArangoDBNetStandard.DocumentApi.Models;
using System.Threading;
using System.Reflection.Metadata;
using Document = TizenDotNet4.components.Document;
using System.Xml.Linq;
using Tizen.Wearable.CircularUI.Forms;
using TizenDotNet4;

namespace TizenDotNet4
{
    /// <summary>
    /// Represents the application class.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public static ArangoDBClient DatabaseNew;
        public Document document;
        public string collectionName;

        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            InitializeComponent();
            // Wrap the LandingPage with a NavigationPage
            MainPage = new NavigationPage(new LandingPage());
            //document = new Document();
            //collectionName = "firstCollection";
            //InitializeArangoDB();

            //MainPage = new MainPage();
            //MainPage = new TizenDotNet4.MainPage();
            MainPage = new LandingPage();
            MainPage = new TizenDotNet4.LandingPage();
#if DEBUG
            TizenHotReloader.HotReloader.Open(this);
#endif
        }

        /// <summary>
        /// Initializes ArangoDB connection. This is the working alternative of interacting with ArangoDB. It has its limitation
        /// in .NET. Therefore the best practise is recommended, ie using of a API endpoint of backend NodeJS server which is
        /// being used in this project.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async void InitializeArangoDB()
        {

            //The endpoint of your deployment Now
            string endpoint = "https://92ac4680fb78.arangodb.cloud:18529";
            //Root password
            string rootPassword = "yMfTVSAyJhqzjII2jtwB";
            //Base64 encoded CA certificate
            string encodedCA = "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSURHRENDQWdDZ0F3SUJBZ0lRWGxzQzZwU0laUlRBbmpGak1XM2R6ekFOQmdrcWhraUc5dzBCQVFzRkFEQW0KTVJFd0R3WURWUVFLRXdoQmNtRnVaMjlFUWpFUk1BOEdBMVVFQXhNSVFYSmhibWR2UkVJd0hoY05Nak13TmpFMApNVEExTURJMFdoY05Namd3TmpFeU1UQTFNREkwV2pBbU1SRXdEd1lEVlFRS0V3aEJjbUZ1WjI5RVFqRVJNQThHCkExVUVBeE1JUVhKaGJtZHZSRUl3Z2dFaU1BMEdDU3FHU0liM0RRRUJBUVVBQTRJQkR3QXdnZ0VLQW9JQkFRQ3oKMDgybGd4ZUpqcGNFbVU1WnltMmZpWS9DaWU1YkRLUG83TTlXbk5KOGxXTnlXR1NPQUFhRmlHMU1wRytTTWw2bgpzR0xDSVpOb2duRXZyVmo1T0ttZ3IvUGZtZkpTVVo2ZWUxbVgzbWhqbGhOdDhjd3ZhamhDNm9IT2lCQWRxQXptClhPSTZtV2tGVU1tWjVPUFNkd08xYTZtMkNLeHluTEhhQkRpeWFOSXpTN29YdGpZaWgzR0FxMWRnZUU5NGRhV0MKN2IvWmhsN0VsZ0c0UHp4aENJdnBzYlFsRmFVMWYrRDFhcTZteUw4Y1F6MjZ6TnVROUtSZmdCUGVZYXV1Z0NhMApLQVE1dXBFSGdweHErc2Ntam13cjFTNTRoQ2MzUGIyNG45WkJwTkxuRE9SZThpSnR0dGpJa3NTblFRbVNIZjdXCjQ4U3BUZlg4NERXd0tJUkltNmtCQWdNQkFBR2pRakJBTUE0R0ExVWREd0VCL3dRRUF3SUNwREFQQmdOVkhSTUIKQWY4RUJUQURBUUgvTUIwR0ExVWREZ1FXQkJRRURodmJSRDRsWlN0VkJ6UVdzU2dHSWNGeUx6QU5CZ2txaGtpRwo5dzBCQVFzRkFBT0NBUUVBZ1owMlh0RUN0OHRNT0xic21yTllQWGZJOWNLS3dpM0JiT2NxSDNhb2hGSWdZWkZwCmYzdkdUaEhmVWVzY092T0VYbVgrR1k5UFQ4MUpqSXNUUGhtWTlqQ3dTaE14Zm83SU1NOUpSMHFGNHQ5RE0zTWkKZ0ZqaU9JL0NFc3JhMTU3VXgyN1hRNitRaHIrSms5Y1dxdUViOU12QzNsdWt2R0VvSkFSMmNCU2tKdEZDMnFlMwptbUducDJMU2pOMDI2RFlmclNiT3cwYXhrcEpQR0F1SjgyUm42RFRBREV2RGE0OGgwUWtsT3hFOFZubEVmZi8yCmhFV0JQSzZJSW5iWnBQNk5jTzNHWjVOTG1JY1l4WTkvK0poWG1HbEM3cE5QSUFIZjdjNlVuWkEwSk52NHlBSTYKN25OR0VuU2wwb0p0Sy83cm1qMTAwdG5IYjBhNUYrbm1nMkFISmc9PQotLS0tLUVORCBDRVJUSUZJQ0FURS0tLS0tCg==";

            //Initialize the http handler to use the certificate
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12;
            handler.ClientCertificates.Add(new X509Certificate2(Convert.FromBase64String(encodedCA)));

            //Initialize the http client with the http handler
            var client = new HttpClient(handler);
            DatabaseNew = new ArangoDBClient(client);
            //Setup the client to use basic authentication
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"root:{rootPassword}")));
            //Setup the client's default content type
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //Setup the client's base address
            client.BaseAddress = new Uri($"{endpoint}/_db/_system/");

            // Connect to the database using the HttpClient and run operations
            using (DatabaseNew = new ArangoDBClient(client))
            {
                // Note that ArangoGraph Insights Platform runs deployments in a cluster configuration.
                // To achieve the best possible availability, your client application has to handle
                // connection failures by retrying operations if needed.

                //Test call
                var dbInfo = await DatabaseNew.Database.GetCurrentDatabaseInfoAsync();
                if (dbInfo.Code == System.Net.HttpStatusCode.OK && !dbInfo.Error)
                {
                    //Success... Display the return value
                    Console.WriteLine($"Database info: Name: {dbInfo.Result.Name}");

                    // Create document in the collection using strong type
                    await DatabaseNew.Document.PostDocumentAsync(
                        collectionName,
                        document);
                    //var document = new Document { Name = "New", Age = 2 };
                    await CreateDocument();

                    //Retrieve documents from the collection

                    document = await GetDocument<Document>("firstCollection");
                    Console.WriteLine(document.ProcessStep);
                }
                else
                {
                    //Call failed
                    Console.WriteLine("GetCurrentDatabaseInfo failed.");
                }
            }

        }

        /// <summary>
        /// Creates a document in the ArangoDB.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateDocument()
        {
            var document = new Document { ProcessStep = "Step 7", MeasuredValue = "30" };
            const string collectionName = "firstCollection";

            try
            {
                // Create document in the collection using strong type
                var response = await DatabaseNew.Document.PostDocumentAsync(
                    collectionName,
                    document);

                Console.WriteLine("Document created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// Gets the documents asynchronously.
        /// </summary>
        /// <param name="collectionName">The name of ArangoDB's collection.</param>
        /// <returns>A document.</returns>
        public async Task<T> GetDocumentAsyncWrap<T>(string collectionName)
        {
            const string documentKey = "8134499";
            // Call the GetDocumentsAsync method of ArangoDBClient
            var documents = await DatabaseNew.Document.GetDocumentAsync<T>(collectionName, documentKey);

            // Return the list of documents
            return documents;
        }

        /// <summary>
        /// Gets the documents asynchronously.
        /// </summary>
        /// <param name="collectionName">The name of ArangoDB's collection.</param>
        /// <returns>A document.</returns>
        public async Task<Document> GetDocument<T>(string collectionName)
        {
            // Retrieve the document from the collection
            var result = await GetDocumentAsyncWrap<T>(collectionName);

            // Assign the result to the document variable if it is of type TizenDotNet1.components.Document
            if (result is Document document)
            {
                this.document = document;
                return this.document;
            }
            return null;
        }


        /// <summary>
        /// Called when the application starts.
        /// </summary>
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        /// <summary>
        /// Called when the application is put to sleep.
        /// </summary>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        /// <summary>
        /// Called when the application resumes from a sleeping state.
        /// </summary>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}

