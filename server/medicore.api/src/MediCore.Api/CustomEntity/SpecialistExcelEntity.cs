using dna.core.libs.Stream;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.CustomEntity
{
    /// <summary>
    /// Class of Excel entities
    /// <remarks>
    /// The order of enties must same as excel column
    /// </remarks>
    /// </summary>
    public class SpecialistExcelEntity : IStreamEntity
    {
        [NotMapped]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
       
    }
}
