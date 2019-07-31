using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MilestoneOne.charts_screen;
using MilestoneOne.source;

using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace MilestoneOne.charts_screen
{
    /// <summary>
    /// Interaction logic for ColumnsCharts.xaml
    /// </summary>
    public partial class ColumnsCharts : UserControl
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();
        private List<double> totalMoneyToDislay = new List<double>();
        private List<string> createdDayToDislay = new List<string>();

        public ColumnsCharts(DateTime df, DateTime dt)
        {
            InitializeComponent();
            setDataForChart(df, dt);
        }

        private void setDataForChart(DateTime df, DateTime dt)
        {
            List<OrderTable> _listOrderFromDateToDate = _database.GetDataByDay(df, dt);
            foreach (OrderTable order in _listOrderFromDateToDate)
            {
                totalMoneyToDislay.Add(order.Total);
                createdDayToDislay.Add(order.CreatedDate.ToString().Substring(0, 9));
            }

            ColumnsValue.Values = totalMoneyToDislay.AsChartValues<double>();
            Axis_columnsName.Labels = createdDayToDislay;
        }
        
        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            var vm = (ViewModel)DataContext;
            var chart = (CartesianChart)sender;

            //lets get where the mouse is at our chart
            var mouseCoordinate = e.GetPosition(chart);

            //now that we know where the mouse is, lets use
            //ConverToChartValues extension
            //it takes a point in pixes and scales it to our chart current scale/values
            var p = chart.ConvertToChartValues(mouseCoordinate);

            //in the Y section, lets use the raw value
            vm.YPointer = p.Y;

            //for X in this case we will only highlight the closest point.
            //lets use the already defined ClosestPointTo extension
            //it will return the closest ChartPoint to a value according to an axis.
            //here we get the closest point to p.X according to the X axis
            var series = chart.Series[0];
            var closetsPoint = series.ClosestPointTo(p.X, AxisOrientation.X);

            vm.XPointer = closetsPoint.X;
        }
    }
}
