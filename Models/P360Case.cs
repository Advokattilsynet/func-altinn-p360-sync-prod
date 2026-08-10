namespace func_altinn_p360_sync_prod.Models
{
  public class P360Case
  {
      public Parameter? parameter { get; set; }
  }

  public class Parameter
  {
    public string? Title { get; set; }
	public string? Status { get; set; }
  }
}
