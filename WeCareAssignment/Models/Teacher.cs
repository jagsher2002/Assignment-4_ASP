namespace WeCareAssignment.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public List<Child>? Children { get; set; }
    }
}
