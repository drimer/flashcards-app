# flashcards-app

This is a proof of concept I am using to learn C# and .NET.

I didn't want to deal with a database, infrastructure, or anything that could distract me
from writing code in C#. This is the reason for the unusual high-level design, ie. when a
question is generated and sent to the client, it isn't stored anywhere, and the API relies
on the client to give back the original question when submitting an answer. 

This API is designed to take as many topics as the developer wants. Things should already
be decoupled enough so that each different topic can have their own ways of generating
questions and checking whether the answers are correct.

I won't reference URLs directly here, but these are the topics I have considered adding:
- Plants: asking for their name from looking at an image. There's a nice API out there, but
it's paid, and this is just a POC.
- Historical figures (properly done): there's also a nice API out there, but it's also paid,
so I've hardcoded the data for these questions manually instead of paying the subscription.


## Improvements (not necessarily in this order)

1. Object mapping between DTOs and Models: surely, there are libraries out there that do this
better than my custom code.
2. Dependency Injection: this has been done at the controller level, but it needs to be applied
everywhere else. Revisit this and see where it's needed.
3. Unit testing: I'm hiding in shame at the complete lack of tests :(
4. Serialisation and validation of requests and DTO objects: there has to be a much nier way
than reinventing the wheel here.
5. Code style (linting, formatting, etc): my IDE has helped me a bit, but I'm sure there's room for improvement here. This point includes proper checks for null values, values returned from TryParse()
and any other small improvements that make this code more C#-y.


## Features I would add before making this public in any website.

1. Avoid duplicated questions within a batch.
2. Use a database to verify that users are asking questions they have been asked. Without this,
a user can pick any question they know for sure the answer, and get it always correct.
   1.5. Alternatively, set a minimum batch size of 10 and use a hsahing algorithm that is secret-based, then use that on the full batch and attach it in a new field to be verified on 
   answer submission. This way, users would still be able to "cheat" by sending always the same
   batch until they get all the answers right, but at least they won't be able to craft their
   own selection of questions from scratch.


## Commands

- Run unit tests:

```bash
dotnet test FlashcardApp.Tests
```