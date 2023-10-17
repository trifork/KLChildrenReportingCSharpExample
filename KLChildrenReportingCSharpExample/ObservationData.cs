using Hl7.Fhir.Model;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;

namespace KLChildrenReportingCSharpExample;

public class ObservationData : ResourceData
{
    private readonly string profile;
    private readonly string subjectReference;
    private readonly string encounterReference;
    private readonly DateTimeOffset effectiveStartDateTime;
    private readonly DateTimeOffset? effectiveEndDateTime;
    private readonly QuantityData? quantity = null;
    private readonly CodingData? codingValue = null;
    private readonly List<CodingData> code;
    private readonly CodingData? category;

    public ObservationData(string id,
                           string profile,
                           string subjectReference,
                           string encounterReference,
                           DateTimeOffset effectiveDateTime,
                           DateTimeOffset? effectiveEndDateTime,
                           QuantityData quantity,
                           List<CodingData> code,
                           CodingData? category)
        : base(id)
    {
        this.profile = profile;
        this.subjectReference = subjectReference;
        this.encounterReference = encounterReference;
        this.effectiveStartDateTime = effectiveDateTime;
        this.effectiveEndDateTime = effectiveEndDateTime;
        this.quantity = quantity;
        this.code = code;
        this.category = category;
    }

    public ObservationData(string id,
                           string profile,
                           string subjectReference,
                           string encounterReference,
                           DateTimeOffset effectiveStartDateTime,
                           DateTimeOffset? effectiveEndDateTime,
                           CodingData codingValue,
                           List<CodingData> code,
                           CodingData? category)
    : base(id)
    {
        this.profile = profile;
        this.subjectReference = subjectReference;
        this.encounterReference = encounterReference;
        this.effectiveStartDateTime = effectiveStartDateTime;
        this.effectiveEndDateTime = effectiveEndDateTime;
        this.codingValue = codingValue;
        this.code = code;
        this.category = category;
    }

    protected override string ResourceType => "Observation";

    public override Resource AsFhirResource()
    {
        var observation = new Observation()
        {
            Id = id,
            Subject = new ResourceReference(subjectReference),
            Encounter = new ResourceReference(encounterReference),
            Status = ObservationStatus.Final,
            Meta = new() {  ProfileElement = new List<FhirUri>() { new FhirUri(profile) } }
        };

        if (category != null)
        {
            observation.Category = new() { new() { Coding = new() { category.AsFhirCoding() } } };
        }
        if (effectiveEndDateTime == null)
        {
            observation.Effective = new FhirDateTime(effectiveStartDateTime);
        }
        else
        {
            observation.Effective = new Period(new FhirDateTime(effectiveStartDateTime), new FhirDateTime((DateTimeOffset)effectiveEndDateTime));
        }
        if (quantity != null)
        {
            observation.Value = quantity.ToFhirQuantity();
        }
        if (codingValue != null)
        {
            var coding = codingValue.AsFhirCoding();
            if (coding.Display == null)
            {
                observation.Value = new CodeableConcept(coding.System, coding.Code);
            }
            else
            {
                observation.Value = new CodeableConcept().Add(coding.System, coding.Code, coding.Display);
            }
        }

        if (code.Count > 0)
        {
            observation.Code = new();
            foreach (var code in code)
            {
                observation.Code.Coding.Add(code.AsFhirCoding());
            }
        }
        return observation;
    }
}
