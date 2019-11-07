# RDeF.net - RDF for .net made simple.

RDeF.net is an ORM class set of libraries that allows developers to work with RDF data sets without deeper knowledge of what's behind.

Version 1 allows to operate and query in-memory RDF data sets. Implementation used transforms input RDF (i.e. XML/RDF or TriG)
into objects implementing various interfaces so querying is as easy as applying LINQ onto those objects.

Please not the current implementation does not support physical triple stores yet!

## Quick start

```csharp
// Create a data context to work with instances.
var context = new DefaultEntityContextFactory()
    .WithMappings(_ => _.FromAssemblyOf<IProduct>())
    .Create();

// Create a new product and fill it with data.
var product = context.Create<IProduct>(new Iri("some:product"));
product.Name = "some Product";
product.Price = 99.99M;
product.Categories.Add("Super cool");

// Commit our changes.
context.Commit();

// Save our current context's data.
var buffer = new MemoryStream();
var rdfWriter = new JsonLdWriter();
using (var streamWriter = new StreamWriter(buffer, Encoding.UTF8, 1024, true))
{
    (Context.EntitySource as ISerializableEntitySource).Write(streamWriter, rdfWriter);
}

// Print out resulting JSON-LD to the console.
buffer.Seek(0, SeekOrigin.Begin);
Console.Write(Encoding.UTF8.GetString(buffer.ToArray()));
```

## Libraries

There are several libraries enabling various features:

- Contracts - pure interface and base types used across other libraries, useful when creating extensions or projects enabled for RDeF.net
- Core - core library
- Mapping.Attributes - enables you to map your interfaces (RDF allows multiple types added dynamically!) to RDF classes and properties with attributes
- Mapping.Fluent - enables you to map your interfaces to RDF classes and properties with fluent-like API driven code
- Serialization - several RDF readers and writers

With these you can start working with RDF data sets in no time without sacrificing performance!

Expect more details soon...