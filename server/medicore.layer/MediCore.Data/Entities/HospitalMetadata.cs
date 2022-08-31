using dna.core.data.Abstract;
using MediCore.Data.Infrastructure;

namespace MediCore.Data.Entities
{

    /// <summary>
    /// ClinicMetadata save the non urgential information
    /// JsonValue is ClassToJson convert. Class is define on Model
    /// </summary>
    public class HospitalMetadata : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public MetaType MetaType { get; set; }
        public string JsonValue { get; set; }
    }
}
