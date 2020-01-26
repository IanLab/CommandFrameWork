
using CommandCore.Data;
using System;

namespace ConfigDTO
{
    public class CustomerDTO: ICommandAble
    {
        public string Name { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
