namespace Core.Utulities.Result
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
