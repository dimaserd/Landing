using System.Collections.Generic;

namespace Croco.Common.Options
{
    public class DbConnectionOptions
    {
        public Dictionary<string, DbConnection> Connections { get; set; }

        public class DbConnection
        {
            public DbType Type { get; set; }
            public string ConnectionString { get; set; }
            public string SqLiteFileDatabaseName { get; set; }
        }

        public enum DbType
        {
            SqLiteFile,
            SqlServer
        }
    }
}