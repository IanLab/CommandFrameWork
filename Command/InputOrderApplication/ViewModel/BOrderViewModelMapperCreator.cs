using AutoMapper.Configuration;
using AutoMapper;
using OrderDTO;

namespace InputOrderApplication.ViewModel
{
    public class BOrderViewModelMapperCreator:IViewModelMapperCreator
    {
        private readonly IMapper _mapper;
        public IMapper Mapper
        {
            get { return _mapper; }
        }

        public BOrderViewModelMapperCreator()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<BOrderDTO, OrderBaseViewModel>();
            cfge.CreateMap<OrderBaseViewModel, BOrderDTO>();
            var cfg = new MapperConfiguration(cfge);
            _mapper = new Mapper(cfg);
        }
    }
}
