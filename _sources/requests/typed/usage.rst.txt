Using HttpRequest<TContext>
===========================

* `Overview <index>`_

*This topic is a work in progress*

.. code-block:: csharp

    // An example of a class used as a request context.

    class ExampleRequestContext
    {
        public string Action { get; set; }
        public int Id { get; set; }
        public bool? Flag { get; set; }
    }

    // Some examples using the request context.

    ExampleRequestContext context = new ExampleRequestContext();

    using (HttpClient httpClient = new HttpClient())
    {
        HttpRequest<ExampleRequestContext> requestBuilder =
            HttpRequest<ExampleRequestContext>.Factory.Create("http://localhost:1234/")
                .WithRelativeRequestUri("{action}/{id}?flag={flag?}")
                .WithTemplateParameter("action", context => context.Action)
                .WithTemplateParameter("id", context => context.Id)
                .WithTemplateParameter("flag", context => context.Flag);

        context.Action = "foo";
        context.Id = 1;
        context.Flag = true;

        // Request URI will be "http://localhost:1234/foo/1?flag=True".
        await client.GetAsync(requestBuilder, context);

        context.Flag = false;

        // Request URI will be "http://localhost:1234/foo/1?flag=False".
        await client.GetAsync(requestBuilder, context);

        context.Action = "diddly";
        context.Id = -17;
        context.Flag = null;

        // Request URI will be "http://localhost:1234/diddly/-17".
        await client.GetAsync(requestBuilder, context);
    }
