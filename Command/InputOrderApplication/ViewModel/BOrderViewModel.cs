using CommandCore.BroadcastBySharedFolder;
using CommandCore.Data;
using OrderDTO;

namespace InputOrderApplication.ViewModel
{
    public class BOrderViewModel : OrderBaseViewModel
    {
        private double _bOrderRate;
        public double BOrderRate
        {
            get { return _bOrderRate; }
            set
            {
                if(_bOrderRate != value)
                {
                    _bOrderRate = value;
                    RaisePropertyChanged();
                }
            }
        }
        public BOrderViewModel(
            IUserInfo userInfo,
            CommandSender cmmdSender,
            IViewModelMapperCreator mapperCreator) 
            : base(userInfo, cmmdSender, mapperCreator)
        {
        }

        protected override ICommandAble CreateUpdateEntity()
        {
            return _mapper.Map<BOrderDTO>(this);
        }
    }
}
