using FluentValidation;

namespace TaskMaster.Domain.ViewModels.v1.Users.Validations
{
    public class UserRequestValidation : AbstractValidator<UserRequest>
    {
        public UserRequestValidation()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .Length(2, 50);

            RuleFor(a => a.Email)
                .EmailAddress()
                .NotNull()
                .MaximumLength(255);

            RuleFor(a => a.Password)
                .NotNull();
        }
    }
}
