namespace func_altinn_p360_sync_prod.Models;

public class SkarvPerson
{
    public string? Tlf { get; set; }
    public string? Tittel { get; set; }
    public string? Id { get; set; }
    public List<HjemmelListe>? HjemmelListe { get; set; }
    public string? Fornavn { get; set; }
    public string? FirmaNavn { get; set; }
    public string? FirmaId { get; set; }
    public string? Etternavn { get; set; }
    public string? BGOpphorsdato { get; set; }
    public string? BegrensetForbud { get; set; }
    //public string? Skvid { get; set; } - To be added
    //public string? Id360 { get; set; } - To be added
}

public class HjemmelListe
{
    public string? Status { get; set; }
    public string? Startdato { get; set; }
    public string? Sluttdato { get; set; }
    public string? Opphorsgrunn { get; set; }
    public string? Hjemmel { get; set; }
}