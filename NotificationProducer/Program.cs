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
                      pubsub.Publish(NotificationChannel, $"Message at { DateTime.Now }");
                      Thread.Sleep(TimeSpan.FromSeconds(3));
                  }
            }
      }
}