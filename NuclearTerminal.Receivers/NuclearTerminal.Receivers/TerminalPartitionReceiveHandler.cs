using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using NuclearTerminal.Receivers.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NuclearTerminal.Receivers
{
    public class TerminalPartitionReceiveHandler : IPartitionReceiveHandler
    {
        public TerminalPartitionReceiveHandler(string partitionId)
        {
            PartitionId = partitionId;
            MaxBatchSize = 10;
        }

        public int MaxBatchSize { get; set; }
        public string PartitionId { get; }

        public Task ProcessErrorAsync(Exception error)
        {
            Console.WriteLine($"Exception: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(IEnumerable<EventData> eventDatas)
        {
            if (eventDatas != null)
            {
                foreach (var eventData in eventDatas)
                {
                    var dataAsJson = Encoding.UTF8.GetString(eventData.Body.Array);
                    var terminalData = JsonConvert.DeserializeObject<TerminalData>(dataAsJson);

                    Console.WriteLine($"{terminalData} | PartitionId: {PartitionId} | Offset: {eventData.SystemProperties.Offset}");
                }
            }
            return Task.CompletedTask;
        }

    }
}
