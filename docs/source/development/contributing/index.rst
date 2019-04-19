Contributing to HTTPlease
=========================

All contributors are welcome.

HTTPlease is designed to be extensible (so you can create your own libraries that extend its functionality) but, if you have an idea or feature request for something that you feel belongs in the core libraries (or have found a bug) please feel free to create a pull request.

Pull requests
-------------

* First, create an issue describing the nature of the change. Let's have a conversation about it :)
* Then, fork the repository and create a feature branch with a descriptive name (e.g. ``feature/my-feature``).
* Finally create a pull request that describes the nature of the change (including the issue number).

Code Style
----------

The golden rule:

  If you're not sure what style to use, look at the surrounding code and match that.

Some basic guidelines:

* The purpose of code is to communicate - use descriptive names for all members (this includes loop variables and lambda parameters)

  * For example, ``customers.Where(customer => customer.Name == name)`` is OK, but ``customers.Where(c => c.Name == name)`` is not
* All code, public or private, needs XML doc comments (you know what you mean but others may not)

  * Please don't use auto-generated comments - all this does is make it harder to know what members actually have meaningful comments.
  * If you really object to adding doc comments, make a note in the pull request and we'll try to add them for you
* Spaces, not tabs
* Please don't use the ``var`` keyword unless:

  * There is a type on the right-hand side of the expression (e.g. ``new Foo()``)
  * You're working with an anonymous type
  * You're working with a LINQ expression whose type is obvious (e.g. a grouping)
  * You're calling a generic method whose sole type parameter determines the return type (e.g. ``container.Resolve<MyEntities>()``)
* Err on the side of ease-of-use / ease-of-correct-usage for the consumer - the unspoken contract when it comes to authoring a library is that it's supposed to be more difficult for you so it's easier for them.
* If it's hard to express how functionality is meant to be used, consider writing a test that shows correct usage.

.. toctree::
   :maxdepth: 2

	 Design <../design/index.rst>
