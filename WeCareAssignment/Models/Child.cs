using System.ComponentModel.DataAnnotations;

namespace WeCareAssignment.Models
{
    public class Child
    {
        public int childId { get; set; }

        public string childName { get; set; }

        [Display(Name = "Parents")]
        public int ParentId { get; set; }

        [Display(Name = "Teachers")]
        public int TeacherId { get; set; }

        [Display(Name = "DailyActivities")]
        public int DailyActivityId { get; set; }

        public Teacher? Teachers { get; set; }

        public Parent? Parents { get; set; }

        public DailyActivity? DailyActivities { get; set; }
    }
}
