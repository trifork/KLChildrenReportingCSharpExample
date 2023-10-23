using Hl7.Fhir.Model;
using static Hl7.Fhir.Model.Bundle;

namespace KLChildrenReportingCSharpExample;

public class BundleData : ResourceData
{
    private readonly BundleType type;
    private readonly DateTimeOffset timestamp;

    private readonly List<ResourceData> entries = new();

    public BundleData(string id, BundleType type, DateTimeOffset timestamp)
        : base(id)
    {
        this.type = type;
        this.timestamp = timestamp;
    }

    protected override string ResourceType => "Bundle";

    public void AddEntry(ResourceData resource)
    {
        entries.Add(resource);
    }

    public override Bundle AsFhirResource()
    {
        var bundle = new Bundle
        {
            Id = id,
            Type = type,
            Timestamp = timestamp,
            Meta = new() { ProfileElement = new() { new FhirUri("http://fhir.kl.dk/children/StructureDefinition/klgateway-children-delivery-report") } }
        };

        foreach (var entry in entries)
        {
            bundle.Entry.Add(
                new EntryComponent()
                {
                    FullUrlElement = new FhirUri(entry.Reference()),
                    Resource = entry.AsFhirResource()
                }); ;
        }

        return bundle;
    }
}
