using FullTextSearchDemo.SearchEngine.Engine;
using FullTextSearchDemo.SearchEngine.Models;
using FullTextSearchDemo.SearchEngine.Services;
using FullTextSearchDemo.SearchEngine.Tests.TestModels;

namespace FullTextSearchDemo.SearchEngine.Tests;

public class SearchEngineForPostsTests
{
    private SearchEngine<Post> _searchEngine = null!;
    private const string Title = "Testing Apache Lucene.NET - Ensuring robust search functionality in C#";
    private readonly DocumentWriter<Post> _documentWriter;

    public SearchEngineForPostsTests()
    {
        _documentWriter = new DocumentWriter<Post>(new PostTestConfiguration());
    }

    [SetUp]
    public void Setup()
    {
        _documentWriter.WriteDocument(new Post
        {
            Id = 1,
            Title = Title,
            Content = "<h1>Fox</h1>"
        });

        _documentWriter.WriteDocument(new Post
        {
            Id = 2,
            Title = "Just another post about Search Engines",
            Content = "<h1>Solr rocks!</h1>"
        });

        _documentWriter.WriteDocument(new Post
        {
            Id = 2,
            Title = "Search is cool with Apache Lucene.NET",
            Content = "<h1>Apache Lucene at the core!</h1>"
        });

        _searchEngine = new SearchEngine<Post>(new DocumentReader<Post>(_documentWriter), _documentWriter);
    }

    [TearDown]
    public void TearDown()
    {
        _documentWriter.Writer.DeleteAll();
        _documentWriter.Writer.Commit();
    }

    [Test]
    public void Search_AllFieldQueryExactSearchWithCorrectTerm_ReturnsPost()
    {
        var query = new AllFieldsSearchQuery { SearchTerm = "fox", Type = SearchType.ExactMatch };

        var result = _searchEngine.Search(query).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo(Title));
        });
    }

    [Test]
    public void Search_AllFieldQueryExactSearchWithWrongTerm_ReturnsEmptyList()
    {
        var query = new AllFieldsSearchQuery { SearchTerm = "foy", Type = SearchType.ExactMatch };

        var result = _searchEngine.Search(query);

        Assert.That(result.Count(), Is.EqualTo(0));
    }

    [Test]
    [TestCase(SearchType.ExactMatch)]
    [TestCase(SearchType.FuzzyMatch)]
    [TestCase(SearchType.PrefixMatch)]
    public void Search_AllFieldQueryExactSearchWithEmptyTerm_ReturnsAllPosts(SearchType searchType)
    {
        var query = new AllFieldsSearchQuery { Type = searchType };

        var result = _searchEngine.Search(query).ToList();

        Assert.That(result, Has.Count.EqualTo(3));
    }

    [Test]
    [TestCase(0, 2, 2)]
    [TestCase(1, 2, 1)]
    [TestCase(2, 2, 0)]
    [TestCase(0, 1, 1)]
    [TestCase(0, 1, 1)]
    [TestCase(1, 1, 1)]
    [TestCase(2, 1, 1)]
    [TestCase(3, 1, 0)]
    public void Search_AllFieldQueryWithEmptyTermWithPagination_ReturnsTowPosts(int pageNumber, int pageSize,
        int expectedPosts)
    {
        var query = new AllFieldsSearchQuery
            { Type = SearchType.ExactMatch, PageNumber = pageNumber, PageSize = pageSize };

        var result = _searchEngine.Search(query).ToList();

        Assert.That(result, Has.Count.EqualTo(expectedPosts));
    }

    [Test]
    [TestCase("Testing", 1)]
    [TestCase("testIng", 1)]
    [TestCase("Search", 3)]
    [TestCase("Seorch", 3)]
    [TestCase("Apache", 2)]
    [TestCase("apache", 2)]
    [TestCase("fox", 0)]
    public void Search_OnlyTitleQueryFuzzySearchWithCorrectTerm_ReturnsAllPost(string search, int expected)
    {
        var query = new FieldSpecificSearchQuery
            { SearchTerms = new Dictionary<string, string?> { { "Title", search } }, Type = SearchType.FuzzyMatch };

        var result = _searchEngine.Search(query).ToList();

        Assert.That(result, Has.Count.EqualTo(expected));
    }

    [Test]
    [TestCase("Testing", 0)]
    [TestCase("testIng", 0)]
    [TestCase("testing", 1)]
    [TestCase("Search", 0)]
    [TestCase("search", 3)]
    [TestCase("seorch", 0)]
    [TestCase("Apache", 0)]
    [TestCase("apache", 2)]
    [TestCase("fox", 0)]
    [TestCase("Fox", 0)]
    public void Search_OnlyTitleQueryExactSearchWithCorrectTerm_ReturnsAllPost(string search, int expected)
    {
        var query = new FieldSpecificSearchQuery
            { SearchTerms = new Dictionary<string, string?> { { "Title", search } }, Type = SearchType.ExactMatch };

        var result = _searchEngine.Search(query).ToList();

        Assert.That(result, Has.Count.EqualTo(expected));
    }
}