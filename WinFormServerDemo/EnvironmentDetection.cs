using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormServerDemo
{
    public partial class EnvironmentDetection : UserControl
    {
        private Thread addDataRunner;
        public delegate void AddDataDelegate();
        public AddDataDelegate addDataDel;

        private DateTime minValue, maxValue;
        private Random rand = new Random();

        private Series hSeries = new Series("湿度");
        private Series wSeries = new Series("水位");
        private Series tSeries = new Series("温度");

        private void EnvironmentDetection_Load(object sender, EventArgs e)
        {
            ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
            addDataRunner = new Thread(addDataThreadStart);
            addDataDel += new AddDataDelegate(AddData);
            minValue = DateTime.Now;
            maxValue = minValue.AddSeconds(30);

            chart1.ChartAreas[0].AxisX.Minimum = minValue.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = maxValue.ToOADate();
            chart1.Series.Clear();

            
            hSeries.ChartType = SeriesChartType.Spline;
            hSeries.BorderWidth = 1;
            hSeries.Color = Color.FromArgb(65, 140, 240);
            hSeries.ShadowOffset = 1;
            hSeries.XValueType = ChartValueType.Time;
            chart1.Series.Add(hSeries);
            
            wSeries.ChartType = SeriesChartType.Spline;
            wSeries.BorderWidth = 1;
            wSeries.Color = Color.FromArgb(252, 180, 65);
            wSeries.ShadowOffset = 1;
            wSeries.XValueType = ChartValueType.Time;
            chart1.Series.Add(wSeries);
            
            tSeries.ChartType = SeriesChartType.Spline;
            tSeries.BorderWidth = 1;
            tSeries.Color = Color.FromArgb(224, 64, 10);
            tSeries.ShadowOffset = 1;
            tSeries.XValueType = ChartValueType.Time;
            chart1.Series.Add(tSeries);

            addDataRunner.Start();
        }

        public EnvironmentDetection()
        {
            InitializeComponent();
        }

        private void AddDataThreadLoop()
        {
            try
            {
                while (true)
                {
                    hSeries.Tag = rand.Next(30, 70);
                    wSeries.Tag = rand.Next(1, 30);
                    tSeries.Tag = rand.Next(70, 100);

                    chart1.Invoke(addDataDel);
                    Thread.Sleep(1000);
                }
            }
            catch
            {
                // Thread is aborted
            }
        }
        public void AddData()
        {
            DateTime timeStamp = DateTime.Now;

            foreach (Series ptSeries in chart1.Series)
            {
                AddNewPoint(timeStamp, ptSeries);
                
            }
        }

        public void AddNewPoint(DateTime timeStamp, Series ptSeries)
        {
            ptSeries.Points.AddXY(timeStamp.ToOADate(), double.Parse(ptSeries.Tag.ToString()));
            double removeBefore = timeStamp.AddSeconds((double)(20) * (-1)).ToOADate();
            while (ptSeries.Points[0].XValue < removeBefore)
            {
                ptSeries.Points.RemoveAt(0);
            }
            chart1.ChartAreas[0].AxisX.Minimum = ptSeries.Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddSeconds(30).ToOADate();
            chart1.Invalidate();
        }
    }
}
