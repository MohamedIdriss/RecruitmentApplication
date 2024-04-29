using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecruitmentApplication.Models;

public partial class Offre
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [DisplayName("Publish")]
    public bool Publier { get; set; }
    [DisplayName("Archive")]

    public bool Archiver { get; set; }

    public int RecruteurId { get; set; }

    public virtual ICollection<OffreCandidate> OffreCandidates { get; set; } = new List<OffreCandidate>();

    public virtual Recruteur Recruteur { get; set; } = null!;
}
