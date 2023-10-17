using Hl7.Fhir.Model;

namespace KLChildrenReportingCSharpExample;

public abstract class ResourceData
{
    protected readonly string id;

    public ResourceData(string id)
    {
        this.id = id;
    }

    public string Reference() => $"{ResourceType}/{id}";

    protected abstract string ResourceType { get; }

    public abstract Resource AsFhirResource();

}
