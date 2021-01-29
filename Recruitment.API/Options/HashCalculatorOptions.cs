namespace Recruitment.API.Options
{
    public class HashCalculatorOptions : IHashCalculatorOptions
    {
        public string BaseUrl { get; set; }
        public string CalculateHashUri { get; set; }
    }
}