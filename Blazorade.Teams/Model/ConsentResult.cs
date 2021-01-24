namespace Blazorade.Teams.Model
{
    internal class ConsentResult
    {
        public bool Consented { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
    }
}