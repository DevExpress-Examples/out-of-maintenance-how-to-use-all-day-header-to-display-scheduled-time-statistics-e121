using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraScheduler;

namespace DayHeaderStatistics {
    public class DataFiller {
        public static string[] Users = new string[] { "Sarah Brighton", "Ryan Fischer", "Andrew Miller" };
        public static string[] Usernames = new string[] { "sbrighton", "rfischer", "amiller" };
        public static Random RandomInstance = new Random();


        public static void FillResources(SchedulerStorage storage, int count) {
            ResourceCollection resources = storage.Resources.Items;
            storage.BeginUpdate();
            try {
                int cnt = Math.Min(count, Users.Length);
                for (int i = 1; i <= cnt; i++) {
                    Resource resource = storage.CreateResource(Usernames[i - 1], Users[i - 1]);
                    resources.Add(resource);
                }
            }
            finally {
                storage.EndUpdate();
            }
        }


        public static void GenerateAppointments(SchedulerStorage storage) {
            int count = storage.Resources.Count;
            for (int i = 0; i < count; i++) {
                Resource resource = storage.Resources[i];
                string subjPrefix = resource.Caption + "'s ";

                storage.Appointments.Add(AptCreate(storage, resource.Id, subjPrefix + "meeting", 2, 5));
                storage.Appointments.Add(AptCreate(storage, resource.Id, subjPrefix + "travel", 3, 6));
                storage.Appointments.Add(AptCreate(storage, resource.Id, subjPrefix + "phone call", 0, 10));
            }
        }
        public static Appointment AptCreate(SchedulerStorage storage, object resourceId, string subject, int status, int label) {
            Appointment apt = storage.CreateAppointment(AppointmentType.Normal);
            apt.Subject = subject;
            apt.ResourceId = resourceId;
            Random rnd = RandomInstance;
            int rangeInHours = 48;
            apt.Start = DateTime.Today + TimeSpan.FromHours(rnd.Next(0, rangeInHours));
            apt.End = apt.Start + TimeSpan.FromHours(rnd.Next(0, rangeInHours / 8));
            apt.StatusKey = status;
            apt.LabelKey = label;
            return apt;
        }

    }
}
