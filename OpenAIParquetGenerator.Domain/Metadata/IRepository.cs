namespace OpenAIParquetGenerator.Domain
{
    public interface IRepository<T>
    {
        public bool Insert(T objToSave);
        public bool Update(T objToSave);
        public bool Get();
        public bool Delete();
    }
}