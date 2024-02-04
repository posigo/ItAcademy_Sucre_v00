namespace Sucre_DataAccess.Entities
{
    public interface IBaseEntityIn
    {
        public int Id { get; set; }
    }
    public interface IBaseEntityIn<T>
    {
        public T Id { get; set; }
    }
}
