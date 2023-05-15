using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1.Auth
{
    public class RefreshTokenResponse
    {
        public bool Authenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
