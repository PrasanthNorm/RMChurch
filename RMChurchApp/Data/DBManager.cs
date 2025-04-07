namespace RMChurchApp.Data
{
    public class DBManager
    {
        private readonly IConfiguration _configuration;

        public DBManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetDBConnection()
        {
            try
            {
                return _configuration.GetConnectionString("DBCon");
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
