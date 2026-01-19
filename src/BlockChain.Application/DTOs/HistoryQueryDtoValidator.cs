using FluentValidation;

namespace BlockChain.Application.DTOs;

public class HistoryQueryDtoValidator : AbstractValidator<HistoryQueryDto>
{
    public HistoryQueryDtoValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}
