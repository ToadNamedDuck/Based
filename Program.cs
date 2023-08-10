using Discord.WebSocket;
using Discord;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using DotNetEnv.Configuration;

public class Program
{
    private DiscordSocketClient _client;

    public static async Task Main(string[] args)
    {
        DotNetEnv.Env.TraversePath().Load();

        await Task.Run(() => new Program().MainAsync().GetAwaiter().GetResult());
    }
  

    public async Task MainAsync()
    {

        _client = new DiscordSocketClient();
        _client.Log += Log;
        await _client.LoginAsync(TokenType.Bot,
            Environment.GetEnvironmentVariable("DiscordToken"));
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}