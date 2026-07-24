namespace func_altinn_p360_sync_prod.Models;

public class InstanceDataResult
{
    public AT_forstegangssoker? Model { get; set; }
    public byte[]? FileBytes { get; set; }
    public string? ContentType { get; set; }
    public string? Filename { get; set; }
    public bool IsFile => ContentType == "application/pdf";
}