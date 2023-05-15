using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1.Auth.Validations
{
    public class RefreshTokenRequestValidation : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidation()
        {
            RuleFor(a => a.Token)
                .NotNull()
                .NotEmpty();

            RuleFor(a => a.RefreshToken)
                .NotNull()
                .NotEmpty();
        }
    }
}
