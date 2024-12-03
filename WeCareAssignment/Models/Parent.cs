namespace WeCareAssignment.Models
{
    public class Parent
    {
        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public List<Child>? Children { get; set; }
    }
}
