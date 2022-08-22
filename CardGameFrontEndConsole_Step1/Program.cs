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

            try
            {
                Console.WriteLine("Starting a game");
                var responseStart = await service.StartGame("highcard");
                if (!responseStart.IsRunning)
                    throw new Exception("Error: could not start game");

                Console.WriteLine($"Game of type {responseStart.GameType} started at {responseStart.StartTime}");

                Console.WriteLine("\nGet a card");
                var card = await service.DealCard();
                Console.WriteLine(card);

                Console.WriteLine("\nGet 5 cards");
                var cards = await service.DealCards(5);
                foreach (var item in cards)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("\nGet WinningCards");
                var winners = await service.WinningCards(cards);
                foreach (var item in winners)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("\nClose the game");
                var responseEnd = await service.EndGame();
                if (responseEnd.IsRunning)
                    throw new Exception("Error: could not end game");

                Console.WriteLine($"Game of type {responseEnd.GameType} ended at {responseEnd.EndTime}");               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
