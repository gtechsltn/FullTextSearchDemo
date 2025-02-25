# Lucene.NET powered search library for ASP.NET Core

+ [Lucene.NET: Facets — made simple](https://medium.com/@rojasjimenezjosea/lucene-net-facets-made-simple-1de9e2cdc314)
+ [Lucene.NET powered search library for ASP.NET Core](https://medium.com/@rojasjimenezjosea/lucene-net-powered-search-library-for-asp-net-core-ef8dc23959bd)
+ [Full Text Search Demo using Lucene.NET](https://github.com/rojasjo/FullTextSearchDemo)
+ [Deploy full stack ASP.NET Core with Azure SQL database to Azure for free (part1)](https://medium.com/@rojasjimenezjosea/deploy-full-stack-asp-net-core-with-azure-sql-database-to-azure-for-free-dff9072ab56b)
+ [Deploy full stack ASP.NET Core with Azure SQL database to Azure for free (part2)](https://medium.com/@rojasjimenezjosea/deploy-full-stack-asp-net-core-with-azure-sql-database-to-azure-for-free-2-0be0ac0a74ba)

FullTextSearchDemo is a project that provides search engine services for full-text search in documents. It utilizes Lucene.NET for indexing and searching documents efficiently.

[Source Code Latest](https://github.com/rojasjo/FullTextSearchDemo)

## Getting Started

### Installation

TBD

### Configuration

#### Defining the type of ```document``` 

Start by configuring the model you want to search by implementing the IDocument interface in your class.

```csharp

public class Product : IDocument
{
    public string UniqueKey => Id.ToString();
    
    public int Id { get; set; }
    ...
}

```

Please note that the UniqueKey is used for updating and deleting documents. It is crucial to ensure that each document has a unique UniqueKey, as shared UniqueKey values among documents can lead to unintended consequences during update and delete operations.

#### Index directory

Create an class implemeting the `IIndexConfiguration<T>` interface, where `T` is the type of documents you want to search.

```csharp

public class ProductConfiguration : IIndexConfiguration<Product>
{
    public string IndexName => "product-index";
}

```

The index name is the folder where Lucene.NET will store the indexed data and related files.

#### Service registration

Register the search engine services using the provided extension method on IServiceCollection.

```csharp
...
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSearchEngineServices(new ProductConfiguration());

var app = builder.Build();
...
```

### Usage

#### Adding documents

To add a single document to the search engine, use the Add method.

```csharp
var newProduct = new Product
{
    Id = 1,
    Name = "Sample Product",
    Description = "This is a sample product description."
};

_searchEngine.Add(newProduct);

```

The AddRange method is designed to add a collection of documents:

```csharp
var products = new List<Product>();

// Add items to the list

 _searchEngine.AddRange(products);

```

#### Updating documents

To update an existing document in the search engine, use the Update method providing the updated document.

```csharp
var updatedProduct = new Product
{
    Id = 1,
    Name = "Updated Product",
    Description = "This is an updated product description."
};

_searchEngine.Update(updatedProduct);
```


#### Deleting a Document

To remove a single document from the search engine, use the Remove method providing the document.

```csharp
  var product = new Product
        {
            Id = 1,
            Name = "Updated Product",
        };

  _searchEngine.Remove(product);
  
```

If you desire to remove all documents from the index you can use the Clear method.

```csharp
 
  _searchEngine.Clear();
  
```

#### Searching by Specific Fields
You can perform searches based on specific fields within your documents using the FieldSpecificSearchQuery. Here's an example of how to search for products by their name and description.

```csharp
var searchTerm = new Dictionary<string, string?>();

searchTerm.Add(nameof(Product.Name), "MyProduct");
searchTerm.Add(nameof(Product.Description), "Its description");

var searchQuery = new FieldSpecificSearchQuery
{
    SearchTerms = searchTerm,
    PageNumber = 1,
    PageSize = 10,
    Type = SearchType.ExactMatch
};

var result = _searchEngine.Search(searchQuery);

```

#### Full-Text Searching

To perform a full-text search across all fields of your documents, use the AllFieldsSearchQuery. This allows you to find documents that match a search term regardless of the field.

```csharp

var fullTextQuery = new AllFieldsSearchQuery
{
    SearchTerm = "Sample Product",
    PageNumber = 1,
    PageSize = 10,
    Type = SearchType.FuzzyMatch
};

var fullTextResult = searchEngine.Search(fullTextQuery);
```

## LICENSE

Copyright 2023 José Rojas Jimenez

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of th*.doce License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
