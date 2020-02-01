using Reader;
using System.Collections.Generic;
using OrderDTO;
using System;
using OrderSqliteDb;
using Autofac;
using Autofac.Features.Metadata;
using System.Linq;

namespace SqliteReader
{
    public class OrderReader 
    {
        private readonly IEnumerable<Meta<ICertainTypeOrderReader>> _certainTypeOrderReaders;
        public delegate OrderReader Factory(OrderDbContext dbContext);

        public const string ReaderMetadataName = "OrderTypeName";

        public OrderReader(IEnumerable<Meta<ICertainTypeOrderReader>> certainTypeOrderReaders)
        {           
            _certainTypeOrderReaders = certainTypeOrderReaders ?? throw new ArgumentNullException(nameof(certainTypeOrderReaders));
        }

        public IEnumerable<OrderBaseDTO> GetOrders(OrderDbContext dbContext)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            List<OrderBaseDTO> dtos = new List<OrderBaseDTO>();
            foreach(var readers in _certainTypeOrderReaders)
            {
                dtos.AddRange(readers.Value.GetOrders(dbContext));
            }
            return dtos;
        }

        public OrderBaseDTO GetOrder(Guid id, string orderTypeName, OrderDbContext dbContext)
        {
            var reader = (from i in _certainTypeOrderReaders 
                          where i.Metadata["ReaderMetadataName"].Equals(orderTypeName) select i.Value).FirstOrDefault();

            if(reader == null)
            {
                throw new Exception();
            }

            return reader.GetOrder(id, dbContext);
        }
    }
}
