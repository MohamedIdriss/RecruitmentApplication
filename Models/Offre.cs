using System;
using System.Collections.Generic;

namespace RecruitmentApplication.Models;

public partial class Offre
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool Publier { get; set; }

    public bool Archiver { get; set; }

    public int RecruteurId { get; set; }

    public virtual Recruteur Recruteur { get; set; } = null!;
}
