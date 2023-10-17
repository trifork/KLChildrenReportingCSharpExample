using Hl7.Fhir.Model;
using static Hl7.Fhir.Model.Identifier;
using static Hl7.Fhir.Model.Patient;

namespace KLChildrenReportingCSharpExample;

public class PatientData : ResourceData
{
    private readonly string personalIdentificationNumber;
    private readonly string managingOrganization;

    private readonly List<PatientLinkData> linkData = new();

    public PatientData(string id, string personalIdentificationNumber, string managingOrganization)
        : base(id)
    {
        this.personalIdentificationNumber = personalIdentificationNumber;
        this.managingOrganization = managingOrganization;
    }

    protected override string ResourceType => "Patient";

    public void AddLink(string otherReference, LinkType linkType)
    {
        linkData.Add(new(otherReference, linkType));
    }

    public override Resource AsFhirResource()
    {
        var patient = new Patient()
        {
            Id = id,
            Identifier = new()
            {
                new()
                {
                    Use = IdentifierUse.Official,
                    System = "urn:oid:1.2.208.176.1.2",
                    Value = personalIdentificationNumber
                }
            },
            ManagingOrganization = new()
            {
                Identifier = new()
                {
                    Use = IdentifierUse.Official,
                    System = "urn:oid:1.2.208.176.1.1",
                    Value = managingOrganization
                }
            },
            Meta = new() { ProfileElement = new() { new FhirUri("http://fhir.kl.dk/children/StructureDefinition/klgateway-children-citizen") } }
        };

        foreach (var link in linkData)
        {
            patient.Link.Add(link.AsLinkComponent());
        }

        return patient;
    }
}
