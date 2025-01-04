namespace UserManagementAPI
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Validate(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                errorMessage = "Name cannot be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.Email) || !this.Email.Contains("@"))
            {
                errorMessage = "Email is invalid.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }


}
