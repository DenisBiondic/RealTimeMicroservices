using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationProducer
{
      public static class Program
      {
            private const string RedisConnectionString = "redis";

            private static ConnectionMultiplexer connection =
              ConnectionMultiplexer.Connect(RedisConnectionString);

            private const string ListName = "Notification-List";
            private const string ChannelName = "Notification-Channel";

            public static void Main()
            {
                  var db = connection.GetDatabase();
                  var subscription = connection.GetSubscriber();

                  while (true)
                  {
                      var notificationMessage = $"Notification at { DateTime.Now }";
                      
                      // put the message into the "queue" simulated by the list
                      db.ListLeftPush(ListName, notificationMessage);
                  
                      // notify possible subscribers that there is a new message to process (so they don't have to poll)
                      subscription.Publish(ChannelName, "There is a new message in the list. If you are " +
                        "the lucky consumer instance, you might have the honour to proccess it!");

                      Console.WriteLine(notificationMessage);

                      Thread.Sleep(TimeSpan.FromSeconds(3));
                  }
            }
      }
}