using CommandCore.BroadcastBySharedFolder;
using CommandCore.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoMapper;

namespace InputOrderApplication.ViewModel
{
    public abstract class CommandAbleViewModel : ViewModelBase
    {
        private ICommandAble backUpData;
        private readonly IUserInfo _userInfo;
        private readonly CommandSender _cmmdSender;
        protected readonly IMapper _mapper;
        public bool IsEdited { get { return backUpData != null; } }
        protected ICommandAble BackUpData 
        { 
            get => backUpData;
            set
            {
                if(backUpData != value)
                {
                    backUpData = value;
                    RaisePropertyChanged(nameof(IsEdited));
                }
            }
        }
        protected abstract IEnumerable<ICommandAble> GetReferences();
        protected abstract void CleanReferences();
        protected CommandAbleViewModel(IUserInfo userInfo, 
            CommandSender cmmdSender, 
            IViewModelMapperCreator mapperCreator)
        {
            _mapper = mapperCreator.Mapper;
            _userInfo = userInfo ?? throw new ArgumentNullException(nameof(userInfo));
            _cmmdSender = cmmdSender ?? throw new ArgumentNullException(nameof(cmmdSender));
            BackUpData = null;
        }

        public override void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.RaisePropertyChanged(propertyName);
            if (BackUpData == null)
            {
                BackUp();
            }
        }

        protected void BackUp()
        {
            System.Diagnostics.Debug.Assert(this.BackUpData == null);

            _mapper.Map(this, this.BackUpData);
        }


        protected abstract ICommandAble CreateUpdateEntity();

        public void CancelEdit()
        {
            if(this.BackUpData != null)
            {
                _mapper.Map(this.BackUpData, this);
                this.CleanReferences();
            }
        }

        public void SendCommand()
        {
            var updateEntity = CreateUpdateEntity();
            var entityWithReferences = new EntityWithReferences
            {
                Entity = updateEntity,
                References = GetReferences()
            };
            CleanReferences();
            BackUpData = null;
            _cmmdSender.Send(new Command(_userInfo.User, DateTime.Now, entityWithReferences));
        }
    }
}
