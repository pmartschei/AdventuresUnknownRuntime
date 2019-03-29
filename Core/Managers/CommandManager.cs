using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class CommandManager : SingletonBehaviour<CommandManager>
    {
        public static void AddCommand(ConsoleCommand consoleCommand)
        {
            Instance.AddCommandImpl(consoleCommand);
        }

        public static void ExecuteCommand(string command)
        {
            Instance.ExecuteCommandImpl(command);
        }

        public static List<ConsoleCommand> ListCommands()
        {
            return Instance.ListCommandsImpl();
        }
        public static List<ConsoleCommand> ListCommands(string startingName)
        {
            throw new NotImplementedException();
        }

        protected abstract void AddCommandImpl(ConsoleCommand consoleCommand);
        protected abstract void ExecuteCommandImpl(string command);
        protected abstract List<ConsoleCommand> ListCommandsImpl();
    }
}
