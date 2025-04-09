
using Domain.Models;

namespace Business.Models;

public class StatusResult : ServiceResult
{
    public IEnumerable<Status>? Result { get; set; }
}
