namespace API.Helpers
{
    public enum UserOrder { CreatedAt, LastActive }

    public class UserParams : PaginationParams
    {
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150;
        public UserOrder OrderBy { get; set; } = UserOrder.LastActive;
    }
}
