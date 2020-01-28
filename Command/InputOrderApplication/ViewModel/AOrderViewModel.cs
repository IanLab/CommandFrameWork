using CommandCore.BroadcastBySharedFolder;
using CommandCore.Data;
using System;
using OrderDTO;
using System.Collections.Generic;

namespace InputOrderApplication.ViewModel
{
    public class AOrderViewModel : OrderBaseViewModel
    {
        private string _orderAP1;
        public string OrderAP1
        {
            get { return _orderAP1; }
            set
            {
                if(_orderAP1 != value)
                {
                    _orderAP1 = value;
                    RaisePropertyChanged();
                }
            }
        }
        public AOrderViewModel(IUserInfo userInfo,
            CommandSender cmmdSender,
            IViewModelMapperCreator mapperCreator)
            : base(userInfo, 
                  cmmdSender, 
                  mapperCreator)
        {
        }


        protected override ICommandAble CreateUpdateEntity()
        {
            return _mapper.Map<AOrderDTO>(this);
        }

        protected override IEnumerable<ICommandAble> GetReferences()
        {
            return base.GetReferences();
        }

    }
}
