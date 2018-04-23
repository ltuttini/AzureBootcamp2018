using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearTerminal.EventHub.Sender
{
    public class TerminalDataSender : ITerminalDataSender
    {
        private EventHubClient _eventHubClient;

        public TerminalDataSender(string eventHubConnectionString)
        {
            _eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString);
        }
        public async Task SendDataAsync(TerminalData data)
        {
            EventData eventData = CreateEventData(data);
            await _eventHubClient.SendAsync(eventData);
        }

        public async Task SendDataAsync(IEnumerable<TerminalData> datas)
        {
            var eventDatas = datas.Select(terminalData => CreateEventData(terminalData));

            //CreateBatch() ya tiene definido el tamaño correcto para el mensaje
            var eventDataBatch = _eventHubClient.CreateBatch();

            foreach (var eventData in eventDatas)
            {
                if (!eventDataBatch.TryAdd(eventData))
                {
                    await _eventHubClient.SendAsync(eventDataBatch);
                    eventDataBatch = _eventHubClient.CreateBatch();
                    eventDataBatch.TryAdd(eventData);
                }
            }

            if (eventDataBatch.Count > 0)
            {
                await _eventHubClient.SendAsync(eventDataBatch);
            }
        }

        private static EventData CreateEventData(TerminalData data)
        {
            var dataAsJson = JsonConvert.SerializeObject(data);
            var eventData = new EventData(Encoding.UTF8.GetBytes(dataAsJson));

            return eventData;
        }
    }
}
