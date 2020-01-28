using AutoMapper.Configuration;
using AutoMapper;
using OrderDTO;

namespace InputOrderApplication.ViewModel
{
    public class AOrderViewModelMapperCreator:IViewModelMapperCreator
    {
        private readonly IMapper _mapper;
        public IMapper Mapper
        {
            get { return _mapper; }
        }
        public AOrderViewModelMapperCreator()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<AOrderViewModel, AOrderDTO>();
            cfge.CreateMap<AOrderDTO, AOrderViewModel>();
            var cfg = new MapperConfiguration(cfge);
            _mapper = new Mapper(cfg);
        }
    }
}
