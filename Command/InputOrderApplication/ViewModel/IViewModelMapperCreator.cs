using AutoMapper;

namespace InputOrderApplication.ViewModel
{
    public interface IViewModelMapperCreator
    {
        public IMapper Mapper
        {
            get;
        }
    }
}
