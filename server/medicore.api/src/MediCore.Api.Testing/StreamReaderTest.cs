using dna.core.libs.Stream;
using dna.core.libs.Stream.Option;
using MediCore.Api.CustomEntity;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MediCore.Api.Testing
{
    public class StreamReaderTest
    {
        private FileStream _file;
        private StreamAdvanceOption _options;

        public StreamReaderTest()
        {
            _file = new FileStream(@"C:\Users\IdaBagusLeo\Documents\Spesialist.xlsx", 
                FileMode.Open, FileAccess.Read);
           StreamAdvanceOption _options = new StreamAdvanceOption()
            {
                FirstRowAsColumnName = true
            };
        }

        
        [Fact]
        public void LoadExcelForSpesialistTest()
        {
            
            
            var builder = new AdvanceStreamBuilder<SpecialistExcelEntity>(_options);
            IList<SpecialistExcelEntity> result = builder.CreateReader(Path.GetExtension(_file.Name))
                                                    .Read(_file);
            Assert.NotEmpty(result);
           
        }

        [Fact]
        public void LoadExcelForPoliTest()
        {
            
            var builder = new AdvanceStreamBuilder<PoliclinicExcelEntity>(_options);
            IList<PoliclinicExcelEntity> result = builder.CreateReader(Path.GetExtension(_file.Name))
                                                    .Read(_file, 2);
            Assert.NotEmpty(result);

        }
    }
}
