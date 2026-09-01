namespace func_altinn_p360_sync_prod.Models
{
  public class P360File
  {
      public FileParameter? parameter { get; set; }
  }

  public class FileParameter
  {
    public string? Title { get; set; }
	public string? Format { get; set; }
	public string? Base64Data { get; set; }
	public string? DocumentNumber { get; set; }
  }
}