using ConfigDTO;
using ConfigSqliteDb;
using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Configuration;
using ConfigEntity;
using System.Linq;

namespace SqliteRepository
{
    public class CustomerRepository
    {
        private readonly ConfigDbContext _dbContext;
        private static readonly IMapper _mapper = CreateMapper();

        private static IMapper CreateMapper()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<CustomerEntity, CustomerDTO>();
            cfge.CreateMap<CustomerDTO, CustomerEntity>();
            var cfg = new MapperConfiguration(cfge);
            return new Mapper(cfg);
        }

        public CustomerRepository(ConfigDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<CustomerDTO> GetCustomers()
        {
            var entities = (from c in _dbContext.Customers select c).ToArray();
            return _mapper.Map<IEnumerable<CustomerDTO>>(entities);
        }
    }
}
