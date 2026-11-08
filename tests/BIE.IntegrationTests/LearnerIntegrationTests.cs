using System.Net;
using System.Net.Http.Json;
using BIE.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bie.IntegrationTests;

[TestClass]
public class LearnerIntegrationTests
{
    [TestMethod]
    public async Task TouristAt30DaysIsSavedAsApproved()
    {
        using var app = new BieFactory();
        using var client = app.CreateClient();
        var entry = new Entry("Sam Test", "Orion", "Tourismus", 30, false, false, false, true);
        var response = await client.PostAsJsonAsync("/api/cases", entry);
        var records = await client.GetFromJsonAsync<List<Case>>("/api/cases");
        // TODO: HTTP-Status, gespeicherte Werte und Entscheidung prüfen.
        // HTTP-Status prüfen
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        // Gespeicherte Werte prüfen
        Assert.IsNotNull(records);
        Assert.AreEqual(1, records.Count);
        Assert.AreEqual(entry, records[0].Applicant);
        Assert.IsTrue(records[0].Id > 0);

        // Entscheidung prüfen
        Assert.AreEqual("approved", records[0].Decision.Status);
    }
}
