using System.Collections.Generic;

namespace Azure_function_logs.Models
{
    public class ApiResponse
    {
    }
    
    public class Entry
    {
        public string API { get; set; }
        public string Description { get; set; }
        public string Auth { get; set; }
        public bool HTTPS { get; set; }
        public string Cors { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
    }

    public class Root
    {
        public int count { get; set; }
        public List<Entry> entries { get; set; }
    }


}