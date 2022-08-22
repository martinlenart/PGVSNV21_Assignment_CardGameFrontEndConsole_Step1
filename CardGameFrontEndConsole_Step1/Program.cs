using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CardGameFrontEndConsole.Models;
using CardGameFrontEndConsole.Services;

namespace SeidoDbWebApiConsumer
{
    class Program
    {
        static void Main(string[] args)
        {

            //running from local host
            var ServiceUri = "https://localhost:7049".Split(',');

            foreach (var su in ServiceUri)
            {
                var service = new CardGameHttpService(new Uri(su));
                Console.WriteLine($"\nConnecting to WebApi: {su}");
                PlayGameAsync(service).Wait();
            }
        }


        private static async Task PlayGameAsync(CardGameHttpService service)
        {
            Console.WriteLine("Testing CardGameWebApi");
            Console.WriteLine("------------");

            Console.WriteLine("Starting a game");
            var responseStart = await service.StartGame("highcard");
            Console.WriteLine($"response: {responseStart}");

            Console.WriteLine("\nGet a card");
            var card = await service.DealCard();
            Console.WriteLine(card);

            Console.WriteLine("\nGet 5 cards");
            var cards = await service.DealCards(5);
            Console.WriteLine(cards);

            Console.WriteLine("\nGet WinningCards");
            var winners = await service.WinningCards(cards);
            Console.WriteLine(winners);

            Console.WriteLine("Close the game");
            var responseEnd = await service.EndGame();
            Console.WriteLine($"response: {responseEnd}");
        }
    }
}
