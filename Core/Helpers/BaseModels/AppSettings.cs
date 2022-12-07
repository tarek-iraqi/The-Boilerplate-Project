using System.Collections.Generic;

namespace Helpers.BaseModels;

public class AppSettings
{
    public EmailSettings EmailSettings { get; set; }
    public List<ApiClient> ApiClients { get; set; }
    public JWTSettings JWTSettings { get; set; }
    public string Api_URL { get; set; }
    public string[] UrlsSkipApiKey { get; set; }
}

public class EmailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string FromEmail { get; set; }
    public string Password { get; set; }
}

public class ApiClient
{
    public string Name { get; set; }
    public string ApiKey { get; set; }
}

public class JWTSettings
{
    public string SecretHashKey { get; set; }
    public int DurationInMillisecond { get; set; }
}

public class Firebase_Settings
{
    public string type { get; set; }
    public string project_id { get; set; }
    public string private_key_id { get; set; }
    public string private_key { get; set; }
    public string client_email { get; set; }
    public string client_id { get; set; }
    public string auth_uri { get; set; }
    public string token_uri { get; set; }
    public string auth_provider_x509_cert_url { get; set; }
    public string client_x509_cert_url { get; set; }
}

public class AmazonSettings
{
    public string AWS_REGION { get; set; }
    public string AWS_SECRET_ACCESS_KEY { get; set; }
    public string AWS_ACCESS_KEY_ID { get; set; }
    public string AWS_BUCKET { get; set; }
    public string AWS_FILE_PATH { get; set; }
}