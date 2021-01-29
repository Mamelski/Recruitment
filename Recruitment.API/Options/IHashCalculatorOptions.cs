namespace Recruitment.API.Options
{
    public interface IHashCalculatorOptions
    { 
        string BaseUrl { get; set; }
        public string CalculateHashUri { get; set; }
    }
}