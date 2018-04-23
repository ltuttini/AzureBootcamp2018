using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;

namespace NuclearTerminal.FunctionApp
{
    public static class TerminalProcessMessages
    {
        [FunctionName("TerminalProcessMessages")]
        public static void Run([EventHubTrigger("nuclearterminal", Connection = "TerminalConnection", ConsumerGroup ="terminal_azure_function")]
        string[] eventHubMessage, 
            TraceWriter log)
        {
            foreach (var message in eventHubMessage)
            {
                var data = JsonConvert.DeserializeObject<TerminalData>(message);
                log.Info($"{data}");
            }
            
        }
    }
}
