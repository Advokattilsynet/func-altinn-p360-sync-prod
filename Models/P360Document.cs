namespace func_altinn_p360_sync_prod.Models
{
  public class P360Document
  {
      public DocumentParameter? parameter { get; set; }
  }

  public class DocumentParameter
  {
    public string? Title { get; set; }
    public string? DocumentDate { get; set; }
	public string? Category { get; set; }
	public string? Status { get; set; }
	public string? CaseNumber { get; set; }
	public string? AccessCode { get; set; }
	public string? Paragraph { get; set; }
	public string? AccessGroup { get; set; }
	public string? Files { get; set; }
	public string? ResponsiblePersonRecno { get; set; }
	public List<DocumentContact?> Contacts { get; set; }
  }

  public class DocumentContact
  {
      public string? ReferenceNumber { get; set; }
      public string? ExternalId { get; set; }
      public string? Role { get; set; }
  }
}