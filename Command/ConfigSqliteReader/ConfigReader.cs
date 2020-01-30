using AutoMapper;
using ConfigDTO;
using Reader;
using System.Collections.Generic;
using AutoMapper.Configuration;
using ConfigEntity;
using System.Linq;
using OrderDTO;
using System;
using OrderSqliteDb;
using OrderEntity;

namespace ConfigSqliteReader
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

        private readonly ConfigSqliteDb.ConfigDbContext _dbContext;
        public override IEnumerable<CustomerDTO> GetCustomers()
        {
            var cs = (from c in _dbContext.Customers where c.IsDeleted != true select c).ToArray();
            return _mapper.Map<IEnumerable<CustomerDTO>>(cs);
        }
    }


    internal abstract class CertainTypeOrderReaderBase<DTO>
        where DTO : OrderBaseDTO, new()
    {
        private readonly IMapper _mapper;
        private readonly string _orderTypeName;

        private readonly OrderDbContext _dbContext;

        public CertainTypeOrderReaderBase(OrderDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            _orderTypeName = typeof(DTO).Name;

            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<OrderCommonEntity, DTO>();
            CreateMapFromCertainTypeOrderTable(cfge);
            var cfg = new MapperConfiguration(cfge);
            _mapper = new Mapper(cfg);
        }

        protected abstract void CreateMapFromCertainTypeOrderTable(MapperConfigurationExpression cfge);

        public IEnumerable<OrderBaseDTO> GetOrders()
        {
            var orderCommons = (from o in _dbContext.OrderCommons where o.OrderType == _orderTypeName && o.IsDeleted != true select o);
            var certainOrderEntities = GetCertainTypeOrderEntities();

            List<order>
        }

        protected abstract IEnumerable<object> GetCertainTypeOrderEntities();
    }

    public class OrderReader : OrderReaderBase
    {
        private readonly static IMapper _mapper = CreateMapper();

        private static IMapper CreateMapper()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<OrderEntity.OrderCommonEntity, AOrderDTO>().;
            var cfg = new MapperConfiguration(cfge);
            return new Mapper(cfg);
        }

        public override IEnumerable<OrderBaseDTO> GetOrders(DateTime dt)
        {
            throw new NotImplementedException();
        }
    }
}
