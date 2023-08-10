using Discord.WebSocket;
using Discord;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using DotNetEnv.Configuration;
using Discord.Net;
using Newtonsoft.Json;

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

        _client.Ready += Client_Ready;

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public async Task Client_Ready()
    {
        // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
        var guild = _client.GetGuild(1110416331070771200);

        // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
        var guildCommand = new SlashCommandBuilder();

        // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
        guildCommand.WithName("based");

        // Descriptions can have a max length of 100.
        guildCommand.WithDescription("Returns a random quote from the New Testament.");

        try
        {
            // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
            await guild.CreateApplicationCommandAsync(guildCommand.Build());

        }
        catch (ApplicationCommandException exception)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        }
    }
}