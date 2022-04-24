using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aton.Domain.Models;

namespace Aton.Infrastructure.Identity.Models;

public class AccountToUser
{

    public AccountToUser(Guid accountId, Guid userId)
    {
        AccountId = accountId;
        UserId = userId;
    }

    public AccountToUser()
    {
        
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Guid { get;  set; }
    public Guid AccountId { get; set; }
    public Account Account { get;  set; }
    public Guid UserId { get;  set; }
    public User User { get;  set; }
}