using Newtonsoft.Json;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class AuthorChannelId
{
    public string value { get; set; }
}

public class Item
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string id { get; set; }
    public Snippet snippet { get; set; }
}

public class PageInfo
{
    public int totalResults { get; set; }
    public int resultsPerPage { get; set; }
}

public class Root
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string nextPageToken { get; set; }
    public PageInfo pageInfo { get; set; }
    public List<Item> items { get; set; }
}

public class Snippet
{
    public string videoId { get; set; }
    public TopLevelComment topLevelComment { get; set; }
    public bool canReply { get; set; }
    public int totalReplyCount { get; set; }
    public bool isPublic { get; set; }
    public string textDisplay { get; set; }
    public string textOriginal { get; set; }
    public string authorDisplayName { get; set; }
    public string authorProfileImageUrl { get; set; }
    public string authorChannelUrl { get; set; }
    public AuthorChannelId authorChannelId { get; set; }
    public bool canRate { get; set; }
    public string viewerRating { get; set; }
    public int likeCount { get; set; }
    public DateTime publishedAt { get; set; }
    public DateTime updatedAt { get; set; }
}

public class TopLevelComment
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string id { get; set; }
    public Snippet snippet { get; set; }
}


class Program
{
    public static List<string> comments = new List<string>();
    static async Task Main()
    {
        HttpClient client = new HttpClient();

        string videoId = "ENTER_VIDEO_ID";
        string APIKey = "ENTER_API_KEY";
        string url, nextPageToken, response;
        Root data;

        url = "https://youtube.googleapis.com/youtube/v3/commentThreads?part=snippet&maxResults=10000&order=time&videoId=" + videoId + "&key=" + APIKey + "";
        response = await client.GetStringAsync(url);
        data = JsonConvert.DeserializeObject<Root>(response);

        nextPageToken = data.nextPageToken;
        scanForZeroComments(data);
        while (nextPageToken != null)
        {
            url = "https://youtube.googleapis.com/youtube/v3/commentThreads?part=snippet&maxResults=10000&order=time&pageToken=" + nextPageToken + "&videoId=" + videoId + "&key=" + APIKey + "";
            response = await client.GetStringAsync(url);
            data = JsonConvert.DeserializeObject<Root>(response);

            nextPageToken = data.nextPageToken;
            scanForZeroComments(data);
        }

        int i = 1;
        foreach(string id in comments)
        {
            string link = "https://www.youtube.com/watch?v="+videoId+"&lc="+id;
            Console.WriteLine(i + ". " + link);
            i++;
        }
    }

    public static void scanForZeroComments(Root data)
    {
        foreach(Item temp in data.items)
        {
            if(temp.snippet.topLevelComment.snippet.likeCount == 0)
            {
                comments.Add(temp.id);
            }
        }
    }
}
