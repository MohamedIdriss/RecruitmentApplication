using System;
using System.Collections.Generic;

namespace RecruitmentApplication.Models;

public partial class Recruteur
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string? Url { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Offre> Offres { get; set; } = new List<Offre>();
}
