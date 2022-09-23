using OpenAIParquetGenerator.Domain;
using OpenAIParquetGenerator.Repository.MongoDB;

namespace OpenAIParquetGenerator.Repository.Metadata
{
    public class MetadataRepository : GenericMongoDBRepository<Domain.Metadata>, IRepository<Domain.Metadata>
    {
        public MetadataRepository(
            string connectionString)
            : base(connectionString,
                  "OperationAIParquetGenerator",
                  "Metadata")
        {
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool Get()
        {
            throw new NotImplementedException();
        }

        public bool Insert(Domain.Metadata objToSave)
        {
            throw new NotImplementedException();
        }

        public bool Update(Domain.Metadata objToSave)
        {
            throw new NotImplementedException();
        }
    }
}