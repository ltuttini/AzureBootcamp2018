using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NuclearTerminal.EventHub.Sender
{
    public interface ITerminalDataSender
    {
        Task SendDataAsync(TerminalData data);
        Task SendDataAsync(IEnumerable<TerminalData> datas);
    }
}
