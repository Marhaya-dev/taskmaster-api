using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1.Auth.Validations
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
            RuleFor(a => a.Email)
                .NotNull()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(a => a.Password)
                .NotNull()
                .MaximumLength(255);
        }
    }
}
