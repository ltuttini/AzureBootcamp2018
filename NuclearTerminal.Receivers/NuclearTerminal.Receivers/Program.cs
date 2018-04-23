using Microsoft.Azure.EventHubs;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;

namespace NuclearTerminal.Receivers
{
    class Program
    {
        private static string eventHubConnectionString;

        static void Main(string[] args)
        {
            eventHubConnectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Connecting to the Event Hub...");

            var eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString);
            var runtimeInformation = await eventHubClient.GetRuntimeInformationAsync();

            var partitionReceivers = runtimeInformation.PartitionIds
                                                        .Select(partitionId => 
                                                                    eventHubClient.CreateReceiver("terminaldirect", partitionId, EventPosition.FromEnd())).ToList();

            Console.WriteLine("Waiting for incoming events...");

            foreach (var partitionReceiver in partitionReceivers)
            {
                partitionReceiver.SetReceiveHandler(new TerminalPartitionReceiveHandler(partitionReceiver.PartitionId));
            }

            Console.WriteLine("Press any key to shutdown");
            Console.ReadLine();
            await eventHubClient.CloseAsync();
        }
    }
}
