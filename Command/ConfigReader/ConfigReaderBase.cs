using ConfigDTO;
using System.Collections.Generic;

namespace Reader
{
    public abstract class ConfigReaderBase
    {
        public abstract IEnumerable<CustomerDTO> GetCustomers();
    }
}
