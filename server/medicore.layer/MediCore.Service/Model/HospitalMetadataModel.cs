using dna.core.service.Models.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class HospitalMetadataModel : WriteHistoryBaseModel, IModelBase
    {

        public int Id { get; set; }
        public int HospitalId { get; set; }
        public MetaType MetaType { get; set; }
        public string JsonValue { get; set; }
    }
}
