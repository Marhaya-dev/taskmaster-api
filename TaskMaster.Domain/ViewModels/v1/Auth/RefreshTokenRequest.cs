using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1.Auth
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
