namespace OctoCrypto.Configuration;

public class MexcApiOptions
{
    public const string MexcApi = "MexcApi";

    public string Url { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}