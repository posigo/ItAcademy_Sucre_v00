namespace Sucre_DataAccess.Services.IServices
{
    public interface IUtilApplicattionDbContext
    {
        string DatabaseName {  get; }

        bool InitDbValue(out string errMsg);
    }
}
