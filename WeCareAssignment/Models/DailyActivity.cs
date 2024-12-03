namespace WeCareAssignment.Models
{
    public class DailyActivity
    {
        public int DailyActivityId { get; set; }

        public string DailyActivityName { get; set; }

        public List<Child>? Children { get; set; }
    }
}
