using FullTextSearchDemo.SearchEngine.Models;

namespace FullTextSearchDemo.Models;

public class Movie : IDocument
{
    public string UniqueKey => TConst;
    
    public string TConst { get; set; }
    
    public string TitleType { get; set; }
    
    public string PrimaryTitle { get; set; }
    
    public string OriginalTitle { get; set; }
    
    public bool IsAdult { get; set; }
    
    public int StartYear { get; set; }
    
    public int EndYear { get; set; }
    
    public int RuntimeMinutes { get; set; }
    
    public string[] Genres { get; set; }
}