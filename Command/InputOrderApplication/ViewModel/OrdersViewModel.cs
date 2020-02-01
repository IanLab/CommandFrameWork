using AutoMapper;
using OrderSqliteDb;
using SqliteReader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper.Configuration;
using OrderDTO;

namespace InputOrderApplication.ViewModel
{

    public class OrdersViewModel:ViewModelBase
    {
        private readonly IMapper _mapper; 
        private readonly OrderDbContext.Factory _orderDbContextFactory;
        public ObservableCollection<OrderBaseViewModel> Orders { get; private set; }
        private readonly OrderReader _orderReader;
        private OrderBaseViewModel _selectedOrder;
        public OrderBaseViewModel SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if(_selectedOrder != value)
                {
                    _selectedOrder = value;
                    RaisePropertyChanged();

                    if(_selectedOrder != null)
                    {
                        _selectedOrder.SendCommand();
                    }
                }
            }
        }        
        
        private DateTime _startDateTime;
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set
            {
                if(_startDateTime != value)
                {
                    _startDateTime = value;
                    RaisePropertyChanged();
                    ReloadOrder();
                }
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        public OrdersViewModel(OrderDbContext.Factory orderDbContextFactory,
            OrderReader orderReader)
        {
            _orderDbContextFactory = orderDbContextFactory ?? throw new ArgumentNullException(nameof(orderDbContextFactory));
            _orderReader = orderReader ?? throw new ArgumentNullException(nameof(orderReader));

            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<OrderBaseDTO, OrderBaseViewModel>()
                .Include<AOrderDTO, AOrderViewModel>()
                .Include<BOrderDTO, BOrderViewModel>();
            cfge.CreateMap<AOrderDTO, AOrderViewModel>();
            cfge.CreateMap<BOrderDTO, BOrderViewModel>();
            var cfg = new MapperConfiguration(cfge);
            _mapper = new Mapper(cfg);

            this.StartDateTime = DateTime.Now;

            SaveCommand = new RelayCommand(OnCanSave, OnSave);
        }

        private void OnSave(object obj)
        {
            if (SelectedOrder == null)
            {
                SelectedOrder.SendCommand();
            }
        }

        private bool OnCanSave(object obj)
        {
            if(SelectedOrder == null)
            {
                return false;
            }

            return SelectedOrder.IsEdited;
        }

        private void ReloadOrder()
        {
            Orders.Clear();

            using var dbConext = _orderDbContextFactory(_startDateTime);
            var orderDTOs = _orderReader.GetOrders(dbConext);
            var orderViewModels = _mapper.Map<IEnumerable<OrderBaseViewModel>>(orderDTOs);
            foreach(var ovm in orderViewModels)
            {
                Orders.Add(ovm);
            }
        }
    }
}
