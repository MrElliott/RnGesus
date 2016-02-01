using System;
using TweetSharp;

namespace TweetBot
{
    class Program
    {
        private const string ConsumerKey = "eo8O4yXKhSi2gqoTsSxCEgIRg";
        private const string ConsumerSecret = "lGAQ0FWjoXB1VeHp4gBBFkQ1UV2ctbAbDCwLmMQaLidGhwXlyW";

        private const string AccessToken = "4869317847-UW6uY8pJlp6XHoqiQno12a2CywKtgDiOpH753u1";
        private const string AccessTokenSecret = "rQDLje7UcAJmKAVyuu9sWG32A2R2NQ6JWTypDqf5xfobo";

        static void Main(string[] args)
        {

            var service = new TwitterService(ConsumerKey, ConsumerSecret);
            service.AuthenticateWith(AccessToken, AccessTokenSecret);

            var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
            }

            Console.WriteLine("Application Complete");
            Console.WriteLine("Press 'E' to Exite");

            string exit = Console.ReadLine();

            while (exit.ToLower() == "e")
            {
                exit = Console.ReadLine();
            }
        }
    }
}
