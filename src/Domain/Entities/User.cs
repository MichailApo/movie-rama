namespace MovieRamaWeb.Domain
{
    public class User
    {
        public int Id { get; }
        public string UserName { get; }

        public User(int id, string userName)
        {
            Id = id;
            UserName = userName;
        }

    }
}
