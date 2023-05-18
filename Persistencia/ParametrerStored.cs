using System.Data;

namespace Persistencia
{
    public class ParameterStored
    {
        public string? ParameterName { get; set; }

        public object? ParameterValue { get; set; }

        public DbType Type { get; set; }

        public ParameterDirection Direction { get; set; }
    }
}
