using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace Wajeeh.Models.TokenAuth
{
    public class AuthenticateByPhoneModel
    {
        [Required]
        public string Phone { get; set; }
    }
}
