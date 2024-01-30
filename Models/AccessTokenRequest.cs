namespace Main_Plaid.Models
{
    public class AccessTokenRequest
    {
        public string? client_id { get; set; }
        public string? ClientSecret { get; set; }
        public string? PublicKey { get; set; }
        public string? PublicToken { get; internal set; }
    }

    public class publictokenRequest
    {
        public string? public_token { get; set; }
        public string? institution_id { get; set; }
        public string[] initial_products { get; set; }
    }

    public class ResponcePublictoken
    {
        public string? public_token { get; set; }
        public string? request_id { get; set; }
    }

    public class ResponceAccessToken
    {
        public string? access_token { get; set; }
        public string? item_id { get; set; }
        public string? client_id { get; set; }
        public string? PublicKey { get; set; }
    }
}
