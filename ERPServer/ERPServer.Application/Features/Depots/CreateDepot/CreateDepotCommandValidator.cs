using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPServer.Application.Features.Depots.CreateDepot;

public sealed class CreateDepotCommandValidator : AbstractValidator<CreateDepotCommand>
{
    public CreateDepotCommandValidator()
    {
        RuleFor(p => p.Name).MinimumLength(3);
        RuleFor(p => p.City).MinimumLength(3);
        RuleFor(p => p.Town).MinimumLength(3);
        RuleFor(p => p.FullAddress).MinimumLength(3);
    }
}
