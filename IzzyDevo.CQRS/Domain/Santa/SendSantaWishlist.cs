using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IzzyDevo.CQRS.ExternalServices.Santa.Contracts;
using IzzyDevo.CQRS.Infrastructure.Pipeline;
using IzzyDevo.CQRS.Infrastructure.Pipeline.Logging;
using MediatR;

namespace IzzyDevo.CQRS.Domain.Santa
{
	public class SendSantaWishlist : ICommand, ILogged
	{
		public IEnumerable<string> Wishes { get; set; }

		public string LogMessage { get; }
	}

	/// <summary>
	/// Handles <see cref="SendSantaWishlist"/> command.
	/// </summary>
	public class SendSantaWishlistHandler : IRequestHandler<SendSantaWishlist>
	{
		private readonly IMediator _mediator;

		public SendSantaWishlistHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task Handle(SendSantaWishlist message, CancellationToken cancellationToken)
		{
			var wishlist = message.Wishes.Select(Wish.Create).ToList();

			var publishTasks = new List<Task>();
			publishTasks.Add(NotifySanta(cancellationToken));
			publishTasks.AddRange(NotifyToyProducers(cancellationToken, wishlist));

			await Task.WhenAll(publishTasks.ToArray());
		}

		private Task NotifySanta(CancellationToken cancellationToken)
		{
			return _mediator.Publish(new NewWishlistSentToSantaEvent(), cancellationToken);
		}

		private List<Task> NotifyToyProducers(CancellationToken cancellationToken, List<Wish> wishlist)
		{
			// If wishlist contains any supported toy, notify about demand
			var supportedToys = new[] { "Bear", "Doll", "Car" };
			Func<Wish, bool> toysFilter = wish => supportedToys.Contains(wish.Name);
			IEnumerable<string> toys = wishlist.Where(toysFilter).Select(x => x.Name);

			var sendNewDemandEventTasks = toys.Select(toy => new NewDemandForToyEvent { Toy = toy })
				.Select(e => _mediator.Publish(e, cancellationToken))
				.ToList();

			return sendNewDemandEventTasks;
		}
	}

	public class NewDemandForToyEvent : INotification
	{
		public string Toy { get; set; }
	}

	public class NewDemandForToyEventHandler : INotificationHandler<NewDemandForToyEvent>
	{
		public async Task Handle(NewDemandForToyEvent notification, CancellationToken cancellationToken)
		{
			var toy = notification.Toy;

			await Task.Delay(toy == "Doll" ? 500 : 1000, cancellationToken);


			Console.WriteLine($"New demand for {toy} has been recorded.");
		}
	}

	public class NewWishlistSentToSantaEvent : INotification
	{
	}

	public class NewWishlistSentToSantaEventHandler : INotificationHandler<NewWishlistSentToSantaEvent>
	{
		public async Task Handle(NewWishlistSentToSantaEvent notification, CancellationToken cancellationToken)
		{
			await Task.Delay(2 * 500, cancellationToken);

			Console.WriteLine("Received event about new wishlist to santa! Is it December already?!");
		}
	}
}
