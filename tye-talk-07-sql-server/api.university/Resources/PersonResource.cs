namespace api.university.Resources
{
    public abstract class PersonResource
    {
        public int ID { get; set; }

        public string LastName { get; set; }
        public string FirstMidName { get; set; }

        public string FullName { get; set; }
    }
}