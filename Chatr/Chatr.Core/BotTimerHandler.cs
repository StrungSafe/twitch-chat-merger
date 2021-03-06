﻿namespace Chatr.Core
{
    using System;
    using System.Timers;

    using Chatr.Core.BotTimers;
    using Chatr.Core.Contracts;

    using TwitchLib.Client.Interfaces;

    public class BotTimerHandler : IDisposable
    {
        private readonly IBotTimer botTimer;

        private readonly string channel;

        private readonly ITwitchClient client;

        private bool disposed;

        private Timer timer;

        public BotTimerHandler(ITwitchClient client, string channel, BotTimerConfig config)
        {
            this.client = client;
            this.channel = channel;

            Type type = Type.GetType($"Chatr.Console.BotTimers.{config.Type}");
            botTimer = Activator.CreateInstance(type, config) as IBotTimer;

            timer = botTimer.Build();
            timer.Elapsed += OnTimeElapsed;
        }

        public void Dispose()
        {
            if (!disposed && timer != null)
            {
                timer.Dispose();
            }

            disposed = true;
            timer = null;
        }

        private void OnTimeElapsed(object sender, EventArgs e)
        {
            string command = string.Empty;
            if (botTimer is RandomTimer)
            {
                command = botTimer.OnTimeElapsed(() =>
                {
                    timer.Dispose();
                    timer = botTimer.Build();
                    timer.Elapsed += OnTimeElapsed;
                });
            }
            else
            {
                command = botTimer.OnTimeElapsed();
            }

            SendMessage(command);
        }

        private void SendMessage(string commandMessage)
        {
            client.SendMessage(channel, commandMessage);
        }
    }
}