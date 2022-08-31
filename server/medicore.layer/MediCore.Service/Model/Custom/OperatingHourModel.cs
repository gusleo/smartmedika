using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model.Custom
{
    public class OperatingHourModel
    {
        //day integer https://msdn.microsoft.com/en-us/library/system.dayofweek.aspx
        public int Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsClossed { get; set; }
    }
}
