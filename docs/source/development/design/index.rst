HTTPlease Design
================

HTTPlease uses Microsoft's immutable collections to ensure that the state for a given HttpRequest is immutable. In order to change the HttpRequest, you clone it, making one or more changes in the process (leaving the original untouched).
Normally, this mutate-by-cloning operation is implemented as an extension method.

For example:

.. code-block:: csharp

	public static HttpRequest WithRequestAction(this HttpRequest request, RequestAction<object> requestAction)
	{
		if (request == null)
			throw new ArgumentNullException(nameof(request));

		if (requestAction == null)
			throw new ArgumentNullException(nameof(requestAction));

		return request.Clone(properties =>
		{
			properties[nameof(HttpRequest.RequestActions)] = request.RequestActions.Add(requestAction);
		});
	}

Note that the ``Clone`` method takes an ``Action<IDictionary<string, object>>`` delegate. This delegate makes changes to an ``IDictionary`` that represents a mutable (change-tracking) version of the ``ImmutableDictionary`` that acts as a backing store for the original request's properties.
Any changes you make to this dictionary will be applied as deltas to the original ``ImmutableDictionary`` to produce a new ``ImmutableDictionary`` that only contains the differences from the original.
