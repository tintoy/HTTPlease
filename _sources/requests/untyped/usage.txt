Using HttpRequest
=================

.. code-block:: csharp

	class ExampleResponseBody
	{
		public string Message { get; set; }
		public int ErrorCode { get; set; }
	}

	// Our base request definition.
	// You don't have to do this separately - you can declare the full URI in-place where you use it if you prefer.
	// You can also use relative URIs if you prefer (the HttpClient's base URI will be used instead).
	HttpRequest baseRequest = HttpRequest.Create.FromUri("http://localhost:1234");

	using (HttpClient client = new HttpClient())
	{
		string variable2 = null;

		HttpRequest request =
			baseRequest.WithRelativeRequestUri("api/{variable1}/foo/{variable2}/bar")
				.WithQueryParameter("diddly", "bonk")
				.WithTemplateParameter("variable1", 1234) // Use constant value
				.WithTemplateParameter("variable2", () => variable2) // Get value when request is invoked
				.UseJson() // Use Newtonsoft.Json for serialisation
				.ExpectJson(); // Set the Accept header to "application/json"

		// Short-hand for .UseJson().ExpectJson() is to call HttpRequest.Create.JsonFromUri(uri).

		// If variable2 is left as null, the request URI will be:
		//	http://localhost:1234/api/1234/foo/bar?diddly=bonk

		// If variable 2 is given a value ("hello world"), the request URI will be:
		//	http://localhost:1234/api/1234/foo/hello%20world/bar?diddly=bonk

		ExampleResponseBody responseBody;

		// If you want full control of response handling:

		using (HttpResponseMessage response = await client.GetAsync(request))
		{
			response.EnsureSuccessStatusCode();

			// If reading directly from the content you'll have to specify the content formatter to use.
			responseBody = await response.Content.ReadAsAsync<ExampleResponseBody>(new JsonFormatter());
		}

		// Or, for something a bit less verbose:

		using (HttpResponseMessage response = await client.GetAsync(request))
		{
			response.EnsureSuccessStatusCode();

			// Uses appropriate formatter configured above by calling UseJson.
			responseBody = await response.ReadContentAsAsync<ExampleResponseBody>();
		}

		// Or, for the most ceremony-free way possible:

		// Uses appropriate formatter configured above by calling UseJson.
		ExampleResponseBody responseBody =
			await client
				.GetAsync(request)
				.ReadAsAsync<ExampleResponseBody>();
	}
