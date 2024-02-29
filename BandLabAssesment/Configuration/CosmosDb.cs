namespace BandLabAssesment.Configuration;

public class CosmosDb
{
    public string DatabaseName { get; set; }
    public string ConnectionString { get; set; }

    public static string PostsContainer = "posts";
    public static string CommentsContainer = "comments";
}