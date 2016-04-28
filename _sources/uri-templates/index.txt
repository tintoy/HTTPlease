URI Templates
=============

*This topic is a work in progress*

If you are calling services over HTTP, chances are that you've found generating request URLs to be a relatively common source of bugs or hard-to-maintain code (e.g. complex use of ``String.Format`` or string interpolation, forgetting to correctly escape values).

A typical example
-----------------

.. code-block:: csharp

    string serverName = "foo.com";
    string baseUri = String.Format("http://{0}/api/v1/", serverName);
    string customerId "AU-SYD/17";
    string sortField = "First Name";
    DateTime startOfMonth;

    string customerRequestUri = String.Format("{0}/customers/{0}/orders?after={1}"
        baseUri,
        customerId,
        startOfMonth
    );

Chances are, you'll have problems trying to inject the date as-is into the "after" parameter, and depending on the format for ``customerId`` (e.g. "AU-SYD/27") you may have even more trouble with escaped slashes in the path (IIS, for one, can get quite picky about that sort of thing).
You'll also wind up with a double slash after the base URI.

A slightly tidier version
-------------------------

.. code-block:: csharp

    Uri baseUri = new Uri(
        String.Format("http://{0}/api/v1", serverName)
    );

    // ...

    Uri customerRequestUri = new Uri(baseUri,
        String.Format("customers/{0}?sortBy={1}"
            Uri.EncodeUriString(customerId),
            Uri.EncodeDataString(sortField)
        )
    )

This version takes care of escaping values, and will correctly concatenate the base URI with the rest of the request URI.
But it's still a little awkward because you have to remember to escape each parameter.

Using URI templates
-------------------

.. code-block:: csharp

    Uri baseUri = new Uri(
        String.Format("http://{0}/api/v1", serverName)
    );

    UriTemplate template = new UriTemplate("customers/{customerId}?sortBy={sortField}");

    var templateParameters = new Dictionary<string, string>
    {
        ["customerId"] = customerId,
        ["sortField"] = sortField
    };

    Uri customerRequestUri = template.Populate(baseUri, templateParameters);

    templateParameters["customerId"] = "AU-MEL/4";
    customerRequestUri = template.Populate(baseUri, templateParameters);

Now you can declare your template once (and, since it's immutable, cache it in a static field) then populate it as required to generate the URI for each request.
