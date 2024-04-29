
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentApplication.Models;

public partial class Recruteur
{
    public int Id { get; set; }
    [Required]

    public string? Name { get; set; }
    [Required]
    [DisplayName("Last Name")]

    public string? LastName { get; set; }
    [DisplayName("Company Name")]

    public string? CompanyName { get; set; }
    [DisplayName("Company Url")]
    [DataType(DataType.Url)]

    public string? Url { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Offre> Offres { get; set; } = new List<Offre>();
}
