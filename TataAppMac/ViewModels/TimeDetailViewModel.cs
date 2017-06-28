namespace TataAppMac.ViewModels
{
    using TataAppMac.Models;

    public class TimeDetailViewModel : Time
    {
        Time time;

        public TimeDetailViewModel(Time time)
        {
            this.time = time;

            TimeId = time.TimeId;
            EmployeeId = time.EmployeeId;
            ProjectId = time.EmployeeId;
            ActivityId = time.ActivityId;
            DateRegistered = time.DateRegistered;
            DateReported = time.DateReported;
            From = time.From;
            To = time.To;
            Latitude = time.Latitude;
            Longitude = time.Longitude;
            Remarks = time.Remarks;
    	}
    }
}
