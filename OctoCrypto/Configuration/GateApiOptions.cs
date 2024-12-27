namespace OctoCrypto.Configuration;

public class GateApiOptions
{
    public const string GateApi = "GateApi"; 
    
    public string Url { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}