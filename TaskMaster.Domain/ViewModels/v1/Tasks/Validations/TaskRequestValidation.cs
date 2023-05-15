using FluentValidation;
using System;

namespace TaskMaster.Domain.ViewModels.v1.Tasks.Validations
{
    public class TaskRequestValidation : AbstractValidator<TaskRequest>
    {
        public TaskRequestValidation()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .Length(2, 50);

            RuleFor(a => a.Description)
                .Length(2, 255);

            RuleFor(a => a.Deadline)
                .NotNull()
                .Custom((value, context) =>
                {
                    if (value == DateTime.MinValue)
                    {
                        context.AddFailure("Deadline", "Data e hora inválidas.");
                    }
                });
        }
    }
}
