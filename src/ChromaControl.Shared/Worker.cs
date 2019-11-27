using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Razer.Chroma.Broadcast;
using RGBKit.Core;
using ChromaControl.Shared;

namespace ChromaControl
{
    /// <summary>
    /// The Chroma Control worker
    /// </summary>
    public class Worker : BackgroundService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<Worker> _logger;

        /// <summary>
        /// The RGB Kit service
        /// </summary>
        private readonly IRGBKitService _rgbKit;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The razer broadcast api
        /// </summary>
        private readonly RzChromaBroadcastAPI _api;

        /// <summary>
        /// Creates the worker
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="rgbKit">The RGB Kit service</param>
        /// <param name="configuration">The configuration</param>
        public Worker(ILogger<Worker> logger, IRGBKitService rgbKit, IConfiguration configuration)
        {
            _logger = logger;
            _rgbKit = rgbKit;
            _configuration = configuration;
            _api = new RzChromaBroadcastAPI();
            _api.ConnectionChanged += Api_ConnectionChanged;
            _api.ColorChanged += Api_ColorChanged;
            
        }

        /// <summary>
        /// Executes the worker
        /// </summary>
        /// <param name="stoppingToken">The stopping token</param>
        /// <returns>A task</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);

            _logger.LogInformation($"Initializing {Utilities.ApplicationId}...");

            await Task.Delay(1000, stoppingToken);

            _rgbKit.Initialize();

            _api.Init(Guid.Parse(Utilities.ApplicationGuid));

            foreach (var provider in _rgbKit.DeviceProviders)
            {
                foreach (var device in provider.Devices)
                {
                    _logger.LogInformation($"Found Device: {provider.Name} - {device.Name}");
                }
            }

            _logger.LogInformation($"{Utilities.ApplicationId} started successfully!");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// Occurs when the connection status to the Razer Chroma Broadcast API changes
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The arguments</param>
        private void Api_ConnectionChanged(object sender, RzChromaBroadcastConnectionChangedEventArgs e)
        {
            _logger.LogInformation(e.Connected ? "Razer Chroma Broadcast API connected" : "Razer Chroma Broadcast API disconnected");
        }

        /// <summary>
        /// Occurs when the connection status to the Razer Chroma Broadcast API changes
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The arguments</param>
        private void Api_ColorChanged(object sender, RzChromaBroadcastColorChangedEventArgs e)
        {
            var currentColor = 0;

            foreach (var deviceProvider in _rgbKit.DeviceProviders)
            {
                foreach (var device in deviceProvider.Devices)
                {
                    foreach (var light in device.Lights)
                    {
                        light.Color = e.Colors[currentColor];
                        currentColor++;

                        if (currentColor == e.Colors.Length)
                            currentColor = 0;
                    }

                    device.ApplyLights();
                }
            }
        }
    }
}
