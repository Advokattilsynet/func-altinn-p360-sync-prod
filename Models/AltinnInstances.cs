using System.Text.Json.Serialization;

namespace func_altinn_p360_sync_prod.Models;

public class AltinnInstances
{
    [JsonPropertyName("count")]
    public int count { get; set; }

    [JsonPropertyName("self")]
    public string self { get; set; } = string.Empty;

    [JsonPropertyName("next")]
    public string next { get; set; } = string.Empty;

    [JsonPropertyName("instances")]
    public List<AltinnSoknadsskjema> instances { get; set; } = [];
}

public class AltinnSoknadsskjema
{
    [JsonPropertyName("id")]
    public string id { get; set; } = string.Empty;

    [JsonPropertyName("instanceOwner")]
    public InstanceOwner? instanceOwner { get; set; }

    [JsonPropertyName("appId")]
    public string appId { get; set; } = string.Empty;

    [JsonPropertyName("org")]
    public string org { get; set; } = string.Empty;
    
    [JsonPropertyName("selfLinks")]
    public SelfLinks? selfLinks { get; set; }

    [JsonPropertyName("visibleAfter")]
    public string visibleAfter { get; set; } = string.Empty;

    [JsonPropertyName("process")]
    public Process? process { get; set; }

    [JsonPropertyName("status")]
    public Status? status { get; set; }

    [JsonPropertyName("data")]
    public List<Data> data { get; set; } = [];

    [JsonPropertyName("dataValues")]
    public DataValues? dataValues { get; set; }

    [JsonPropertyName("created")]
    public string created { get; set; } = string.Empty;

    [JsonPropertyName("createdBy")]
    public string createdBy { get; set; } = string.Empty;

    [JsonPropertyName("lastChanged")]
    public string lastChanged { get; set; } = string.Empty;

    [JsonPropertyName("lastChangedBy")]
    public string lastChangedBy { get; set; } = string.Empty;
}

public class InstanceOwner
{
    [JsonPropertyName("partyId")]
    public string partyId { get; set; } = string.Empty;
    
    [JsonPropertyName("organisationNumber")]
    public string organisationNumber { get; set; } = string.Empty;
}

public class SelfLinks
{
    [JsonPropertyName("platform")]
    public string platform { get; set; } = string.Empty;
}

public class Process
{
    [JsonPropertyName("started")]
    public string started { get; set; } = string.Empty;

    [JsonPropertyName("startEvent")]
    public string startEvent { get; set; } = string.Empty;

    [JsonPropertyName("currentTask")]
    public CurrentTask? currentTask { get; set; }
}

public class CurrentTask
{
    [JsonPropertyName("flow")]
    public string flow { get; set; } = string.Empty;

    [JsonPropertyName("started")]
    public string started { get; set; } = string.Empty;

    [JsonPropertyName("elementId")]
    public string elementId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string name { get; set; } = string.Empty;

    [JsonPropertyName("altinnTaskType")]
    public string altinnTaskType { get; set; } = string.Empty;

    [JsonPropertyName("flowType")]
    public string flowType { get; set; } = string.Empty;
}

public class Status
{
    [JsonPropertyName("isArchived")]
    public string isArchived { get; set; } = string.Empty;

    [JsonPropertyName("isSoftDeleted")]
    public string isSoftDeleted { get; set; } = string.Empty;

    [JsonPropertyName("isHardDeleted")]
    public string isHardDeleted { get; set; } = string.Empty;

    [JsonPropertyName("readStatus")]
    public string readStatus { get; set; } = string.Empty;
}

public class Data
{
    [JsonPropertyName("id")]
    public string id { get; set; } = string.Empty;

    [JsonPropertyName("instanceGuid")]
    public string instanceGuid { get; set; } = string.Empty;

    [JsonPropertyName("dataType")]
    public string dataType { get; set; } = string.Empty;

    [JsonPropertyName("contentType")]
    public string contentType { get; set; } = string.Empty;

    [JsonPropertyName("blobStoragePath")]
    public string blobStoragePath { get; set; } = string.Empty;

    [JsonPropertyName("selfLinks")]
    public SelfLinks? selfLinks { get; set; }

    [JsonPropertyName("size")]
    public string size { get; set; } = string.Empty;

    [JsonPropertyName("locked")]
    public string locked { get; set; } = string.Empty;

    [JsonPropertyName("isRead")]
    public string isRead { get; set; } = string.Empty;

    [JsonPropertyName("tags")]
    public List<string> tags { get; set; } = [];

    [JsonPropertyName("fileScanResult")]
    public string fileScanResult { get; set; } = string.Empty;

    [JsonPropertyName("created")]
    public string created { get; set; } = string.Empty;

    [JsonPropertyName("createdBy")]
    public string createdBy { get; set; } = string.Empty;

    [JsonPropertyName("lastChanged")]
    public string lastChanged { get; set; } = string.Empty;

    [JsonPropertyName("lastChangedBy")]
    public string lastChangedBy { get; set; } = string.Empty;
}

public class DataValues
{
    [JsonPropertyName("dialog.id")]
    public string dialog_id { get; set; } = string.Empty;
}