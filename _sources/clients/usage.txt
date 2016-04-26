Using HttpClientBuilder
=======================

.. code-block:: csharp

	static readonly HttpClientBuilder ClientBuilder =
		new HttpClientBuilder()
			.WithActivityCorrelation()
			.WithLogging();

	HttpClient client1 = ClientBuilder.CreateClient(baseUri: "http://foo.com/bar");
	using (client1)
	{
		// Use client
	}

	HttpClient client2 = ClientBuilder.CreateClient(baseUri: "http://baz.net/bonk");
	using (client2)
	{
		// Use client
	}
