namespace Courier_Management.Models
{
    public class Package
    {
        public int PackageID { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ClientID { get; set; }
        public int? CourierID { get; set; }

    }



    public class Users
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
