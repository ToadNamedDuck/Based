﻿using Discord.WebSocket;
using Discord;
using System.Threading.Tasks;
using System;
using Discord.Net;
using Newtonsoft.Json;
using Based.ExternalManagers;

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
        _client.SlashCommandExecuted += SlashCommandHandler;

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

        //make a second guild command for a different discord server (guild)
        var guild2 = _client.GetGuild(1068364448978444338);
        var based2 = new SlashCommandBuilder();
        based2.WithName("based");
        based2.WithDescription("Returns a random quote from the New Testament.");

        var guild3 = _client.GetGuild(931983109602308157);//Realized I could just make other guilds, probably using the creation hook, and apply a list of commands to them
        //as they come in.

        try
        {
            // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
            await guild.CreateApplicationCommandAsync(guildCommand.Build());
            await guild2.CreateApplicationCommandAsync(based2.Build());
            await guild3.CreateApplicationCommandAsync(based2.Build());
        }
        catch (ApplicationCommandException exception)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        }
    }

    public async Task SlashCommandHandler(SocketSlashCommand cmd)
    {
        await cmd.DeferAsync();
        var bibleGetter = new BibleVerseGetter();
        var msgToSend = await bibleGetter.GetRandomVerse();
        await cmd.ModifyOriginalResponseAsync(m => m.Content = msgToSend.t);

    }
}