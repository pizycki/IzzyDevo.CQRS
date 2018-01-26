using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IzzyDevo.CQRS.Domain.Life;
using IzzyDevo.CQRS.Domain.Santa;
using IzzyDevo.CQRS.Domain.YouTube;
using MediatR;

namespace IzzyDevo.CQRS
{
    public static class Runner
    {
        public static async Task Run(IMediator mediator)
        {
            // Point of life
            var answer = await mediator.Send(new WhatIsTheMeaningOfLife());
            Console.WriteLine($"The answer to the point of life is {answer}");

            // Santa
            await mediator.Send(new SendSantaWishlist
            {
                Wishes = new List<string>
                {
                    "Car", "Doll", "Harry Potter book"
                }
            });
            Console.WriteLine("Your wishlist has been sent to santa!");

            // YouTube
            await mediator.Send(new Subscribe
            {
                ChannelId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            });
            Console.WriteLine("You've subscribed to YouTube channel!");

        }
    }
}