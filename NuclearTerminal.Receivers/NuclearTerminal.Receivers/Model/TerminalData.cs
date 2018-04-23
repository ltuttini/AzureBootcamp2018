using System;
using System.Collections.Generic;
using System.Text;

namespace NuclearTerminal.Receivers.Model
{
    public class TerminalData
    {
        public string City { get; set; }
        public string Sector { get; set; }
        public string SensorType { get; set; }
        public int SensorValue { get; set; }
        public DateTime CreateDateTime { get; set; }

        public override string ToString()
        {
            return $"Time: {CreateDateTime:HH:mm:ss} | {SensorType}: {SensorValue} | "
                 + $"City: {City} | Sector: {Sector} ";
        }
    }
}
