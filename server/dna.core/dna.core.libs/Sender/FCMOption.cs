using System;
using System.Collections.Generic;
using System.Text;

namespace dna.core.libs.Sender
{
    public class FCMOption
    {
        public static string PriorityHigh = "High";
        public string TargetScreen { get; set; }
        public string JsonData { get; set; }
        public string Priority { get; set; }

    }
}
