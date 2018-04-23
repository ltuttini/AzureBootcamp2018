using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NuclearTerminal.EventHub.Sender
{
    public class TerminalDataSenderMock : ITerminalDataSender
    {
        public TerminalDataSenderMock(string eventHubConnectionString)
        {
            
        }

        public Task SendDataAsync(TerminalData data)
        {
            return Task.CompletedTask;
        }

        public Task SendDataAsync(IEnumerable<TerminalData> datas)
        {
            return Task.CompletedTask;
        }
    }
}
