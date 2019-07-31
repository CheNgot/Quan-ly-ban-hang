using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using MilestoneOne.charts_screen;
using MilestoneOne.source;

namespace MilestoneOne.charts_screen
{
    /// <summary>
    /// Interaction logic for LineChart.xaml
    /// </summary>
    public partial class LineChart : UserControl
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();
        private List<string> createdDayToDislay = new List<string>();

        public LineChart(DateTime df, DateTime dt)
        {
            InitializeComponent();
            #region "Set color for chart"
            //Lets define a custom mapper, to set fill and stroke
            //according to chart values...
            Mapper = Mappers.Xy<ObservableValue>()
                .X((item, index) => index)
                .Y(item => item.Value)
                .Fill(item => item.Value > 200 ? DangerBrush : null)
                .Stroke(item => item.Value > 200 ? DangerBrush : null);

            Formatter = x => x + " ms";

            DangerBrush = new SolidColorBrush(Color.FromRgb(238, 83, 80));
            #endregion
            setDataForChart(df, dt);
        }

        private void setDataForChart(DateTime df, DateTime dt)
        {
            Values = new ChartValues<ObservableValue>();
            List<OrderTable> _listOrderFromDateToDate = _database.GetDataByDay(df, dt);
            foreach (OrderTable order in _listOrderFromDateToDate)
            {
                Values.Add(new ObservableValue(order.Total));
                createdDayToDislay.Add(order.CreatedDate.ToString().Substring(0, 9));
            }
            Axis_Name.Labels = createdDayToDislay;

            DataContext = this;
        }

        public Func<double, string> Formatter { get; set; }
        public ChartValues<ObservableValue> Values { get; set; }
        public Brush DangerBrush { get; set; }
        public CartesianMapper<ObservableValue> Mapper { get; set; }
    }
}
