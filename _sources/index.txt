.. image:: _static/logo-large.gif

==================================

Introduction
^^^^^^^^^^^^
HTTPlease is a library for creating / configuring HttpClients, using them to make HTTP requests, and processing the responses.

It is based on the concept of an immutable template (with lazily-resolved parameters) that acts as the definition for an HTTP request. Any change to this template makes a copy of it, only capturing the differences between the old and the new template. This allows you to progressively build up a hierarchy of templates that increasingly specialise requests until they refer to a specific API operation.

Getting bored? Skip ahead
"""""""""""""""""""""""""

* `Usage examples <requests/untyped/usage>`_
* `Source code <https://github.com/tintoy/HTTPlease>`_

Request templates...?
"""""""""""""""""""""
For example, you might have a base request with a URL of ``/api``, and then extend that with a relative URI ``{apiVersion}``, which would effectively be ``/api/{apiVersion}``.

Then you might extend that with ``{entitySet}`` to become ``api/{apiVersion}/{entitySet}`` (let's call this template "entity list"), and extend that with ``{entityId}`` resulting in ``api/{apiVersion}/{entitySet}/{entityId}`` (let's call this template "entity by Id").

So if all the entity sets exposed by your API follow this pattern then you can simply customise these templates by giving them values for "apiVersion" and "entitySet" (e.g. "user list" = "entity list with entitySet=user" and "user by Id" = "entity by Id" with "entitySet = user", resulting in ``api/v1/users/1``).

If the overall pattern for your API changes (e.g. ``/api/{entityVersion}`` becomes ``/api/public/versions/{entityVersion}``) then you simply modify the base template definition.

Note that not all template parameter values have to be deferred (lazily evaluated). Some can be given constant values as part of the template definition. See the tests for examples of how the API can be used.

What about clients?
"""""""""""""""""""
The `HttpClient builder <clients/index>`_ performs a similar role; it is a template describing how to create and configure an HttpClient (most of its power comes from the ability to define extension methods to encapsulate special configuration or custom message handlers in the pipeline). Again, see the tests for examples of how it can be used.

How does this compare to...?
""""""""""""""""""""""""""""

* Microsoft.AspNet.WebApi.Client
    * HTTPlease is definitely inspired by ASP.NET WebAPI (especially regarding the use of extension methods)
    * However, it seems to me that ASP.NET WebAPI is being superceded by ASP.NET MVC Core (and, therefore, a gap now exists regarding client functionality)
* RestSharp - similar facilities, but an emphasis on:
    * Testability (and repeatability) of requests
    * Support for URI templates
    * Composability
    * Extensibility
    * Pay-for-play (only pull in the dependencies that you need)

Honestly, if you use either of these frameworks to make one-off web requests, they're totally fit-for-purpose; where HTTPlease really excels is in building client libraries to wrap web APIs. 


Contents
""""""""
.. toctree::
   :maxdepth: 2

   self
   uri-templates/index
   clients/index
   requests/index
   Extensibility <extensibility/index>
   Development <development/index>

Seem familiar?
~~~~~~~~~~~~~~

This project is a reimagining of my previous work in the `WebAPI Template Toolkit (Watt) <https://github.com/DimensionDataCBUSydney/Watt>`_.
