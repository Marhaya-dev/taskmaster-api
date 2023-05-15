using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1.TaskStatus.Validations
{
    public class TaskStatusRequestValidation : AbstractValidator<TaskStatusRequest>
    {
        public TaskStatusRequestValidation()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .Length(2, 50);

            RuleFor(a => a.Description)
                .Length(2, 255);
        }
    }
}
