using CommandCore.Data;
using System;
using System.Collections.Generic;

namespace CommandCore.Execute
{
    public class CommandExecutedEventArg
    {
        public CommandExecutedEventArg(ICommandAble entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            IsSuccess = true;
            ErrorMessage = string.Empty;
        }

        public ICommandAble Entity { get; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
