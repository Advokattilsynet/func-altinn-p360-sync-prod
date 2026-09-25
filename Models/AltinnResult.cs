namespace func_altinn_p360_sync_prod.Models;

public class AltinnResult {
    public string? InstanceId { get; set; }
    public string? PartyId { get; set; }
    public AT_forstegangssoker? FormData { get; set; }
    public List<(string Filename, byte[] Bytes)>? Files { get; set; }
}