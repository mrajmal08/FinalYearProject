using System;
using System.Collections.Generic;

namespace FinalYearProject.Models
{
    public partial class UserSystem
    {
        public int Id { get; set; }
        public string UserFname { get; set; }
        public string UserSname { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserImage { get; set; }
        public string UserAddress { get; set; }
        public DateTime? UserDob { get; set; }
        public string UserGender { get; set; }
        public string UserCountry { get; set; }
        public string UserType { get; set; }
        public string UserStatus { get; set; }
        public string UserContact { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FaceBookID { get; set; }

    }
}
