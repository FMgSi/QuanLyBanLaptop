using System;

namespace QuanLyBanLaptop.Models
{
    public abstract class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public sealed class Customer : Person
    {
        public string Membership { get; set; }
    }

    public sealed class Employee : Person
    {
        public string Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
