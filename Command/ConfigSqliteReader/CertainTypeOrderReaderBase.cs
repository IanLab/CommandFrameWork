using AutoMapper;
using System.Collections.Generic;
using AutoMapper.Configuration;
using System.Linq;
using OrderDTO;
using OrderSqliteDb;
using OrderEntity;
using System;
using Microsoft.EntityFrameworkCore;
using CommandCore.Data;

namespace SqliteReader
{
    public abstract class CertainTypeOrderReaderBase<TDTO, TEntity> : ICertainTypeOrderReader 
        where TDTO : OrderBaseDTO, new()
        where TEntity:class,IHasId
    {
        private readonly IMapper _mapper;
        private readonly string _orderTypeName;

        protected CertainTypeOrderReaderBase()
        {
            _orderTypeName = typeof(TDTO).Name;

            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<OrderCommonEntity, TDTO>();
            cfge.CreateMap<TEntity, TDTO>();
            var cfg = new MapperConfiguration(cfge);
            _mapper = new Mapper(cfg);            
        }

        public IEnumerable<OrderBaseDTO> GetOrders(OrderDbContext dbContext)
        {
            var commonEntities = (from o in dbContext.OrderCommons
                                  where o.OrderType == _orderTypeName && o.IsDeleted != true
                                  select o).ToArray();

            List<TDTO> dtos = new List<TDTO>(commonEntities.Length);
            var certainEntities = (from c in GetCertainOrderDbSet(dbContext) select c).ToList();

            foreach (var commonEntity in commonEntities)
            {
                var dto = _mapper.Map<TDTO>(commonEntity);
                var certainEntity = certainEntities.Single(i => i.Id == dto.Id);
                _mapper.Map(certainEntity, dto);
                certainEntities.Remove(certainEntity);
                dtos.Add(dto);
            }

            return dtos;
        }

        protected abstract DbSet<TEntity> GetCertainOrderDbSet(OrderDbContext dbContext);

        public OrderBaseDTO GetOrder(Guid id, OrderDbContext dbContext)
        {
            var commonEntity = dbContext.OrderCommons.SingleOrDefault(i => i.Id == id);
            if(commonEntity == null)
            {
                throw new Exception();
            }
            if(commonEntity.IsDeleted)
            {
                throw new Exception();
            }

            var certainEntity = GetCertainOrderDbSet(dbContext).Single(i => i.Id == id);

            var dto = _mapper.Map<TDTO>(commonEntity);
            _mapper.Map(certainEntity, dto);
            return dto;
        }
    }
}
