using Hl7.Fhir.Model;
using static Hl7.Fhir.Model.Encounter;

namespace KLChildrenReportingCSharpExample;

public class EncounterData : ResourceData
{
    private readonly EncounterStatus status;
    private readonly CodingData encounterClass;
    private readonly CodingData encounterType;
    private readonly string subjectReference;
    private readonly DateTimeOffset startTime;

    public EncounterData(string id, EncounterStatus status, CodingData encounterClass, CodingData encounterType, string subjectReference, DateTimeOffset startTime)
        : base(id)
    {
        this.status = status;
        this.encounterClass = encounterClass;
        this.encounterType = encounterType;
        this.subjectReference = subjectReference;
        this.startTime = startTime;
    }

    protected override string ResourceType => "Encounter";

    public override Resource AsFhirResource()
    {
        var encounter = new Encounter()
        {
            Id = id,
            Status = status,
            Class = encounterClass.AsFhirCoding(),
            Type = new() { new() { Coding = new() { encounterType.AsFhirCoding() } } },
            Subject = new ResourceReference() { Reference = subjectReference },
            Period = new() { StartElement = new FhirDateTime(startTime) },
            Meta = new() { ProfileElement = new() { new FhirUri("http://fhir.kl.dk/children/StructureDefinition/klgateway-children-encounter") } }
        };

        return encounter;
    }
}
