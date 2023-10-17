using Hl7.Fhir.Model;

namespace KLChildrenReportingCSharpExample;

public class RelatedPersonData : ResourceData
{
    private readonly string patientReference;
    private readonly CodingData relationship;

    public RelatedPersonData(string id, string patientReference, CodingData relationship)
        : base(id)
    {
        this.patientReference = patientReference;
        this.relationship = relationship;
    }

    protected override string ResourceType => "RelatedPerson";

    public override Resource AsFhirResource()
    {
        var relatedPerson = new RelatedPerson()
        {
            Id = id,
            Patient = new ResourceReference(patientReference),
            Relationship = new() { new() { Coding = new() { relationship.AsFhirCoding() } } },
            Meta = new() { ProfileElement = new() { new FhirUri("http://fhir.kl.dk/children/StructureDefinition/klgateway-children-related-parent") } }
        };

        return relatedPerson;
    }
}
