﻿using System;
using System.Collections.Generic;
using TeamZ.Code.Helpers;
using UniRx;

namespace TeamZ.Code.Mediator
{
	public interface ICommand
	{

	}

	public interface IHandler
	{

	}

	public interface IHandler<TCommand> : IHandler
		where TCommand : ICommand
	{
		void Handle(TCommand command);
	}

	public class Mediator : Singletone<Mediator>
	{
		private Dictionary<Type, IHandler> handlers;

		public Mediator()
		{
			this.handlers = new Dictionary<Type, IHandler>();
		}

		public void Add<TCommand>(IHandler<TCommand> handler)
			where TCommand : ICommand
		{
			this.handlers.Add(typeof(TCommand), handler);
			MessageBroker.Default.Receive<TCommand>().
				ObserveOnMainThread().
				Subscribe(command => this.Handle(command));
		}

		public void Add<TCommand>(Action<TCommand> handler)
			where TCommand : ICommand
		{
			MessageBroker.Default.Receive<TCommand>().
				ObserveOnMainThread().
				Subscribe(command => handler(command));
		}

		public void Handle<TCommand>(TCommand command)
			where TCommand : ICommand
		{
			if(!this.handlers.TryGetValue(command.GetType(), out var handler))
			{
				throw new InvalidOperationException("Handler is missing");
			}

			((IHandler<TCommand>)handler).Handle(command);
		}

        public void Reset()
        {
            this.handlers.Clear();
        }
    }
}
