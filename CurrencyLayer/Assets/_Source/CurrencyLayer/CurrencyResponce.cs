using System.Collections.Generic;

namespace CurrencyLayer
{
    public class CurrencyResponse
    {
        public bool Success;
        public string Terms;
        public string Privacy;
        public long Timestamp;
        public string Source;
        public Dictionary<string, float> Quotes;
    }
}