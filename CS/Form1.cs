using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler.Drawing;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraScheduler.Tools;

namespace DayHeaderStatistics
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataFiller.FillResources(schedulerControl1.Storage, 3);
            DataFiller.GenerateAppointments(schedulerControl1.Storage);
            schedulerControl1.GroupType = SchedulerGroupType.Resource;
            schedulerControl1.Start = DateTime.Now;

           


        }

        private void schedulerControl1_CustomDrawDayViewAllDayArea(object sender, DevExpress.XtraScheduler.CustomDrawObjectEventArgs e)
        {
            if (schedulerStorage1.Appointments != null)
            {
                AllDayAreaCell cell = (AllDayAreaCell)e.ObjectInfo;
                Resource resource = cell.Resource;
                TimeInterval interval = cell.Interval;
                
                float percent = CalcCurrentWorkTimeLoad(interval, resource);
                Brush brush;
                if (percent == 0.0)
                    brush = Brushes.Yellow;
                else if (percent < 0.5)
                    brush = Brushes.LightGreen;
                else brush = Brushes.LightCoral;
                e.Cache.FillRectangle(brush, e.Bounds);
                e.Cache.DrawString(string.Format("{0:P}", percent), cell.Appearance.Font, Brushes.Black, e.Bounds, cell.Appearance.TextOptions.GetStringFormat());
                e.Handled = true;
            }
        }

        private float CalcCurrentWorkTimeLoad(TimeInterval interval, Resource resource)
        {
            AppointmentBaseCollection apts = this.schedulerStorage1.GetAppointments(interval);
            
            ResourceBaseCollection resources = new ResourceBaseCollection();
            resources.Add(resource);
            ResourcesAppointmentsFilter filter = new ResourcesAppointmentsFilter(resources);
            filter.Process(apts);
            IntervalLoadRatioCalculator calc = new IntervalLoadRatioCalculator(interval, (AppointmentBaseCollection)filter.DestinationCollection);
            return calc.Calculate();
        }
    }


}