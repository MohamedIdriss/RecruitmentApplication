using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentApplication.Models;

public partial class Candidat
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
    [Required]

    [DisplayName("Last Name")]
    public string? LastName { get; set; }

    public string? University { get; set; }
    [DisplayName("Date of Birth")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
    public DateTime? Date { get; set; }

    public string? Framework { get; set; }

    public string? Langue { get; set; }
    [DisplayName("Github Url")]
    [DataType(DataType.Url)]
    public string? GithubUrl { get; set; }
    [DisplayName("Phone Number")]

    public string? Tel { get; set; }
    [DisplayName("Internships or Experiences")]

    public string? StageExperience { get; set; }

    public virtual ICollection<OffreCandidate> OffreCandidates { get; set; } = new List<OffreCandidate>();
}
