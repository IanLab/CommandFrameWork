using AutoMapper;
using AutoMapper.Configuration;
using CommandCore.Data;
using CommandCore.Execute;
using ConfigDTO;
using ConfigSqliteDb;
using OrderDTO;
using OrderEntity;
using OrderSqliteDb;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OrderSqliteRepository
{
    public sealed class OrderARepository : IEntityRepository
    {
        private readonly OrderADTO _OrderA;
        private readonly CustomerDTO _customer;
        private readonly DateTime _updateDatetime;
        private readonly string _updateUser;

        private readonly OrderCommonEntity _lastUpdatedOrderCommonEntity;
        private readonly OrderAEntity _lastUpddatedOrderAEntity;
        private readonly OrderDbContext _orderDbContest;
        private readonly ConfigDbContext _configDbContext;
        private static readonly IMapper _mapper;


        static OrderARepository()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<OrderAEntity, OrderADTO>();
            cfge.CreateMap<OrderCommonEntity, OrderADTO>();
            cfge.CreateMap<OrderADTO, OrderAEntity>();
            cfge.CreateMap<OrderADTO, OrderCommonEntity>();
            var cfg = new MapperConfiguration(cfge);
            _mapper = new Mapper(cfg);
        }

        public OrderARepository(OrderADTO updateEntity, 
            IEnumerable<ICommandAble> references,
            DateTime updateDatetime,
            string updateUser, 
            OrderDbContext orderDbContext,
            ConfigDbContext configDbContext)
        {
            _OrderA = updateEntity;
            _customer = references.First() as CustomerDTO;
            _updateDatetime = updateDatetime;
            _updateUser = updateUser;

            _lastUpdatedOrderCommonEntity = _orderDbContest.OrderCommons
                .SingleOrDefault(i => i.Id == _OrderA.Id);
            _lastUpddatedOrderAEntity = _orderDbContest.OrderAs
                .SingleOrDefault(i => i.Id == _OrderA.Id);

            Debug.Assert(_lastUpdatedOrderCommonEntity != null ? 
                _lastUpddatedOrderAEntity != null : _lastUpddatedOrderAEntity == null);

            _orderDbContest = orderDbContext;
            _configDbContext = configDbContext;
        }

        public ICommandAble LastUpdated
        {
            get
            {
                if (_lastUpdatedOrderCommonEntity != null)
                {
                    var lastUpdated = _mapper.Map<OrderADTO>(_lastUpdatedOrderCommonEntity);
                    _mapper.Map(_lastUpddatedOrderAEntity, lastUpdated);
                    return lastUpdated;
                }

                return null;
            }
        }

        public IEnumerable<ICommandAble> NewerReferences
        {
            get
            {
                List<ICommandAble> newerReferences = new List<ICommandAble>();
                var newerCustomer = _configDbContext.Customers.SingleOrDefault(i => i.Id == _customer.Id
                && i.LastUpdateDateTime > _customer.LastUpdateDateTime);
                if (newerCustomer != null)
                {
                    newerReferences.Add(newerCustomer);
                }

                return newerReferences;
            }
        }

        public void Save()
        {
            if(_lastUpdatedOrderCommonEntity != null)
            {
                _mapper.Map(_OrderA, _lastUpdatedOrderCommonEntity);
                _mapper.Map(_OrderA, _lastUpddatedOrderAEntity);
                _lastUpdatedOrderCommonEntity.LastUpdateDateTime = _updateDatetime;
                _lastUpdatedOrderCommonEntity.LastUpdatedUser = _updateUser;                
            }
            else
            {
                var orderCommonEntity = _mapper.Map<OrderCommonEntity>(_OrderA);
                orderCommonEntity.LastUpdateDateTime = _updateDatetime;
                orderCommonEntity.LastUpdatedUser = _updateUser;
                var orderAEntity = _mapper.Map<OrderAEntity>(_OrderA);
                _orderDbContest.OrderCommons.Add(orderCommonEntity);
                _orderDbContest.OrderAs.Add(orderAEntity);
            }

            _orderDbContest.SaveChanges();
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(!_disposed)
                {
                    _orderDbContest.Dispose();
                    _configDbContext.Dispose();
                    _disposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~OrderARepository()
        {
            Dispose(false);
        }
    }
}
