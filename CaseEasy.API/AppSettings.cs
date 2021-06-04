using CaseEasy.Domain.Models;
using System.Collections.Generic;

namespace CaseEasy.API
{
    public class AppSettings
    {
        public Endpoint Fundos { get; set; }
        public Endpoint RendaFixa { get; set; }
        public Endpoint TesouroDireto { get; set; }
        public int CacheExpiration { get; set; }
    }

    public class Endpoint
    {
        public string Url { get; set; }
        public string RootTag { get; set; }
    }
}
