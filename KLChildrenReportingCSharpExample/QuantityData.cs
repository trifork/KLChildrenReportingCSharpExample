using Fhir.Metrics;
using Hl7.Fhir.Model;

namespace KLChildrenReportingCSharpExample;

public class QuantityData
{
    private readonly decimal value;
    private readonly string unit;
    private readonly string system;

    public QuantityData(decimal value, string unit, string system)
    {
        this.value = value;
        this.unit = unit;
        this.system = system;
    }

    public Hl7.Fhir.Model.Quantity ToFhirQuantity()
    {
        return new Hl7.Fhir.Model.Quantity(value, unit, system);
    }
}