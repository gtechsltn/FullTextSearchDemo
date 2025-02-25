using FullTextSearchDemo.Models;
using FullTextSearchDemo.SearchEngine.Configuration;

namespace FullTextSearchDemo.Search;

public class ProductConfiguration : IIndexConfiguration<Product>
{
    public string IndexName => "product-index";
}