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
    public sealed class AOrderRepository : IEntityRepository
    {
        private readonly AOrderDTO _AOrder;
        private readonly CustomerDTO _customer;
        private readonly DateTime _updateDatetime;
        private readonly string _updateUser;

        private readonly OrderCommonEntity _lastUpdatedOrderCommonEntity;
        private readonly AOrderEntity _lastUpddatedOrderAEntity;
        private readonly OrderDbContext _orderDbContest;
        private readonly ConfigDbContext _configDbContext;
        private static readonly IMapper _mapper;


        static AOrderRepository()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<AOrderEntity, AOrderDTO>();
            cfge.CreateMap<OrderCommonEntity, AOrderDTO>();
            cfge.CreateMap<AOrderDTO, AOrderEntity>();
            cfge.CreateMap<AOrderDTO, OrderCommonEntity>();
            var cfg = new MapperConfiguration(cfge);
            _mapper = new Mapper(cfg);
        }

        public AOrderRepository(AOrderDTO updateEntity, 
            IEnumerable<ICommandAble> references,
            DateTime updateDatetime,
            string updateUser, 
            OrderDbContext orderDbContext,
            ConfigDbContext configDbContext)
        {
            _AOrder = updateEntity;
            _customer = references.First() as CustomerDTO;
            _updateDatetime = updateDatetime;
            _updateUser = updateUser;

            _lastUpdatedOrderCommonEntity = _orderDbContest.OrderCommons
                .SingleOrDefault(i => i.Id == _AOrder.Id);
            _lastUpddatedOrderAEntity = _orderDbContest.AOrders
                .SingleOrDefault(i => i.Id == _AOrder.Id);

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
                    var lastUpdated = _mapper.Map<AOrderDTO>(_lastUpdatedOrderCommonEntity);
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
                _mapper.Map(_AOrder, _lastUpdatedOrderCommonEntity);
                _mapper.Map(_AOrder, _lastUpddatedOrderAEntity);
                _lastUpdatedOrderCommonEntity.LastUpdateDateTime = _updateDatetime;
                _lastUpdatedOrderCommonEntity.LastUpdatedUser = _updateUser;                
            }
            else
            {
                OrderNoHelper.SetNo(_orderDbContest,_AOrder);
                var orderCommonEntity = _mapper.Map<OrderCommonEntity>(_AOrder);
                orderCommonEntity.LastUpdateDateTime = _updateDatetime;
                orderCommonEntity.LastUpdatedUser = _updateUser;
                var orderAEntity = _mapper.Map<AOrderEntity>(_AOrder);
                _orderDbContest.OrderCommons.Add(orderCommonEntity);
                _orderDbContest.AOrders.Add(orderAEntity);
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

        ~AOrderRepository()
        {
            Dispose(false);
        }
    }
}
