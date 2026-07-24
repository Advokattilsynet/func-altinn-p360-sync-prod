#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace func_altinn_p360_sync_prod.Models
// cs-model copied from the actual Altinn app (/tra/soknad-om-advokatbevilling)
{
  [XmlRoot(ElementName="AT_forstegangssoker")]
  public class AT_forstegangssoker
  {
      [XmlElement("Personalia", Order = 1)]
      [JsonProperty("Personalia")]
      [JsonPropertyName("Personalia")]
      public Personalia personalia { get; set; }

      [XmlElement("Utdanning", Order = 2)]
      [JsonProperty("Utdanning")]
      [JsonPropertyName("Utdanning")]
      public Utdanning utdanning { get; set; }

      [XmlElement("Praksis", Order = 3)]
      [JsonProperty("Praksis")]
      [JsonPropertyName("Praksis")]
      public Praksis praksis { get; set; }

      [XmlElement("Prosedyredokumentasjon", Order = 4)]
      [JsonProperty("Prosedyredokumentasjon")]
      [JsonPropertyName("Prosedyredokumentasjon")]
      public Prosedyredokumentasjon prosedyredokumentasjon { get; set; }

      [XmlElement("Virksomhetsopplysninger", Order = 5)]
      [JsonProperty("Virksomhetsopplysninger")]
      [JsonPropertyName("Virksomhetsopplysninger")]
      public Virksomhetsopplysninger virksomhetsopplysninger { get; set; }

      [XmlElement("Vandel", Order = 6)]
      [JsonProperty("Vandel")]
      [JsonPropertyName("Vandel")]
      public Vandel vandel { get; set; }

      [XmlElement("Bekreftelser", Order = 7)]
      [JsonProperty("Bekreftelser")]
      [JsonPropertyName("Bekreftelser")]
      public Bekreftelser bekreftelser { get; set; }
  }

  public class Personalia
  {
      [XmlElement("name", Order = 1)]
      [JsonProperty("name")]
      [JsonPropertyName("name")]
      public string name { get; set; }

      [Range(Double.MinValue,Double.MaxValue)]
      [XmlElement("ssn", Order = 2)]
      [JsonProperty("ssn")]
      [JsonPropertyName("ssn")]
      [Required]
      public decimal? ssn { get; set; }

      [XmlElement("phonenumber", Order = 3)]
      [JsonProperty("phonenumber")]
      [JsonPropertyName("phonenumber")]
      public string phonenumber { get; set; }

      [XmlElement("personalEmail", Order = 4)]
      [JsonProperty("personalEmail")]
      [JsonPropertyName("personalEmail")]
      public string personalEmail { get; set; }

      [XmlElement("workEmail", Order = 5)]
      [JsonProperty("workEmail")]
      [JsonPropertyName("workEmail")]
      public string workEmail { get; set; }
  }

  public class Utdanning
  {
      [XmlElement("vitnemalNeeded", Order = 1)]
      [JsonProperty("vitnemalNeeded")]
      [JsonPropertyName("vitnemalNeeded")]
      public string vitnemalNeeded { get; set; }

      [XmlElement("vitnemalUrl", Order = 2)]
      [JsonProperty("vitnemalUrl")]
      [JsonPropertyName("vitnemalUrl")]
      public string vitnemalUrl { get; set; }

      [XmlElement("vitnemalPass", Order = 3)]
      [JsonProperty("vitnemalPass")]
      [JsonPropertyName("vitnemalPass")]
      public string vitnemalPass { get; set; }
  }

  public class Praksis
  {
      [XmlElement("praksiskravNeeded", Order = 1)]
      [JsonProperty("praksiskravNeeded")]
      [JsonPropertyName("praksiskravNeeded")]
      public string praksiskravNeeded { get; set; }

      [XmlElement("references", Order = 2)]
      [JsonProperty("references")]
      [JsonPropertyName("references")]
      public List<repeterendePraksisAttester> references { get; set; }

      [XmlElement("confirmReferences", Order = 3)]
      [JsonProperty("confirmReferences")]
      [JsonPropertyName("confirmReferences")]
      public string confirmReferences { get; set; }
  }

  public class repeterendePraksisAttester
  {
      [XmlAttribute("altinnRowId")]
      [JsonPropertyName("altinnRowId")]
      [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
      [Newtonsoft.Json.JsonIgnore]
      public Guid AltinnRowId { get; set; }

      public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

      [XmlElement("positionDuringJob", Order = 1)]
      [JsonProperty("positionDuringJob")]
      [JsonPropertyName("positionDuringJob")]
      public string positionDuringJob { get; set; }

      [XmlElement("referenceFile", Order = 2)]
      [JsonProperty("referenceFile")]
      [JsonPropertyName("referenceFile")]
      public string referenceFile { get; set; }
  }

  public class Prosedyredokumentasjon
  {
      [XmlElement("soknadPaGrunnlagSomAdvokatfullmektig", Order = 1)]
      [JsonProperty("soknadPaGrunnlagSomAdvokatfullmektig")]
      [JsonPropertyName("soknadPaGrunnlagSomAdvokatfullmektig")]
      public string soknadPaGrunnlagSomAdvokatfullmektig { get; set; }

      [XmlElement("sak1", Order = 2)]
      [JsonProperty("sak1")]
      [JsonPropertyName("sak1")]
      public sak1 sak1 { get; set; }

      [XmlElement("procedureDocumentationList", Order = 3)]
      [JsonProperty("procedureDocumentationList")]
      [JsonPropertyName("procedureDocumentationList")]
      public List<procedureDocumentation> procedureDocumentationList { get; set; }
  }

  public class sak1 {
      [XmlElement("typesOfProcedureDocumentationSak1", Order = 1)]
      [JsonProperty("typesOfProcedureDocumentationSak1")]
      [JsonPropertyName("typesOfProcedureDocumentationSak1")]
      public string typesOfProcedureDocumentationSak1 { get; set; }

      [XmlElement("rettsbokSak1", Order = 2)]
      [JsonProperty("rettsbokSak1")]
      [JsonPropertyName("rettsbokSak1")]
      public string rettsbokSak1 { get; set; }

      [XmlElement("domEllerKjennelseSak1", Order = 3)]
      [JsonProperty("domEllerKjennelseSak1")]
      [JsonPropertyName("domEllerKjennelseSak1")]
      public string domEllerKjennelseSak1 { get; set; }

      [XmlElement("edgecaseForlikSak1", Order = 4)]
      [JsonProperty("edgecaseForlikSak1")]
      [JsonPropertyName("edgecaseForlikSak1")]
      public string edgecaseForlikSak1 { get; set; }

      [XmlElement("edgecaseForlikUttalelseFraRettenSak1", Order = 5)]
      [JsonProperty("edgecaseForlikUttalelseFraRettenSak1")]
      [JsonPropertyName("edgecaseForlikUttalelseFraRettenSak1")]
      public string edgecaseForlikUttalelseFraRettenSak1 { get; set; }

      [XmlElement("edgecaseDeltsakSak1", Order = 6)]
      [JsonProperty("edgecaseDeltsakSak1")]
      [JsonPropertyName("edgecaseDeltsakSak1")]
      public string edgecaseDeltsakSak1 { get; set; }

      [XmlElement("prinsipalRedegjorelseSak1", Order = 7)]
      [JsonProperty("prinsipalRedegjorelseSak1")]
      [JsonPropertyName("prinsipalRedegjorelseSak1")]
      public string prinsipalRedegjorelseSak1 { get; set; }

      [XmlElement("addtionalInfoForCaseSak1", Order = 8)]
      [JsonProperty("addtionalInfoForCaseSak1")]
      [JsonPropertyName("addtionalInfoForCaseSak1")]
      public addtionalInfoForCase addtionalInfoForCaseSak1 { get; set; }
  }

  public class procedureDocumentation
  {
      [XmlAttribute("altinnRowId")]
      [JsonPropertyName("altinnRowId")]
      [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
      [Newtonsoft.Json.JsonIgnore]
      public Guid AltinnRowId { get; set; }

      public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

      [XmlElement("typeOfProcedureDocumentation", Order = 1)]
      [JsonProperty("typeOfProcedureDocumentation")]
      [JsonPropertyName("typeOfProcedureDocumentation")]
      public string typeOfProcedureDocumentation { get; set; }

      [XmlElement("edgecaseForlik", Order = 2)]
      [JsonProperty("edgecaseForlik")]
      [JsonPropertyName("edgecaseForlik")]
      public string edgecaseForlik { get; set; }

      [XmlElement("edgecaseForlikUttalelseFraRetten", Order = 3)]
      [JsonProperty("edgecaseForlikUttalelseFraRetten")]
      [JsonPropertyName("edgecaseForlikUttalelseFraRetten")]
      public string edgecaseForlikUttalelseFraRetten { get; set; }

      [XmlElement("edgecaseDeltsak", Order = 4)]
      [JsonProperty("edgecaseDeltsak")]
      [JsonPropertyName("edgecaseDeltsak")]
      public string edgecaseDeltsak { get; set; }

      [XmlElement("prinsipalRedegjorelse", Order = 5)]
      [JsonProperty("prinsipalRedegjorelse")]
      [JsonPropertyName("prinsipalRedegjorelse")]
      public string prinsipalRedegjorelse { get; set; }

      [XmlElement("trialExam", Order = 6)]
      [JsonProperty("trialExam")]
      [JsonPropertyName("trialExam")]
      public string trialExam { get; set; }

      [XmlElement("procedureDocumentation1", Order = 7)]
      [JsonProperty("procedureDocumentation1")]
      [JsonPropertyName("procedureDocumentation1")]
      public string procedureDocumentation1 { get; set; }

      [XmlElement("procedureDocumentation2", Order = 8)]
      [JsonProperty("procedureDocumentation2")]
      [JsonPropertyName("procedureDocumentation2")]
      public string procedureDocumentation2 { get; set; }

      [XmlElement("addtionalInfoForCase", Order = 9)]
      [JsonProperty("addtionalInfoForCase")]
      [JsonPropertyName("addtionalInfoForCase")]
      public addtionalInfoForCase addtionalInfoForCase { get; set; }
  }
  public class addtionalInfoForCase
  {
    [XmlElement("comments", Order = 1)]
    [JsonProperty("comments")]
    [JsonPropertyName("comments")]
    public string comments { get; set; }

    [XmlElement("attachments", Order = 2)]
    [JsonProperty("attachments")]
    [JsonPropertyName("attachments")]
    public List<repeterendeVedlegg> attachments { get; set; }
  }

  public class Virksomhetsopplysninger
  {
      [XmlElement("typeOfLawyer", Order = 1)]
      [JsonProperty("typeOfLawyer")]
      [JsonPropertyName("typeOfLawyer")]
      public string typeOfLawyer { get; set; }

      [XmlElement("orgnumber", Order = 2)]
      [JsonProperty("orgnumber")]
      [JsonPropertyName("orgnumber")]
      public string orgnumber { get; set; }

      [XmlElement("orgname", Order = 3)]
      [JsonProperty("orgname")]
      [JsonPropertyName("orgname")]
      public string orgname { get; set; }

      [XmlElement("orgAddress", Order = 4)]
      [JsonProperty("orgAddress")]
      [JsonPropertyName("orgAddress")]
      public AddressType orgAddress { get; set; }

      [XmlElement("startupDate", Order = 5)]
      [JsonProperty("startupDate")]
      [JsonPropertyName("startupDate")]
      public string startupDate { get; set; }

      [XmlElement("securityObligations", Order = 6)]
      [JsonProperty("securityObligations")]
      [JsonPropertyName("securityObligations")]
      public string securityObligations { get; set; }

      [XmlElement("accountingObligations", Order = 7)]
      [JsonProperty("accountingObligations")]
      [JsonPropertyName("accountingObligations")]
      public string accountingObligations { get; set; }

      [XmlElement("reportDuty", Order = 8)]
      [JsonProperty("reportDuty")]
      [JsonPropertyName("reportDuty")]
      public string reportDuty { get; set; }

      [XmlElement("selectedSubunitOrgnr", Order = 9)]
      [JsonProperty("selectedSubunitOrgnr")]
      [JsonPropertyName("selectedSubunitOrgnr")]
      public string selectedSubunitOrgnr { get; set; }

      [XmlElement("overordnetEnhetOrgnr", Order = 10)]
      [JsonProperty("overordnetEnhetOrgnr")]
      [JsonPropertyName("overordnetEnhetOrgnr")]
      public string overordnetEnhetOrgnr { get; set; }

      [XmlElement("activeSubunitCount", Order = 11)]
      [JsonProperty("activeSubunitCount")]
      [JsonPropertyName("activeSubunitCount")]
      public int activeSubunitCount { get; set; }
  }
  public class AddressType
  {
    [XmlElement("addressDescription", Order = 1)]
    [JsonProperty("addressDescription")]
    [JsonPropertyName("addressDescription")]
    public string addressDescription { get; set; }

    [XmlElement("zipcode", Order = 2)]
    [JsonProperty("zipcode")]
    [JsonPropertyName("zipcode")]
    public string zipcode { get; set; }

    [XmlElement("postPlace", Order = 3)]
    [JsonProperty("postPlace")]
    [JsonPropertyName("postPlace")]
    public string postPlace { get; set; }
  }

  public class Vandel {
      [XmlAttribute("altinnRowId")]
      [JsonPropertyName("altinnRowId")]
      [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
      [Newtonsoft.Json.JsonIgnore]
      public Guid AltinnRowId { get; set; }

      public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

      [XmlElement("attest", Order = 1)]
      [JsonProperty("attest")]
      [JsonPropertyName("attest")]
      public string attest { get; set; }
  }

  public class Bekreftelser
  {
      [XmlElement("fulfilledLaw_part1", Order = 1)]
      [JsonProperty("fulfilledLaw_part1")]
      [JsonPropertyName("fulfilledLaw_part1")]
      public string fulfilledLaw_part1 { get; set; }

      [XmlElement("fulfilledLaw_part2", Order = 2)]
      [JsonProperty("fulfilledLaw_part2")]
      [JsonPropertyName("fulfilledLaw_part2")]
      public string fulfilledLaw_part2 { get; set; }
  }

  public class repeterendeVedlegg
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("Vedlegg", Order = 1)]
    [JsonProperty("Vedlegg")]
    [JsonPropertyName("Vedlegg")]
    public string Vedlegg { get; set; }
  }
}
