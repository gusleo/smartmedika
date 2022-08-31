using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dna.core.libs.Upload.Abstract
{
    public interface IServerConfig
    {
        string ServerType { get; set; }
        string DestinationFolder { get; set; }
       
    }
}
