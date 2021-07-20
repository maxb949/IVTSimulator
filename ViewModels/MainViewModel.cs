using IvtSimulator.Models;
using IvtSimulator.Views;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Controls;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;


namespace IvtSimulator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private SimModel _simModel;

        private DateTime _simTime;
        private string _status;

        private DispatcherTimer _iterationPoolingTimer; // таймер численного метода
        private Page _currentPage;

        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }
        private SeriesCollection _graphCollection;
        public string[] ChartLabels { get; set; }
        public string[] ChartShortLabels { get; set; }
        public Func<double, string> Formatter { get; set; }

        


        public Page CurrentPage 
        { 
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(() => CurrentPage);
            }
        }

        public SeriesCollection GraphCollection 
        { 
            get => _graphCollection; 
            set => _graphCollection = value; 
        }


        public MainViewModel()
        {
            Status = "Инициализация симулятора";
            // инициализируем таймер с периодом 1 сек.
            _iterationPoolingTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 1, 0)
            };
            // определям делегата для обработки событий таймера
            _iterationPoolingTimer.Tick += IterationPoolingTimerTick;

            CurrentPage = new MainPage()
            {
                DataContext = this,
            };

            //Инициализируем модель процесса
            _simModel = new SimModel()
            {
                //Инициализация давлений источников и потребителей
                P1=200000,
                P2 = 200000,
                P3 = 100000,
                P4 = 100000,
                PB1 = 120000,
                PB2 = 120000,
            };
            SimTime = new DateTime(0);

            //создание графиков
            Formatter = value => ((double)value).ToString("P1");
            _graphCollection = new SeriesCollection();

            _graphCollection.Add(_simModel.L1.Trend);
            _graphCollection.Add(_simModel.L2.Trend);


        }

        #region INotifyPropertyChanged interface

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged<T>(Expression<Func<T>> ptyAccessor)
        {
            var name = ((MemberExpression)ptyAccessor.Body).Member.Name;
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        #endregion INotifyPropertyChanged interface

        //Запуск вычслительной интерации по таймеру
        private void IterationPoolingTimerTick(object sender, EventArgs e)
        {
            _simModel.Iteration();
            SimTime = SimTime.AddSeconds(1); // увеличиваем счетчик времени моделирования
            //Принудительная перерисовка контролов
            OnPropertyChanged(()=>L1);
            OnPropertyChanged(() => L2);
        }

        public double L1
        {
            get => _simModel.L1.Value;
        }

        public double L2
        {
            get => _simModel.L2.Value;
        }

        public double K1
        {
            get => _simModel.K1;
            set
            {
                _simModel.K1 = value;
                OnPropertyChanged(() => K1);
                Status = $"Изм. положение V1 - {K1:P0}";
            }
        }

        public double K2
        {
            get => _simModel.K2;
            set
            {
                _simModel.K2 = value;
                OnPropertyChanged(() => K2);
                Status = $"Изм. положение V2 - {K2:P0}";
            }
        }
        public double K3
        {
            get => _simModel.K3;
            set
            {
                _simModel.K3 = value;
                OnPropertyChanged(() => K3);
                Status = $"Изм. положение V3 - {K3:P0}";
            }
        }
        public double K4
        {
            get => _simModel.K4;
            set
            {
                _simModel.K4 = value;
                OnPropertyChanged(() => K4);
                Status = $"Изм. положение V4 - {K4:P0}";
            }
        }
        public double K5
        {
            get => _simModel.K5;
            set
            {
                _simModel.K5 = value;
                OnPropertyChanged(() => K5);
                Status = $"Изм. положение V5 - {K5}";
            }
        }


        public double PB1
        {
            get => _simModel.PB1;
            set
            {
                _simModel.PB1 = value;
                OnPropertyChanged(() => PB1);
                Status = $"Изм. Значения PB1 - {PB1:F1}";
            }
        }

        public double PB2
        {
            get => _simModel.PB2;
            set
            {
                _simModel.PB2 = value;
                OnPropertyChanged(() => PB2);
                Status = $"Изм. Значения PB2 - {PB2:F1}";
            }
        }

        public DateTime SimTime 
        { 
            get => _simTime;
            set
            {
                _simTime = value;
                OnPropertyChanged(() => SimTime);
            }
        }

        public double P1
        {
            get => _simModel.P1;
            set
            {
                _simModel.P1 = value;
                OnPropertyChanged(() => P1);
                Status = $"Изм. значения P1 - {P1:F2}";
            }
        }

        public double P2
        {
            get => _simModel.P2;
            set
            {
                _simModel.P2 = value;
                OnPropertyChanged(() => P2);
                Status = $"Изм. значения P2 - {P2:F2}";
            }
        }

        public double P3
        {
            get => _simModel.P3;
            set
            {
                _simModel.P3 = value;
                OnPropertyChanged(() => P3);
                Status = $"Изм. значения P3 - {P3:F2}";
            }
        }

        public double P4
        {
            get => _simModel.P4;
            set
            {
                _simModel.P4 = value;
                OnPropertyChanged(() => P4);
                Status = $"Изм. значения P4 - {P4:F2}";
            }
        }

        private RelayCommand _switchPageCommand;
        public RelayCommand SwitchPageCommand => _switchPageCommand ?? (_switchPageCommand = new RelayCommand((a) => {
            //по параметру команды переключам страницы
            switch (a.ToString())
            {
                case "home":
                    {
                        if (CurrentPage.GetType().ToString() != "MainPage")
                        {
                            CurrentPage = new MainPage()
                            {
                                DataContext = this
                            };
                            Status = "Переход к мнемосхеме";
                        }
                        break;
                    }
                case "graph":
                    {
                        if (CurrentPage.GetType().ToString() != "GraphPage")
                        {
                            CurrentPage = new GraphPage()
                            {
                                DataContext = this
                            };
                            Status = "Переход к графикам";
                        }
                        break;
                    }
            }
        }));

        private RelayCommand _startCommand;
        public RelayCommand StartCommand => _startCommand ?? (_startCommand = new RelayCommand((a) => {
            _iterationPoolingTimer.Start();
            Status = "Эмуляция запущена";
        }));

        private RelayCommand _pauseCommand;
        public RelayCommand PauseCommand => _pauseCommand ?? (_pauseCommand = new RelayCommand((a) => {
            _iterationPoolingTimer.Stop();
            Status = "Эмуляция приостановлена";
        }));

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(() => Status);
            }
        }
    }
}
