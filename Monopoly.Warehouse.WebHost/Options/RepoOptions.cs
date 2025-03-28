using Microsoft.Extensions.Options;

namespace Monopoly.Warehouse.WebHost.Options;

public class RepoOptions : IOptions<RepoOptions>
{
    public RepoOptions Value => this;
    
    public bool UseInMemoryDatabase { get; set; }
}