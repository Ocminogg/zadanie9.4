using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InputFiles;

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
    
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        //await HandleMessage(botClient, update.Message, cancellationToken);
        return;
    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        await HandleMessage(botClient, update.Message, cancellationToken);
        return;
    }
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    

}

async Task HandleMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
{
    if (message.Text == "/start")
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Choose commands: /inline | /keyboard");
        return;
    }

    if (message.Text == "/keyboard")
    {
        ReplyKeyboardMarkup keyboard = new(new[]
        {
            new KeyboardButton[] {"Hello", "Салам"},
            new KeyboardButton[] {"Привет", "Прощай дворф" }
        })
        {
            ResizeKeyboard = true
        };
        await botClient.SendTextMessageAsync(message.Chat.Id, "Choose:", replyMarkup: keyboard);
        return;
    }

    List<string> mes = new List<string>() { "Hello", "Hi", "Салам", "Привет", "Здравствуй", "Здравствуйте", "Прощай дворф" };
    foreach (var me in mes)
    {
        if (me == message.Text)
        {
            if (message.Text == "Салам")
            {
                // Echo received message text
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Ас-саляму алейкум\n",
                    cancellationToken: cancellationToken);
                break;
            }
            if (message.Text == "Прощай дворф")
            {
                // Echo received message text
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Прощай, хорошего денёчка)\n",
                    cancellationToken: cancellationToken);

                Message sentVideo = await botClient.SendVideoAsync(
                chatId: message.Chat.Id,
                video: new InputOnlineFile ("https://gogetvideo.net/index.php?output=yt/XGXYPDXTfn4/128%7e%7e1%7e%7e%D0%9F%D1%80%D0%BE%D1%89%D0%B0%D0%B9%D0%94%D0%B2%D0%BE%D1%80%D1%84_uuid-62ffa994ed671.mp4"),
                supportsStreaming: true,
                cancellationToken: cancellationToken);
                break;
            }
            else
            {
                // Echo received message text
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Здравствуй\n",
                    cancellationToken: cancellationToken);
                break;
            }
        }
    }

    if (message.Text == "We")
    {
        Message messageMusic = await botClient.SendAudioAsync(
        chatId: message.Chat.Id,
        audio: "https://minty.club/artist/daft-punk/get-lucky-feat-pharrell-williams-and-nile-rodgers/daft-punk-get-lucky-feat-pharrell-williams-and-nile-rodgers.mp3",

        cancellationToken: cancellationToken);
    }


    if (message.Text == "Отправь фото")
    {
        Message messagePhoto = await botClient.SendPhotoAsync(
        chatId: message.Chat.Id,
        photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
        caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
        parseMode: ParseMode.Html,
        cancellationToken: cancellationToken);
    }


    //await botClient.SendTextMessageAsync(message.Chat.Id, $"You said:\n{message.Text}");
}
/////////////////////////////////////////////////////////



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