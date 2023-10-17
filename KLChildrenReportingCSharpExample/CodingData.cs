using Hl7.Fhir.Model;

namespace KLChildrenReportingCSharpExample;

public class CodingData
{
    private readonly string system;
    private readonly string code;
    private readonly string? display;

    public CodingData(string system, string code, string? display = null)
    {
        this.system = system;
        this.code = code;
        this.display = display;
    }

    public Coding AsFhirCoding()
    {
        if (display == null)
        {
            return new Coding() { System = system, Code = code };
        }
        else
        {
            return new Coding() { System = system, Code = code, Display = display };
        }
    }
}