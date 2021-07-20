using System;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;


namespace IvtSimulator.Models
{
    public class SimVariable
    {
        private double _vl;
        private LineSeries _trend;
        private Int64 _points = 0;

        public SimVariable(string name, double value)
        {
            Name = name;
            Trend = new LineSeries
            {
                Title = Name,
                Values = new ChartValues<ObservablePoint>(),
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 15
            };
            Value = value;
        }

        public String Name { get; set; }
        public double Value
        {
            get => _vl;
            set {
                _vl = value;
                AddValue(value);
            }
        }
        public LineSeries Trend { get => _trend; set => _trend = value; }

        public void AddValue(double value) 
        { 
            //создание скользящего окна значений на 20 точек
            if (Trend.Values.Count > 20) 
            { 
                Trend.Values.RemoveAt(0); 
            } 
            //добавляем очередную точку
            ObservablePoint p1 = new ObservablePoint(_points, value); 
            Trend.Values.Add(p1); 
            _points++; 
        }

    }
}
