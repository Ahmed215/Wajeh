using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Clients.Dto;

namespace Wajeeh.Models.TokenAuth
{
    public class AuthenticateClientResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }
        public bool isNew { get; set; }
        public bool IsDriver { get; set; }
        public bool IsHasePrifle { get; set; }

        public ClientDto Profile { get; set; }
    }
}
