using System;
using System.Collections.Generic;

namespace RecruitmentApplication.Models;

public partial class Candidat
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? University { get; set; }

    public DateTime? Date { get; set; }

    public string? Framework { get; set; }

    public string? Langue { get; set; }

    public string? GithubUrl { get; set; }

    public string? Tel { get; set; }

    public string? StageExperience { get; set; }

    public virtual ICollection<OffreCandidate> OffreCandidates { get; set; } = new List<OffreCandidate>();
}
