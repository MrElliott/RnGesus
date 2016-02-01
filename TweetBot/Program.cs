using System;
using System.Net;
using TweetSharp;

namespace TweetBot
{
    class Program
    {
        private const string ConsumerKey = "eo8O4yXKhSi2gqoTsSxCEgIRg";
        private const string ConsumerSecret = "lGAQ0FWjoXB1VeHp4gBBFkQ1UV2ctbAbDCwLmMQaLidGhwXlyW";

        private const string AccessToken = "4869317847-UW6uY8pJlp6XHoqiQno12a2CywKtgDiOpH753u1";
        private const string AccessTokenSecret = "rQDLje7UcAJmKAVyuu9sWG32A2R2NQ6JWTypDqf5xfobo";

        // Timespan based on minutes
        const double ResetTime = 5;

        static void Main(string[] args)
        {
            // Create the random number generater
            Random rng = new Random();
            // Pull down the twitter service 
            TwitterService service = new TwitterService(ConsumerKey, ConsumerSecret);
            // Authenticate the service
            service.AuthenticateWith(AccessToken, AccessTokenSecret);
            
            // Send a tweet based on the RNG value
            SendTweet(service, rng.Next().ToString());

            bool exit = false;
            
            TimeSpan time = TimeSpan.FromSeconds(0);
            DateTime lastUpdate = DateTime.Now;

            while (!exit)
            {
                TimeSpan sincelast = TimeSpan.FromTicks(DateTime.Now.Ticks - lastUpdate.Ticks);

                time -= sincelast;

                lastUpdate = DateTime.Now;

                if (time <= TimeSpan.Zero)
                {
                    time = TimeSpan.FromMinutes(ResetTime);

                    SendTweet(service, rng.Next().ToString());
                }
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

        private static void ListPreviousTweets(TwitterService service)
        {
            var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());

            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
            }
        }
    }
}
