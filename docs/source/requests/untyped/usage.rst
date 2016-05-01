Using HttpRequest
=================

* `Creating a request from a URI`_
* `Binding request parameters`_
* `Binding request parameters to an object's properties`_
* `Creating a request with late-bound parameters`_
* `Working with responses`_

  * `Just give me the response`_
  * `I want to handle specific status codes`_
  * `I want to handle failure status codes as exceptions`_
  * `Some basic data-types`_

Creating a request from a URI
-----------------------------

First, let's define a request that will act as the basis for the requests in subsequent examples.

Note that, if you prefer, you don't have to do this separately (you can declare the full URI in-place where you use it).
You can also use relative URIs (in which case the the HttpClient's base URI will be used).

.. code-block:: csharp

    HttpRequest baseRequest = HttpRequest.Factory.Create("http://localhost:1234");
    HttpRequest requestTemplate = baseRequest.WithRelativeRequestUri(
        "customers/{variable1}/foo/{variable2}/bar"
    );

    // Full request URI is now http://localhost:1234/customers/{variable1}/foo/{variable2}/bar
    // Note the use of parameters (the bits in braces). That means this URI is a template.

Binding request parameters
--------------------------

The parameters (or "holes") in the request URI must be bound to values before the request can be invoked.
One way to do this is to give the parameters constant values by specialising the template:

.. code-block:: csharp

    using (HttpClient client = new HttpClient())
    {
        HttpRequest request =
            requestTemplate
                .WithQueryParameter("sort", "name")
                .WithTemplateParameter("variable1", 1234)
                .WithTemplateParameter("variable2", (string)null);

        using (HttpResponseMessage response = await client.GetAsync(request))
        {
            // Request URI will be:
            //    http://localhost:1234/customers/1234/foo/bar?sort=name
        }
    }

Note that while all of a request's parameters must be bound, they can be have a value of ``null`` which will result in their path segment / query parameter being omitted from the final URI.

Binding request parameters to an object's properties
----------------------------------------------------

The parameters in the request URI can also be bound to the properties of an object (including anonymous types).
If you're not using anonymous types (or other immutable object types) then you may be better off using `HttpRequest<TContext> <../typed/index>`_.

.. code-block:: csharp

    using (HttpClient client = new HttpClient())
    {
        HttpRequest request =
            requestTemplate
                .WithQueryParameter("sort", "name")
                .WithTemplateParameters(new
                {
                    variable1 = 1234,
                    variable2 = "baz"
                });

        using (HttpResponseMessage response = await client.GetAsync(request))
        {
            // Request URI will be:
            //    http://localhost:1234/customers/1234/foo/baz/bar?sort=name
        }
    }

Creating a request with late-bound parameters
---------------------------------------------

Instead of constant values, however, request parameters can also be bound to delegates that resolve their values each time the request is invoked.

.. code-block:: csharp

    using (HttpClient client = new HttpClient())
    {
        int variable1 = 1234;
        string variable2 = "hello";

        HttpRequest request =
            requestTemplate
                .WithQueryParameter("diddly", "bonk")
                .WithTemplateParameter("variable1", () => variable1))
                .WithTemplateParameter("variable2", () => variable2);

        using (HttpResponseMessage response = await client.GetAsync(request))
        {
            // Request URI will be:
            //    http://localhost:1234/customers/1234/foo/hello/bar?diddly=bonk
        }

        // Let's give "variable2" a value so that its path segment will be present in the final URI.
        variable2 = "hello world";

        using (HttpResponseMessage response = await client.GetAsync(request))
        {
            // Request URI will be:
            //    http://localhost:1234/customers/1234/foo/hello%20world/bar?diddly=bonk
        }
    }

Working with responses
----------------------

HTTPlease offers several styles of interaction with the responses from HTTP requests.
These range from simple / low-ceremony to fully-customisable.

Click :ref:`here <data-types>` for the definitions of some basic data-types used in these examples.

Just give me the response
^^^^^^^^^^^^^^^^^^^^^^^^^
.. code-block:: csharp

    using (HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:1234/") })
    {
        HttpRequest request = HttpRequest.Factory.CreateJson("customers/1");

        // Uses appropriate formatter(s) configured above by calling CreateJson.
        Customer customer =
            await client.GetAsync(request)
                .ReadAsAsync<Customer>();
    }

I want to handle specific status codes
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
.. code-block:: csharp

    using (HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:1234/") })
    {
        HttpRequest request = HttpRequest.Factory.CreateJson("customers/1");

        using (HttpResponseMessage response = await client.GetAsync(request))
        {
            // Handle specific status codes.
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Forbidden)
            {
                Error error = await response.ReadContentAsAsync<Error>();

                throw new HttpRequestException<Error>(error);
            }

            // For all other status codes, use standard behaviour for deciding whether the response indicates success.
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(String.Format(
                    "The request failed because an unexpected status code ({0}) was received from the server.",
                    response.StatusCode
                );
            }

            // Uses appropriate formatter(s) configured above by calling CreateJson.
            Customer customer = await response.ReadContentAsAsync<Customer>();
        }
    }

I want to handle failure status codes as exceptions
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
.. code-block:: csharp

    HttpRequest request = HttpRequest.Factory.CreateJson("customers/1");

    using (HttpClient client = new HttpClient())
    {
        try
        {
            // If status code indicates success, read response as Customer.
            // If status code indicates failure, read response as Error and throw.
            // You can also tell ReadAsAsync which status code(s) indicate success.

            Customer customer =
                await client.GetAsync(request)
                    .ReadAsAsync<Customer, Error>();
        }
        catch (HttpRequestException<Error> expectedError)
          when expectedError.StatusCode == HttpStatusCode.NotFound
        {
            Error errorResponse = expectedError.Response;

            Log.Error(expectedError, errorResponse.ErrorMessage);
        }
        catch (HttpRequestException<Error> expectedError)
          when expectedError.StatusCode == HttpStatusCode.BadRequest
        {
            Error errorResponse = expectedError.Response;

            Log.Error(expectedError, errorResponse.ErrorMessage);
        }
        catch (HttpRequestException unexpectedError)
        {
            // Unexpected error (generic request failure, or unable to read response body)

            Log.Error(unexpectedError, unexpectedError.Message)
        }
    }

I want to do it all myself
^^^^^^^^^^^^^^^^^^^^^^^^^^
.. code-block:: csharp

    using (HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:1234/") })
    {
        HttpRequest request = HttpRequest.Factory.CreateJson("customers/1");

        using (HttpResponseMessage response = await client.GetAsync(request))
        {
            response.EnsureSuccessStatusCode();

            // Perform any response message processing required.

            // Decide how to interpret the response body (call to CreateJson above would have told the server that we Accept "application/json").
            if (response.Content.Headers.ContentType.MediaType != "application/json")
            {
                throw new HttpRequestException(
                    String.Format("The response has an unexpected content type: '{0}'.",
                    response.Content.Headers.ContentType.MediaType
                );
            }

            // Let's customise the JSON formatter.
            IInputFormatter formatter = new JsonFormatter(new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Reuse
            });

            // If reading directly from the content you'll have to specify the content formatter to use.
            responseBody = await response.Content.ReadAsAsync<Customer>(formatter);
        }
    }

Some basic data-types
^^^^^^^^^^^^^^^^^^^^^
.. code-block:: csharp
    :name: data-types

    class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public enum ErrorCode
    {
        Unknown = 0,
        EntityNotFound = 1,
        AccessDenied = 2,
        InternalError = 3
    }

    class Error
    {
        public string ErrorMessage { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }
