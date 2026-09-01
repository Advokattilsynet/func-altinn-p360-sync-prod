namespace func_altinn_p360_sync_prod.Models
{
  public class P360Case
  {
      public CaseParameter? parameter { get; set; }
  }

  public class CaseParameter
  {
    public string? Title { get; set; }
	public string? Status { get; set; }
  }
}
