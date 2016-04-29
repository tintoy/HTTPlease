HttpRequest<TContext>
=====================

.. toctree::
   :maxdepth: 2

   Usage <usage>

*This topic is a work in progress*

``HttpRequest<TContext>`` is much the same as ``HttpRequest``, except that it supports resolving deferred values from an object supplied when the request is built / invoked.

So while HttpRequest tends to be used either for non-parameterised requests (i.e. simply as an HTTP request DSL), ``HttpRequest<TContext>`` is more commonly constructed once and cached (e.g. in a static read-only field) and its parameters get their values from an object passed to construct each request.
