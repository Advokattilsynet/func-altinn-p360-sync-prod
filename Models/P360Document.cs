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

	public List<DocumentContact?>? Contacts { get; set; }

	public List<UnregisteredDocumentContact?>? UnregisteredContacts { get; set; }
  }

  public class DocumentContact
  {
      public string? ReferenceNumber { get; set; }
      public string? ExternalId { get; set; }
      public string? Role { get; set; }
  }

  public class UnregisteredDocumentContact
  {
      public string? ContactName { get; set; }
      public string? Role { get; set; }
      public string? ContactCompanyName { get; set; }
      public string? ReferenceNumber { get; set; }
      public string? Address { get; set; }
      public string? Country { get; set; }
      public string? State { get; set; }
      public string? ZipCode { get; set; }
      public string? ZipPlace { get; set; }
      public string? Email { get; set; }
      public string? MobilePhone { get; set; }
      public string? Phone { get; set; }
      public string? Fax { get; set; }
      public bool? IsUnofficial { get; set; }
      public string? DispatchChannel { get; set; }
  }
}