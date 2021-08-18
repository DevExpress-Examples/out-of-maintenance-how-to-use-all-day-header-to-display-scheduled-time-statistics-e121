<!-- default file list -->
*Files to look at*:

* [DataFiller.cs](./CS/DataFiller.cs) (VB: [DataFiller.vb](./VB/DataFiller.vb))
* [Form1.cs](./CS/Form1.cs) (VB: [Form1.vb](./VB/Form1.vb))
* [Program.cs](./CS/Program.cs) (VB: [Program.vb](./VB/Program.vb))
<!-- default file list end -->
# How to use All-Day header to display scheduled time statistics


<p>Problem :</p>
<p>It would be good to display how much time you have in use (% wise) in your schedule. When an appointment is added/changed/deleted for each resource it will calculate the appointments on the days that have been changed against your total time available and get a % which will be added to the top of the schedule. When you have less than 50% available it will show yellow and when you go over 50% it will show in red.</p>
<p>Solution:</p>
<p>You can utilize the <a href="https://docs.devexpress.com/WindowsForms/DevExpress.XtraScheduler.SchedulerControl.CustomDrawDayViewAllDayArea">SchedulerControl.CustomDrawDayViewAllDayArea event</a> to draw this statistics in the All-Day appointments area. Then, you can collect appointments for a particular day using the <a href="https://docs.devexpress.com/WindowsForms/DevExpress.XtraScheduler.SchedulerDataStorage.GetAppointments.overloads">SchedulerStorageBase.GetAppointments method</a>. To calculate work time loads, use the<strong> DevExpress.XtraScheduler.Tools.IntervalLoadRatioCalculator</strong> instance.<br /> This following picture shows the resulting application.<br /><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-use-all-day-header-to-display-scheduled-time-statistics-e121/15.2.4+/media/0d43b8e0-d3c9-11e4-80bf-00155d62480c.png"></p>

<br/>


