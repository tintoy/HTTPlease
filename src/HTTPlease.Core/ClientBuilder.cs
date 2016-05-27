using System;
using System.Collections.Generic;
using System.Linq;
#if DNXCORE50
using System.Reflection;
#endif // DNXCORE50
using System.Net.Http;

namespace HTTPlease
{
	using Core.Utilities;

	/// <summary>
	///		Builds <see cref="HttpClient"/>s with pipelines of <see cref="DelegatingHandler">HTTP message handler</see>s.
	/// </summary>
	/// <remarks>
	///		Be aware that, if you return singleton instances of message handlers from factory delegates, those handlers will be disposed if the factory encounters any exception while creating a client.
	/// </remarks>
	public sealed class ClientBuilder
	{
		/// <summary>
		///		The default factory for message-pipeline terminus handlers.
		/// </summary>
		static readonly Func<HttpMessageHandler> DefaultMessagePipelineTerminus = () => new HttpClientHandler();

		/// <summary>
		///		Factory delegates used to produce the HTTP message handlers that comprise client pipelines.
		/// </summary>
		readonly List<Func<DelegatingHandler>> _handlerFactories = new List<Func<DelegatingHandler>>();

		/// <summary>
		///		The factory for message-handlers that act as the message-pipeline terminus.
		/// </summary>
		Func<HttpMessageHandler> _messagePipelineTerminus = DefaultMessagePipelineTerminus;

		/// <summary>
		///		Create a new HTTP client builder.
		/// </summary>
		public ClientBuilder()
		{
		}

		/// <summary>
		///		The factory for message-handlers that act as the message-pipeline terminus.
		/// </summary>
		public Func<HttpMessageHandler> MessagePipelineTerminus
		{
			get
			{
				return _messagePipelineTerminus;
			}
			set
			{
				_messagePipelineTerminus = value ?? DefaultMessagePipelineTerminus;
			}
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> using the configured message-handler pipeline.
		/// </summary>
		/// <param name="baseUri">
		///		An optional base URI for the <see cref="HttpClient"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpClient"/>.
		/// </returns>
		public HttpClient CreateClient(Uri baseUri = null)
		{
			HttpMessageHandler pipelineTerminus = null;
			List<DelegatingHandler> pipelineHandlers = new List<DelegatingHandler>();
			HttpMessageHandler pipeline = null;
			HttpClient client = null;

			try
			{
				pipelineTerminus = MessagePipelineTerminus();

				foreach (Func<DelegatingHandler> handlerFactory in _handlerFactories)
				{
					DelegatingHandler currentHandler = null;
					try
					{
						currentHandler = handlerFactory();
					}
					catch
					{
						using (currentHandler)
							throw;
					}
					pipelineHandlers.Add(currentHandler);
				}

				pipeline = CreatePipeline(pipelineTerminus, pipelineHandlers);

				client = new HttpClient(pipeline);
				if (baseUri != null)
					client.BaseAddress = baseUri;
			}
			catch
			{
				using (pipelineTerminus)
				using (pipelineHandlers.ToAggregateDisposable())
				using (pipeline)
				using (client)
				{
					throw;
				}
			}

			return client;
		}

		/// <summary>
		///		Add an HTTP message-handler factory to the end of the pipeline.
		/// </summary>
		/// <typeparam name="THandler">
		///		The handler type.
		/// </typeparam>
		/// <param name="handlerFactory">
		///		The message-handler factory.
		/// </param>
		/// <returns>
		///		The <see cref="ClientBuilder"/> (enables method-chaining).
		/// </returns>
		/// <remarks>
		///		<typeparamref name="THandler"/> cannot be the <see cref="DelegatingHandler"/> base class.
		/// </remarks>
		public ClientBuilder AddHandler<THandler>(Func<THandler> handlerFactory)
			where THandler : DelegatingHandler
		{
			if (handlerFactory == null)
				throw new ArgumentNullException(nameof(handlerFactory));

			if (typeof(THandler) == typeof(DelegatingHandler))
				throw new InvalidOperationException("Handler type cannot be the DelegatingHandler base class.");

			if (_handlerFactories.OfType<Func<THandler>>().Any())
			{
				throw new InvalidOperationException(
					String.Format(
						"The configured handler pipeline already contains a factory for message-handlers of type '{0}'.",
						typeof(THandler).AssemblyQualifiedName
					)
				);
			}

			_handlerFactories.Add(handlerFactory);

			return this;
		}

		/// <summary>
		///		Insert an HTTP message-handler factory to the pipeline before the factory that produces handlers of the specified type.
		/// </summary>
		/// <typeparam name="THandler">
		///		The handler type.
		/// </typeparam>
		/// <typeparam name="TBeforeHandler">
		///		The type of handler before whose factory the new handler factory should be inserted.
		/// </typeparam>
		/// <param name="handlerFactory">
		///		The message-handler factory.
		/// </param>
		/// <param name="throwIfNotPresent">
		///		Throw an <see cref="InvalidOperationException"/> if no factory for <typeparamref name="TBeforeHandler"/> is present?
		/// 
		///		Default is <c>false</c>.
		/// </param>
		/// <returns>
		///		The <see cref="ClientBuilder"/> (enables method-chaining).
		/// </returns>
		/// <remarks>
		///		<typeparamref name="THandler"/> and <typeparamref name="TBeforeHandler"/> cannot be the <see cref="DelegatingHandler"/> base class.
		/// </remarks>
		public ClientBuilder AddHandlerBefore<THandler, TBeforeHandler>(Func<THandler> handlerFactory, bool throwIfNotPresent = false)
			where THandler : DelegatingHandler
			where TBeforeHandler : DelegatingHandler
		{
			if (handlerFactory == null)
				throw new ArgumentNullException(nameof(handlerFactory));

			if (typeof(THandler) == typeof(DelegatingHandler))
				throw new InvalidOperationException("Handler type cannot be the DelegatingHandler base class.");

			if (typeof(THandler) == typeof(DelegatingHandler))
				throw new InvalidOperationException("Handler type cannot be the DelegatingHandler base class.");

			if (_handlerFactories.OfType<Func<THandler>>().Any())
			{
				throw new InvalidOperationException(
					String.Format(
						"The configured handler pipeline already contains a factory for message-handlers of type '{0}'.",
						typeof(THandler).AssemblyQualifiedName
					)
				);
			}

			Type beforeHandlerFactoryType = typeof(Func<TBeforeHandler>);
			for (int handlerIndex = 0; handlerIndex < _handlerFactories.Count; handlerIndex++)
			{
				if (_handlerFactories[handlerIndex].GetType() == beforeHandlerFactoryType)
				{
					_handlerFactories.Insert(handlerIndex, handlerFactory);

					return this;
				}
			}

			if (throwIfNotPresent)
			{
				throw new InvalidOperationException(
					String.Format(
						"Cannot insert factory for message-handlers of type '{0}' before the factory for message-handlers of type '{1}' (the pipeline does not contain a factory for message-handlers of this type.",
						typeof(THandler).AssemblyQualifiedName,
						typeof(TBeforeHandler).AssemblyQualifiedName
					)
				);
			}

			// TBefore is not present, so just append to the end of the pipeline.
			_handlerFactories.Add(handlerFactory);

			return this;
		}

		/// <summary>
		///		Insert an HTTP message-handler factory to the pipeline after the factory that produces handlers of the specified type.
		/// </summary>
		/// <typeparam name="THandler">
		///		The handler type.
		/// </typeparam>
		/// <typeparam name="TAfterHandler">
		///		The type of handler after whose factory the new handler factory should be inserted.
		/// </typeparam>
		/// <param name="handlerFactory">
		///		The message-handler factory.
		/// </param>
		/// <param name="throwIfNotPresent">
		///		Throw an <see cref="InvalidOperationException"/> if no factory for <typeparamref name="TAfterHandler"/> is present?
		/// 
		///		Default is <c>false</c>.
		/// </param>
		/// <returns>
		///		The <see cref="ClientBuilder"/> (enables method-chaining).
		/// </returns>
		/// <remarks>
		///		<typeparamref name="THandler"/> and <typeparamref name="TAfterHandler"/> cannot be the <see cref="DelegatingHandler"/> base class.
		/// </remarks>
		public ClientBuilder AddHandlerAfter<THandler, TAfterHandler>(Func<THandler> handlerFactory, bool throwIfNotPresent = false)
			where THandler : DelegatingHandler
			where TAfterHandler : DelegatingHandler
		{
			if (handlerFactory == null)
				throw new ArgumentNullException(nameof(handlerFactory));

			if (typeof(THandler) == typeof(DelegatingHandler))
				throw new InvalidOperationException("Handler type cannot be the DelegatingHandler base class.");

			if (typeof(THandler) == typeof(DelegatingHandler))
				throw new InvalidOperationException("Handler type cannot be the DelegatingHandler base class.");

			if (_handlerFactories.OfType<Func<THandler>>().Any())
			{
				throw new InvalidOperationException(
					String.Format(
						"The configured handler pipeline already contains a factory for message-handlers of type '{0}'.",
						typeof(THandler).AssemblyQualifiedName
					)
				);
			}

			Type afterHandlerFactoryType = typeof(Func<TAfterHandler>);
			for (int handlerIndex = 0; handlerIndex < _handlerFactories.Count; handlerIndex++)
			{
				if (_handlerFactories[handlerIndex].GetType() == afterHandlerFactoryType)
				{
					_handlerFactories.Insert(handlerIndex + 1, handlerFactory);

					return this;
				}
			}

			if (throwIfNotPresent)
			{
				throw new InvalidOperationException(
					String.Format(
						"Cannot insert factory for message-handlers of type '{0}' after the factory for message-handlers of type '{1}' (the pipeline does not contain a factory for message-handlers of this type.",
						typeof(THandler).AssemblyQualifiedName,
						typeof(TAfterHandler).AssemblyQualifiedName
					)
				);
			}

			// TAfter is not present, so just append to the end of the pipeline.
			_handlerFactories.Add(handlerFactory);

			return this;
		}

		/// <summary>
		///		Enumerate the types of handlers configured in the factory's pipeline.
		/// </summary>
		/// <returns>
		///		A sequence of 0 or more types.
		/// </returns>
		/// <remarks>
		///		This operation uses Reflection, so it can be relatively expensive; use sparingly.
		/// </remarks>
		public IEnumerable<Type> EnumerateHandlerTypes()
		{
			for (int handlerIndex = 0; handlerIndex < _handlerFactories.Count; handlerIndex++)
			{
				Func<DelegatingHandler> factory = _handlerFactories[handlerIndex];
				Type factoryDelegateType = factory.GetType();

				yield return factoryDelegateType.GenericTypeArguments[0];
			}
		}

		/// <summary>
		///		Create an HTTP message-handler pipeline.
		/// </summary>
		/// <param name="pipelineTerminus">
		///		An <see cref="HttpMessageHandler"/> representing the terminus of the pipeline.
		/// </param>
		/// <param name="pipelineHandlers">
		///		A sequence of <see cref="DelegatingHandler"/>s representing additional steps in the pipeline.
		/// </param>
		/// <returns>
		///		An <see cref="HttpMessageHandler"/> representing the head of the pipeline.
		/// </returns>
		static HttpMessageHandler CreatePipeline(HttpMessageHandler pipelineTerminus, IEnumerable<DelegatingHandler> pipelineHandlers)
		{
			if (pipelineTerminus == null)
				throw new ArgumentNullException(nameof(pipelineTerminus));

			if (pipelineHandlers == null)
				throw new ArgumentNullException(nameof(pipelineHandlers));

			HttpMessageHandler pipeline = pipelineTerminus;
			foreach (DelegatingHandler pipelineHandler in pipelineHandlers.Reverse())
			{
				pipelineHandler.InnerHandler = pipeline;
				pipeline = pipelineHandler;
			}

			return pipeline;
		}
	}
}
