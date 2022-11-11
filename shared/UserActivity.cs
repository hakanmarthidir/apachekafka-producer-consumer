namespace shared
{
    public class UserActivity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EmailAddress { get; set; }
        public int ActivityType { get; set; } = 1;
    }
}