using System.ComponentModel.DataAnnotations;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerSendsAccountData : BaseDto
{
    [MinLength(4)]
    public String email { get; set; }
    
    [MinLength(1)]
    public String city { get; set; }
    
    [MinLength(2)]
    public String realname { get; set; }
}
