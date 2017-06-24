namespace TataAppMac.Models
{
    public class Project
    {
		public int ProjectId { get; set; }

		public string Description { get; set; }

        public override int GetHashCode()
        {
            return ProjectId;
        }
    }
}
