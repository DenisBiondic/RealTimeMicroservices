using System;
using BackendForFrontend.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace BackendForFrontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .AllowAnyOrigin();
            }));

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/hubs/notificationhub");
            });

            var notificationHub = app.ApplicationServices.GetRequiredService<IHubContext<NotificationHub>>();
            SubscribeToRedisChannelAndBroadcaast(notificationHub);
        }

        private void SubscribeToRedisChannelAndBroadcaast(IHubContext<NotificationHub> notificationHub)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis");

            ISubscriber sub = redis.GetSubscriber();

            sub.Subscribe("Notification-Channel", async (channel, info) =>
            {
                IDatabase db = redis.GetDatabase();
                var message = db.ListRightPop("Notification-List");

                if (message != RedisValue.Null)
                {
                    await notificationHub.Clients.All.SendAsync("NewNotification", message.ToString());
                    Console.WriteLine($"Forwarded message: { message }");
                }
            });
        }
    }
}
