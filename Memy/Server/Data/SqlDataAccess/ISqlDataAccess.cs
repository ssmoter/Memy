namespace Memy.Server.Data.SqlDataAccess
{
    public interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        string GetConnectionString();
        Task<T> LoadData<T>(string sql);
        Task<IList<T>> LoadDataList<T>(string sql);
        Task SaveData<T>(string sql, T parameters);
    }
}