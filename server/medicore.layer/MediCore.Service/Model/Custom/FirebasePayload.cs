using dna.core.libs.Sender.ConfigType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model.Custom
{
    public class FirebasePayload
    {
        public FCMPayload Review { get; set; }
        public FCMPayload NextAppointment { get; set; }
        public FCMPayload Queue { get; set; }
    }
}
