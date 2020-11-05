using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Wajeeh.Models.TokenAuth
{
    public class AuthenticateByPhoneConfirmModel
    {
        [Required]
        public string Phone { get; set; }

        public string Code { get; set; }
    }
}
