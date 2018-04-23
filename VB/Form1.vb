Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraScheduler.Drawing
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Native
Imports DevExpress.XtraScheduler.Tools

Namespace DayHeaderStatistics
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            DataFiller.FillResources(schedulerControl1.Storage, 2)
            DataFiller.GenerateAppointments(schedulerControl1.Storage)
            schedulerControl1.GroupType = SchedulerGroupType.Resource
            schedulerControl1.Start = Date.Now
        End Sub

        Private Sub schedulerControl1_CustomDrawDayViewAllDayArea(ByVal sender As Object, ByVal e As DevExpress.XtraScheduler.CustomDrawObjectEventArgs) Handles schedulerControl1.CustomDrawDayViewAllDayArea
            If schedulerStorage1.Appointments IsNot Nothing Then
                Dim cell As AllDayAreaCell = CType(e.ObjectInfo, AllDayAreaCell)
                Dim resource As Resource = cell.Resource
                Dim interval As TimeInterval = cell.Interval

                Dim percent As Single = CalcCurrentWorkTimeLoad(interval, resource)
                Dim brush As Brush
                If percent = 0.0 Then
                    brush = Brushes.LightYellow
                ElseIf percent < 0.5 Then
                    brush = Brushes.LightBlue
                Else
                    brush = Brushes.LightCoral
                End If
                e.Cache.FillRectangle(brush, e.Bounds)
                e.Cache.DrawString(String.Format("{0:P}", percent), cell.Appearance.Font, Brushes.Black, e.Bounds, cell.Appearance.TextOptions.GetStringFormat())
                e.Handled = True
            End If
        End Sub

        Private Function CalcCurrentWorkTimeLoad(ByVal interval As TimeInterval, ByVal resource As Resource) As Single
            Dim apts As AppointmentBaseCollection = Me.schedulerStorage1.GetAppointments(interval)
            Dim aptsByResource As New AppointmentBaseCollection()
			Dim aptQuery = apts.Where(Function(a) a.ResourceId.Equals(resource.Id))
            aptsByResource.AddRange(aptQuery.ToList())

            Dim calc As New IntervalLoadRatioCalculator(interval, aptsByResource)
            Return calc.Calculate()
        End Function
    End Class


End Namespace