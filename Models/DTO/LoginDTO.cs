namespace BACKEND_STORE.Models.DTO
{
    public class LoginDTO
    {
        public class LoginResponse
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }
        public class RestoreResponse
        {
            public string Email { get; set; }
        }
        
    }
}
