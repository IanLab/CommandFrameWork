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
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace  OrderSqliteRepository
{
    public sealed class OrderBRepository : IEntityRepository
    {
        private readonly OrderBDTO _OrderB;
        private readonly CustomerDTO _customer;
        private readonly DateTime _updateDatetime;
        private readonly string _updateUser;

        private readonly OrderCommonEntity _lastUpdatedOrderCommonEntity;
        private readonly OrderBEntity _lastUpddatedOrderBEntity;
        private readonly OrderDbContext _orderDbContest;
        private readonly ConfigDbContext _configDbContext;
        private static readonly IMapper _mapper;


        static OrderBRepository()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<OrderBEntity, OrderADTO>();
            cfge.CreateMap<OrderCommonEntity, OrderADTO>();
            cfge.CreateMap<OrderADTO, OrderBEntity>();
            cfge.CreateMap<OrderADTO, OrderCommonEntity>();
            var cfg = new MapperConfiguration(cfge);
            _mapper = new Mapper(cfg);
        }

        public OrderBRepository(OrderBDTO updateEntity,
            IEnumerable<ICommandAble> references,
            DateTime updateDatetime,
            string updateUser,
            OrderDbContext orderDbContext,
            ConfigDbContext configDbContext)
        {
            _OrderB = updateEntity;
            _customer = references.First() as CustomerDTO;
            _updateDatetime = updateDatetime;
            _updateUser = updateUser;

            _lastUpdatedOrderCommonEntity = _orderDbContest.OrderCommons
                .SingleOrDefault(i => i.Id == _OrderB.Id);
            _lastUpddatedOrderBEntity = _orderDbContest.OrderBs
                .SingleOrDefault(i => i.Id == _OrderB.Id);

            Debug.Assert(_lastUpdatedOrderCommonEntity != null ?
                _lastUpddatedOrderBEntity != null : _lastUpddatedOrderBEntity == null);

            _orderDbContest = orderDbContext;
            _configDbContext = configDbContext;
        }

        public ICommandAble LastUpdated
        {
            get
            {
                if (_lastUpdatedOrderCommonEntity != null)
                {
                    var lastUpdated = _mapper.Map<OrderBDTO>(_lastUpdatedOrderCommonEntity);
                    _mapper.Map(_lastUpddatedOrderBEntity, lastUpdated);
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
            if (_lastUpdatedOrderCommonEntity != null)
            {
                _mapper.Map(_OrderB, _lastUpdatedOrderCommonEntity);
                _mapper.Map(_OrderB, _lastUpddatedOrderBEntity);
                _lastUpdatedOrderCommonEntity.LastUpdateDateTime = _updateDatetime;
                _lastUpdatedOrderCommonEntity.LastUpdatedUser = _updateUser;
            }
            else
            {
                var orderCommonEntity = _mapper.Map<OrderCommonEntity>(_OrderB);
                orderCommonEntity.LastUpdateDateTime = _updateDatetime;
                orderCommonEntity.LastUpdatedUser = _updateUser;
                var orderBEntity = _mapper.Map<OrderBEntity>(_OrderB);
                _orderDbContest.OrderCommons.Add(orderCommonEntity);
                _orderDbContest.OrderBs.Add(orderBEntity);
            }

            _orderDbContest.SaveChanges();
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
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

        ~OrderBRepository()
        {
            Dispose(false);
        }
    }
}
