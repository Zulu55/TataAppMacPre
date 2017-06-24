namespace TataAppMac.Models
{
    public class Activity
    {
		public int ActivityId { get; set; }

		public string Description { get; set; }

        public override int GetHashCode()
        {
            return ActivityId;
        }
    }
}
