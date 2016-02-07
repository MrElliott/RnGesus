using System;
using System.Net;
using System.Xml.Linq;
using TweetSharp;

namespace TweetBot
{
    class Program
    {
        private static string _ConsumerKey = "";
        private static string _ConsumerSecret = "";

        private static string _AccessToken = "";
        private static string _AccessTokenSecret = "";

        // Timespan based on minutes
        public const double RESET_TIME = 5;

        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            // Create the random number generater
            Random rng = new Random();

            // Pull the User settings from XML
            XDocument userSettings = XDocument.Load("Content/UserSettings.xml");

            SetUserDetails(userSettings);

            // Pull down the twitter service 
            TwitterService service = new TwitterService(_ConsumerKey, _ConsumerSecret);
            // Authenticate the service
            service.AuthenticateWith(_AccessToken, _AccessTokenSecret);
            
            TimeSpan time = TimeSpan.FromSeconds(0);
            DateTime lastUpdate = DateTime.Now;

            while (true)
            {
                TimeSpan sincelast = TimeSpan.FromTicks(DateTime.Now.Ticks - lastUpdate.Ticks);

                time -= sincelast;

                lastUpdate = DateTime.Now;

                if (time <= TimeSpan.Zero)
                {
                    time = TimeSpan.FromMinutes(RESET_TIME);

                    SendTweet(service, rng.Next().ToString());
                }
            }
            
            // ReSharper disable once FunctionNeverReturns
        }

        /// <summary>
        /// Settings up the users settings my searching through the XML for specific 
        /// </summary>
        /// <param name="settings"></param>
        private static void SetUserDetails(XDocument settings)
        {
            foreach (var descendant in settings.Descendants("ConsumerKey"))
            {
                _ConsumerKey = descendant.Value;
            }

            foreach (var descendant in settings.Descendants("ConsumerSecret"))
            {
                _ConsumerSecret = descendant.Value;
            }

            foreach (var descendant in settings.Descendants("AccessToken"))
            {
                _AccessToken = descendant.Value;
            }

            foreach (var descendant in settings.Descendants("AccessTokenSecret"))
            {
                _AccessTokenSecret = descendant.Value;
            }
        }

        private static void SendTweet(TwitterService service, string message)
        {
            service.SendTweet(new SendTweetOptions { Status = message }, (tweet, response) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Tweet is Good");
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            });

            //var serviceResponse = service.Response.Response;
        }

        public static void ListPreviousTweets(TwitterService service)
        {
            var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());

            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
            }
        }
    }
}
