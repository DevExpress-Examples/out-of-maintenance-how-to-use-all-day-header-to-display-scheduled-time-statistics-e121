using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            DataFiller.FillResources(schedulerControl1.Storage, 2);
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
                AppointmentBaseCollection apts = ((SchedulerControl)sender).Storage.GetAppointments(interval);
                float percent = CalcCurrentWorkTimeLoad(apts, interval, resource);
                Brush brush;
                if (percent == 0.0)
                    brush = Brushes.LightYellow;
                else if (percent < 0.5)
                    brush = Brushes.LightBlue;
                else brush = Brushes.LightCoral;
                e.Cache.FillRectangle(brush, e.Bounds);
                e.Cache.DrawString(string.Format("{0:P}", percent), cell.Appearance.Font, Brushes.Black, e.Bounds, cell.Appearance.TextOptions.GetStringFormat());
                e.Handled = true;
            }
        }

        private float CalcCurrentWorkTimeLoad(AppointmentBaseCollection apts, TimeInterval interval, Resource resource)
        {
            AppointmentBaseCollection aptsByResource = new AppointmentBaseCollection();
            var aptQuery = apts.Where(a => a.ResourceId.Equals(resource.Id));
            aptsByResource.AddRange(aptQuery.ToList());

            IntervalLoadRatioCalculator calc = new IntervalLoadRatioCalculator(interval, aptsByResource);
            return calc.Calculate();
        }
    }


}