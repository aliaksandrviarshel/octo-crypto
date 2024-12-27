namespace OctoCrypto.Configuration;

public class BingXApiOptions
{
    public const string BingXApi = "BingXApi";

    public string Url { get; set; }
    public string? ApiKey { get; set; }
    public string ApiSecret { get; set; }
}