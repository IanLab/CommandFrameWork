using CommandCore.BroadcastBySharedFolder;
using CommandCore.Data;
using ConfigDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace InputOrderApplication.ViewModel
{
    public abstract class OrderBaseViewModel : CommandAbleViewModel
    {
        private string no;
        private DateTime startDate;
        private DateTime maturityDate;
        private double amount;
        private string customerName;
        private string customerAccount;
        private string customerEmail;
        private string owner;
        private string lastUpdatedUser;
        private DateTime lastUpdateDateTime;
        private bool isDeleted;
        protected CustomerDTO _customer;

        public string No
        {
            get => no; set
            {
                if (no != value)
                {
                    no = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DateTime MaturityDate
        {
            get => maturityDate; set
            {
                if (maturityDate != value)
                {
                    maturityDate = value;
                    RaisePropertyChanged();
                }
            }
        }
        public double Amount
        {
            get => amount;
            set
            {
                if (amount != value)
                {
                    amount = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string CustomerName
        {
            get => customerName;
            set
            {
                if (customerName != value)
                {
                    customerName = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string CustomerAccount
        {
            get => customerAccount; set
            {
                if (customerAccount != value)
                {
                    customerAccount = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string CustomerEmail
        {
            get => customerEmail; set
            {
                if (customerEmail != value)
                {
                    customerEmail = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Owner
        {
            get => owner;
            set
            {
                if (owner != value)
                {
                    owner = value;
                    RaisePropertyChanged();
                }
            }
        }
        public Guid Id { get; set; }
        public string LastUpdatedUser
        {
            get => lastUpdatedUser; set
            {
                if (lastUpdatedUser != value)
                {
                    lastUpdatedUser = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DateTime LastUpdateDateTime
        {
            get => lastUpdateDateTime; set
            {
                if (lastUpdateDateTime != value)
                {
                    lastUpdateDateTime = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsDeleted
        {
            get => isDeleted; set
            {
                if (isDeleted != value)
                {
                    isDeleted = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected override void CleanReferences()
        {
            _customer = null;
        }

        public void SetCustomer(CustomerDTO customer)
        {
            this.CustomerAccount = customer.Account;
            this.CustomerEmail = customer.Email;
            this.CustomerName = customer.Name;
            this._customer = customer;
        }

        protected override IEnumerable<ICommandAble> GetReferences()
        {
            return new ICommandAble[] { this._customer };
        }

        protected OrderBaseViewModel(IUserInfo userInfo, CommandSender cmmdSender,
            IViewModelMapperCreator mapperCreator)
            : base(userInfo, 
                  cmmdSender,
                  mapperCreator)
        {
        }
    }
}
