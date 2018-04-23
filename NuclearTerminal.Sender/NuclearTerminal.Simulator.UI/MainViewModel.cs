using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using NuclearTerminal.EventHub.Sender;

namespace NuclearTerminal.Simulator.UI
{
    public class MainViewModel : BindableBase
    {
        private int _counterInternalCall;
        private int _counterExternalCall;
        private string _city;
        private string _sector;
        private int _temprature;
        private int _electricity;
        private bool _sending;
        private ITerminalDataSender _terminalDataSender;
        private DispatcherTimer _dispatcherTimer;

        public MainViewModel(ITerminalDataSender terminalDataSender)
        {
            _terminalDataSender = terminalDataSender;

            Sector = "7G";
            City = "Springfield";

            ExternalCallCommand = new DelegateCommand(MakeExternalCall);
            InternalCallCommand = new DelegateCommand(MakeInternalCall);

            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var tempratureData = CreateTerminalData(nameof(Temprature), Temprature);
            var electricityData = CreateTerminalData(nameof(Electricity), Electricity);

            await SendDataAsync(new[] { tempratureData, electricityData });
        }

        public ICommand InternalCallCommand { get; }

        public ICommand ExternalCallCommand { get; }

        public ObservableCollection<string> Logs { get; }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                RaisePropertyChanged();
            }
        }

        public string Sector
        {
            get { return _sector; }
            set
            {
                _sector = value;
                RaisePropertyChanged();
            }
        }

        public int CounterInternalCall
        {
            get { return _counterInternalCall; }
            set
            {
                _counterInternalCall = value;
                RaisePropertyChanged();
            }
        }

        public int CounterExternalCall
        {
            get { return _counterExternalCall; }
            set
            {
                _counterExternalCall = value;
                RaisePropertyChanged();
            }
        }

        public int Temprature
        {
            get { return _temprature; }
            set
            {
                _temprature = value;
                RaisePropertyChanged();
            }
        }

        public int Electricity
        {
            get { return _electricity; }
            set
            {
                _electricity = value;
                RaisePropertyChanged();
            }
        }

        public bool Sending
        {
            get { return _sending; }
            set
            {
                if (_sending != value)
                {
                    _sending = value;
                    if (_sending)
                    {
                        _dispatcherTimer.Start();
                    }
                    else
                    {
                        _dispatcherTimer.Stop();
                    }

                    RaisePropertyChanged();
                }
            }
        }


        private async void MakeExternalCall()
        {
            CounterExternalCall++;
            TerminalData terminalData = CreateTerminalData(nameof(CounterExternalCall), CounterExternalCall);
            await SendDataAsync(terminalData);
        }

        private async void MakeInternalCall()
        {
            CounterInternalCall++;
            TerminalData terminalData = CreateTerminalData(nameof(CounterInternalCall), CounterInternalCall);
            await SendDataAsync(terminalData);
        }

        private TerminalData CreateTerminalData(string sensorType, int sensorValue)
        {
            return new TerminalData
            {
                City = City,
                Sector = Sector,
                SensorType = sensorType,
                SensorValue = sensorValue,
                CreateDateTime = DateTime.Now
            };
        }

        private async Task SendDataAsync(TerminalData terminalData)
        {
            try
            {
                await _terminalDataSender.SendDataAsync(terminalData);
                WriteLog($"Sent data: {terminalData}");
            }
            catch (Exception ex)
            {
                WriteLog($"Exception: {ex.Message}");
            }
        }

        private async Task SendDataAsync(IEnumerable<TerminalData> terminalDatas)
        {
            try
            {
                await _terminalDataSender.SendDataAsync(terminalDatas);
                foreach (var terminalData in terminalDatas)
                {
                    WriteLog($"Sent data: {terminalData}");
                }
            }
            catch (Exception ex)
            {
                WriteLog($"Exception: {ex.Message}");
            }
        }

        private void WriteLog(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}
