// File: MqttDashboard/MqttDashboard/MainWindow.xaml.cs
// Created:  2018-06-09 4:03 PM
// Modified: 2018-06-09 8:21 PM

using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using MahApps.Metro.Controls;
using MQTTnet;
using MQTTnet.Server;

namespace MqttDashboard
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IMqttServer _mqttServer;

        public MainWindow()
        {
            InitializeComponent();
            _mqttServer = new MqttFactory().CreateMqttServer();
            _mqttServer.ApplicationMessageReceived += _mqttServer_ApplicationMessageReceived;
            Setupdata();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void _mqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LastMessage.Text = e.ClientId + "\r\n" + Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            });
        }

        private async void ToggleSwitch_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleSwitch se)) return;
            if (se.IsChecked != null && se.IsChecked.Value) await _mqttServer.StartAsync(new MqttServerOptions());
            else await _mqttServer.StopAsync();
        }

        public void Setupdata()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> {4, 6, 5, 2, 4}
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {6, 7, 3, 4, 6},
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> {4, 2, 7, 2, 7},
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = new[] {"Jan", "Feb", "Mar", "Apr", "May"};
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> {5, 3, 2, 4},
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }
    }
}