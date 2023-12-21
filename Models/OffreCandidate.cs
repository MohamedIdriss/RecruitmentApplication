using System;
using System.Collections.Generic;

namespace RecruitmentApplication.Models;

public partial class OffreCandidate
{
    public int CandidateId { get; set; }

    public int OffreId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Candidat Candidate { get; set; } = null!;

    public virtual Offre Offre { get; set; } = null!;
}
