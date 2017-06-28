namespace TataAppMac.Models
{
    using System;
	
    public class Time2
    {
		public int TimeId { get; set; }

		public int EmployeeId { get; set; }

		public int ProjectId { get; set; }

		public int ActivityId { get; set; }

		public DateTime DateRegistered { get; set; }

		public DateTime DateReported { get; set; }

		public DateTime From { get; set; }

		public DateTime To { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public string Remarks { get; set; }

        public override int GetHashCode()
        {
            return TimeId;
        }
	}
}