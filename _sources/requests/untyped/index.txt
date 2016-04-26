HttpRequest
===========

Think of `HttpRequest` as a template for creating an `HttpRequestMessage <https://msdn.microsoft.com/en-us/library/system.net.http.httprequestmessage(v=vs.118).aspx>`_.

At its heart, it is a collection of configuration actions (and any data required to support those actions) that are invoked against each outgoing request message.

Because its functionality is expressed in terms of actions, it is trivially easy to write your own domain-specific extensions for `HttpRequest` (see the section on `extensibility <../../extensibility/index>`_ for details).

A powerful feature of HTTPlease is that the data used by those actions is resolved at the time each outgoing `HttpRequestMessage` is created (rather than when the `HttpRequest` template is created).

This enables each `HttpRequest` to be immutable; each change (or sequence of changes) to the `HttpRequest` produces a new `HttpRequest` that only captures the differences from the previous `HttpRequest`.

`HttpRequest` uses an `ImmutableDictionary <https://msdn.microsoft.com/en-us/library/dn467181%28v=vs.111%29.aspx>`_ to store its properties.

.. toctree::
   :maxdepth: 2

   Usage <usage>
