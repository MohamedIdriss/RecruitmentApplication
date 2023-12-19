using System;
using System.Collections.Generic;

namespace RecruitmentApplication.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }

    public bool ProfileCompleted { get; set; }
}
