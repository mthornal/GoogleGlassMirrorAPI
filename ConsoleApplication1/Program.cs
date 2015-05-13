using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Mirror.v1;
using Google.Apis.Mirror.v1.Data;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Books.ListMyLibrary
{
    /// <summary>
    /// Sample which demonstrates how to use the Books API.
    /// https://code.google.com/apis/books/docs/v1/getting_started.html
    /// <summary>
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Mirror API Sample");
            Console.WriteLine("================================");
            try
            {
                new Program().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task Run()
        {
            UserCredential credential;
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "610749358230-9h3a8bhkciihni9bp2dn82qubd94olrb.apps.googleusercontent.com",
                    ClientSecret = ""
                },
                new[] { MirrorService.Scope.GlassTimeline, MirrorService.Scope.GlassLocation, Oauth2Service.Scope.UserinfoProfile },
                "user",
                CancellationToken.None,
                new FileDataStore("Mirror.Sample"));

            // Create the service.
            var service = new MirrorService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Mirror API Sample",
                });

            //await service.Timeline.Insert(new TimelineItem
            //{
            //    Text = "Hello from .NET again"
            //}).ExecuteAsync();           

            await service.Contacts.Insert(new Contact
            {
                Id = Guid.NewGuid().ToString(),

            }).ExecuteAsync();

            await service.Subscriptions.Insert(new Subscription
            {
                Collection = "timeline",
                UserToken = "",
                CallbackUrl = ""
            }).ExecuteAsync();

            var timelineItems = await service.Timeline.List().ExecuteAsync();

            var contacts = await service.Contacts.List().ExecuteAsync();

            var subscriptions = await service.Subscriptions.List().ExecuteAsync();

            var locations = await service.Locations.List().ExecuteAsync();
        }
    }
}