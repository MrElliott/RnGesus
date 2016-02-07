using System;
using System.Net;
using TweetSharp;

namespace TweetBot
{
    class Program
    {
        private const string CONSUMER_KEY = "eo8O4yXKhSi2gqoTsSxCEgIRg";
        private const string CONSUMER_SECRET = "lGAQ0FWjoXB1VeHp4gBBFkQ1UV2ctbAbDCwLmMQaLidGhwXlyW";

        private const string ACCESS_TOKEN = "4869317847-UW6uY8pJlp6XHoqiQno12a2CywKtgDiOpH753u1";
        private const string ACCESS_TOKEN_SECRET = "rQDLje7UcAJmKAVyuu9sWG32A2R2NQ6JWTypDqf5xfobo";

        // Timespan based on minutes
        const double RESET_TIME = 5;

        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            // Create the random number generater
            Random rng = new Random();
            // Pull down the twitter service 
            TwitterService service = new TwitterService(CONSUMER_KEY, CONSUMER_SECRET);
            // Authenticate the service
            service.AuthenticateWith(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);

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
