namespace App.Data.Infrastructure
{
    public interface IDatabaseFactory
    {
        IDatabase Create();
    }
}
