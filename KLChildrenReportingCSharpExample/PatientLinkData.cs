using Hl7.Fhir.Model;
using static Hl7.Fhir.Model.Patient;

namespace KLChildrenReportingCSharpExample;

public class PatientLinkData
{
    private readonly string resourceReference;
    private readonly LinkType type;

    public PatientLinkData(string resourceReference, LinkType type)
    {
        this.resourceReference = resourceReference;
        this.type = type;
    }

    internal LinkComponent AsLinkComponent()
    {
        return new()
        {
            Other = new() { Reference = resourceReference },
            Type = type
        };
    }
}
