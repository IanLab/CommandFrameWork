using AutoMapper;
using ConfigDTO;
using Reader;
using System.Collections.Generic;
using AutoMapper.Configuration;
using ConfigEntity;
using System.Linq;
using ConfigSqliteDb;
using System;

namespace SqliteReader
{
    public class ConfigReader : ConfigReaderBase
    {
        private readonly static IMapper _mapper = CreateMapper();

        private static IMapper CreateMapper()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<CustomerEntity, CustomerDTO>();
            var cfg = new MapperConfiguration(cfge);
            return new Mapper(cfg);
        }

        private readonly ConfigDbContext _dbContext;

        public ConfigReader(ConfigDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public override IEnumerable<CustomerDTO> GetCustomers()
        {
            var cs = (from c in _dbContext.Customers where c.IsDeleted != true select c).ToArray();
            return _mapper.Map<IEnumerable<CustomerDTO>>(cs);
        }
    }
}
