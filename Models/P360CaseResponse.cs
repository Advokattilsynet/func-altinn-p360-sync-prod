namespace func_altinn_p360_sync_prod.Models
{
  public class P360CaseResponse
  {
    public int? Recno { get; set; }
    public string? CaseNumber { get; set; }
    public string? ImportedCaseNumber { get; set; }
    public string? UID { get; set; }
    public string? UIDOrigin { get; set; }
    public string? URL { get; set; }
    public bool? Successful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorDetails { get; set; }
  }
}