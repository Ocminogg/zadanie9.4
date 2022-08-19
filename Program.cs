using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;

var botClient = new TelegramBotClient("5560152751:AAExhnTdlWOYWWBWoxRbDZrOubywSxMfnic");

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();



async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    List<string> mes = new List<string>() { "Hello", "Hi", "Салам", "Привет", "Здравствуй", "Здравствуйте" };
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    foreach (var me in mes)
    {
        if (me == messageText)
        {
            if (messageText == "Салам")
            {
                // Echo received message text
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Ас-саляму алейкум\n",
                    cancellationToken: cancellationToken);
                break;
            }
            else
            {
                // Echo received message text
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Здравствуй\n",
                    cancellationToken: cancellationToken);
                break;
            }
        }
    }
    if (messageText == "We")
    {
        Message messageMusic = await botClient.SendAudioAsync(
        chatId: chatId,
        audio: "https://minty.club/artist/daft-punk/get-lucky-feat-pharrell-williams-and-nile-rodgers/daft-punk-get-lucky-feat-pharrell-williams-and-nile-rodgers.mp3",

        cancellationToken: cancellationToken);
    }


    if (messageText == "Отправь фото")
    {
        Message messagePhoto = await botClient.SendPhotoAsync(
        chatId: chatId,
        photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
        caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
        parseMode: ParseMode.Html,
        cancellationToken: cancellationToken);
    }

}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

namespace zadanie9nout
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

}