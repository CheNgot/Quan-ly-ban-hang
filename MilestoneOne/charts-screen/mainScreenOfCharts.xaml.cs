using MilestoneOne.source;
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
using System.Windows.Shapes;

namespace MilestoneOne.charts_screen
{
    /// <summary>
    /// Interaction logic for mainScreenOfCharts.xaml
    /// </summary>
    public partial class mainScreenOfCharts : Window
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();
        public mainScreenOfCharts()
        {
            InitializeComponent();
            init();
        }

        private int ChartSelected = 0;

        private void enableToSetDateTimeToSeeChart()
        {
            button_ExportCharts.IsEnabled = true;
            datetime_from.IsEnabled = true;
            datetime_to.IsEnabled = true;
        }
        private void blockDateTimeAndButtonExportChart()
        {
            button_ExportCharts.IsEnabled = false;
            datetime_from.IsEnabled = false;
            datetime_to.IsEnabled = false;
        }

        private void init()
        {
            //ColumnsCharts_screen.Visibility = Visibility.Collapsed;
            //PieChart_screen.Visibility = Visibility.Collapsed;
            //LineChart_screen.Visibility = Visibility.Collapsed;
            blockDateTimeAndButtonExportChart();
        }

        private void Button_ColumnsCharts_Click(object sender, RoutedEventArgs e)
        {
            enableToSetDateTimeToSeeChart();
            ToDislayChart.Children.Clear();
            ChartSelected = 1;
        }

        private void Button_LinesCharts_Click(object sender, RoutedEventArgs e)
        {
            enableToSetDateTimeToSeeChart();
            ToDislayChart.Children.Clear();
            ChartSelected = 2;
        }

        private void Button_PieCharts_Click(object sender, RoutedEventArgs e)
        {
            enableToSetDateTimeToSeeChart();
            ToDislayChart.Children.Clear();
            ChartSelected = 3;
        }

        private void Button_ExportCharts_Click(object sender, RoutedEventArgs e)
        {
            
            if (datetime_from.SelectedDate == null || datetime_to.SelectedDate == null)
                MessageBox.Show("Bạn chưa nhập ngày tháng!");
            else
            {
                DateTime df = datetime_from.SelectedDate.Value;
                DateTime dt = datetime_to.SelectedDate.Value;
                var listdatabyday = _database.GetDataByDay(df,dt);
                var listdatabyproinorder = _database.GetDataByProductInOrder(df, dt);
                if(listdatabyday.Count==0 &&listdatabyproinorder.Count()==0)
                {
                    MessageBox.Show("Emty Data !! Can't show!!!");
                }
                else
                {
                    blockDateTimeAndButtonExportChart();
                    switch (ChartSelected)
                    {
                        case 1:
                            ColumnsCharts columnsChart = new ColumnsCharts(df, dt);
                            ToDislayChart.Children.Add(columnsChart);
                            break;
                        case 2:
                            LineChart linechart = new LineChart(df, dt);
                            ToDislayChart.Children.Add(linechart);
                            break;
                        case 3:
                            PieChart piechart = new PieChart(df, dt);
                            ToDislayChart.Children.Add(piechart);
                            break;
                    }

                    ChartSelected = 0;
                }
                
            }
        }
    }
}
