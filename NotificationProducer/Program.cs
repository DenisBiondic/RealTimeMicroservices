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

            private const string NotificationChannel = "Notification-Channel";

            public static void Main()
            {
                  // Create pub/sub
                  var pubsub = connection.GetSubscriber();

                  while (true)
                  {
                      var notificationMessage = $"Notification at { DateTime.Now }";
                      pubsub.Publish(NotificationChannel, notificationMessage);
                      
                      Console.WriteLine(notificationMessage);

                      Thread.Sleep(TimeSpan.FromSeconds(3));
                  }
            }
      }
}