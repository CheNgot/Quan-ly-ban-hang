using System;
using System.Collections.Generic;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using MilestoneOne.charts_screen;
using MilestoneOne.source;

namespace MilestoneOne.charts_screen
{
    /// <summary>
    /// Interaction logic for PieChart.xaml
    /// </summary>
    public partial class PieChart : UserControl
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();
        public PieChart(DateTime df, DateTime dt)
        {
            InitializeComponent();
            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            initChart(df, dt);

            DataContext = this;
        }

        private void initChart(DateTime df, DateTime dt)
        {
            List<MilestoneOne.source.DatabaseHelper.pie> _list = _database.GetDataByProductInOrder(df, dt);
            for (int i = 0; i < _list.Count; i++)
                Pie_chart.Series.Add(new PieSeries() { Values = new ChartValues<double>() { _list[i].total }, Title = $"{_list[i].name}", DataLabels = true, LabelPoint = PointLabel});

        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
