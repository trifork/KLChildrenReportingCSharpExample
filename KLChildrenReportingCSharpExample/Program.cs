using Hl7.Fhir.Serialization;
using KLChildrenReportingCSharpExample;
using static Hl7.Fhir.Model.Bundle;
using static Hl7.Fhir.Model.Encounter;
using static Hl7.Fhir.Model.Patient;

// DateTime values
var timestamp = DateTimeOffset.UtcNow;
var date = new DateTimeOffset(timestamp.Date, new TimeSpan(0));
var encounterTime = date.AddHours(12).AddMinutes(45);

// Fixed values
var managingOrganization = "123456789012345";
var parentCoding = new CodingData("http://terminology.hl7.org/CodeSystem/v3-RoleCode", "PRN");
var homeHealthCoding = new CodingData("http://terminology.hl7.org/CodeSystem/v3-ActCode", "HH");
var TwoMonthsVisitCoding = new CodingData("http://fhir.kl.dk/term/CodeSystem/FBOE", "51f30d1c-d60e-4e3e-ac22-ec9712ea962d", "Besøg ved det ca. 2 måneder gamle barn");
var vitalSignsCoding = new CodingData("http://terminology.hl7.org/CodeSystem/observation-category", "vital-signs");
var headCircumLoincCoding = new CodingData("http://loinc.org", "9843-4");
var headCircumSnomedCoding = new CodingData("http://snomed.info/sct", "363812007", "Head circumference");
var tobaccoSnomedCoding = new CodingData("http://snomed.info/sct", "229819007");
var tobaccoValueSnomedCoding = new CodingData("http://snomed.info/sct", "228524006");
var feedingSnomedCoding = new CodingData("http://snomed.info/sct", "169740003");
var feedingValueSnomedCoding = new CodingData("http://snomed.info/sct", "1145307003");
var indicatorsSKKLCoding = new CodingData("http://fhir.kl.dk/term/CodeSystem/FBOE", "bee30064-8436-4762-83ed-e47d65f23fc6", "Samvær, kontakt, forældre-barn relation");
var indicatorsSKValueKLCoding = new CodingData("http://fhir.kl.dk/term/CodeSystem/FBOE", "96e3eda6-3eb7-4fbb-9850-fc6dfafadb4a", "Problem/bemærkning");
var indicatorsMSKLCoding = new CodingData("http://fhir.kl.dk/term/CodeSystem/FBOE", "2c39af9f-8e45-4c88-962f-e7a9e2cd31b6", "Forælders psykiske tilstand");
var indicatorsMSValueKLCoding = new CodingData("http://fhir.kl.dk/term/CodeSystem/FBOE", "96e3eda6-3eb7-4fbb-9850-fc6dfafadb4a", "Problem/bemærkning");

// Child
var patientRikke = new PatientData("Rikke", "0505209996", managingOrganization);

// Parent
var patientKirsten = new PatientData("Kirsten", "2911829996", managingOrganization);
var rikkesParent = new RelatedPersonData("RikkesParent", patientRikke.Reference(), parentCoding);
patientKirsten.AddLink(rikkesParent.Reference(), LinkType.Seealso);

// Encounter
var twMonthsEncounter = new EncounterData("2mthEncounter", EncounterStatus.Finished, homeHealthCoding, TwoMonthsVisitCoding, patientRikke.Reference(), encounterTime);

// Head Circumference
var headCircumQuantity = new QuantityData((decimal)38.3, "cm", "http://unitsofmeasure.org");
var headCircum = new ObservationData(
    "RikkeHeadCircum",
    "http://fhir.kl.dk/children/StructureDefinition/klgateway-children-headcircum",
    patientRikke.Reference(),
    twMonthsEncounter.Reference(),
    encounterTime,
    null,
    headCircumQuantity,
    new()
    {
        headCircumLoincCoding,
        headCircumSnomedCoding
    },
    vitalSignsCoding);

// Tobacco
var tobacco = new ObservationData(
    "RikkeTobaccoObservation",
    "http://fhir.kl.dk/children/StructureDefinition/klgateway-children-tobacco-observation",
    patientRikke.Reference(),
    twMonthsEncounter.Reference(),
    date,
    null,
    tobaccoValueSnomedCoding,
    new()
    {
        tobaccoSnomedCoding
    },
    null);

// Feeding
var feeding = new ObservationData(
    "RikkeFeedingObservation",
    "http://fhir.kl.dk/children/StructureDefinition/klgateway-children-feeding-observation",
    patientRikke.Reference(),
    twMonthsEncounter.Reference(),
    date,
    date.AddDays(-63),
    feedingValueSnomedCoding,
    new()
    {
        feedingSnomedCoding
    },
    null
    );

// IndicatorsSK
var indicatorsSK = new ObservationData(
    "RikkeIndicatorSK",
    "http://fhir.kl.dk/children/StructureDefinition/klgateway-children-indicator",
    patientRikke.Reference(),
    twMonthsEncounter.Reference(),
    date,
    null,
    indicatorsSKValueKLCoding,
    new()
    {
        indicatorsSKKLCoding
    },
    null
    );

// IndicatorsMS
var indicatorsMS = new ObservationData(
    "KirstenIndicatorMS",
    "http://fhir.kl.dk/children/StructureDefinition/klgateway-children-indicator",
    patientKirsten.Reference(),
    twMonthsEncounter.Reference(),
    date,
    null,
    indicatorsMSValueKLCoding,
    new()
    {
        indicatorsMSKLCoding
    },
    null
    );

// Report
var rikkeReport = new BundleData("RikkeDeliveryReport2months", BundleType.Collection, DateTimeOffset.UtcNow);
rikkeReport.AddEntry(patientRikke);
rikkeReport.AddEntry(patientKirsten);
rikkeReport.AddEntry(rikkesParent);
rikkeReport.AddEntry(twMonthsEncounter);
rikkeReport.AddEntry(headCircum);
rikkeReport.AddEntry(tobacco);
rikkeReport.AddEntry(feeding);
rikkeReport.AddEntry(indicatorsSK);
rikkeReport.AddEntry(indicatorsMS);

// Serialize
var report = rikkeReport.AsFhirResource();
var serializer = new FhirJsonSerializer(new SerializerSettings { Pretty = true });
var jsonContent = serializer.SerializeToString(report);
Console.WriteLine(jsonContent);
