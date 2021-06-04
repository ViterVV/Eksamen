using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace _6
{
    public class MainViewModel
    {
        private Thread a, b;
        private List<double> _y = new List<double>();
        private double startValue = 0.01;
        private ObservableCollection<DataPoint> _pts;
        private RelayCommand startStop;
        private bool isStopped = false;
        public RelayCommand StartStop
        {
            get
            {
                return startStop ?? (startStop = new RelayCommand(obj => {
                    if (isStopped)
                    {
                        a.Resume();
                        b.Resume();
                        isStopped = !isStopped;
                    }
                    else
                    {
                        a.Suspend();
                        b.Suspend();
                        isStopped = !isStopped;
                    }
                }));
            }
        }
        private void GenerateY() {
            while (true)
            {
                
                _y.Add(FunctionValue(startValue));
                Thread.Sleep(500);
                startValue += 0.01;
            }
        }
        private void Update_Graph()
        {
            while (true)
            {
                if (App.Current != null)
                    App.Current.Dispatcher.Invoke(new Action(() => Points.Add(new DataPoint(startValue, _y[_y.Count - 1]))));
                else Environment.Exit(0);
                Thread.Sleep(50);
            }
        }
        private double FunctionValue(double x) => (23 * x * 2) - 33;
        
        public ObservableCollection<DataPoint> Points
        {
            get => _pts;
            set
            {
                _pts = value;
                OnPropertyChanged(nameof(Points));
            }
        }
        public MainViewModel()
        {
            Points = new ObservableCollection<DataPoint>();
            Title = "MyGraph";
            a = new Thread(GenerateY);
            b = new Thread(Update_Graph);
            a.Start(); b.Start();
            
        }
        public string Title { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
