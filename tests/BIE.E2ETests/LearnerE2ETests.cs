using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace BIE.E2ETests;

[TestClass]
public class LearnerE2ETests : BrowserTestBase
{
    [TestMethod]
    public void TouristAt30DaysRemainsApprovedAfterReload()
    {
        Fill("Sam Browser", "Tourismus", "30");
        // TODO: Absenden, sichtbares Ergebnis abwarten und prüfen.
        // Absenden
        Driver.FindElement(By.Id("submit")).Click();

        // Auf sichtbares Ergebnis warten
        var result = Wait.Until(d =>
        {
            var element = d.FindElement(By.Id("result"));
            return element.Displayed ? element : null;
        });

        // Entscheidung prüfen
        StringAssert.Contains(result.Text, "GENEHMIGT");
        var caseId = Driver.FindElement(By.Id("result")).GetDomAttribute("data-application-id");
        Driver.Navigate().Refresh();
        var row = Wait.Until(d => d.FindElements(By.CssSelector("#application-list tr")).FirstOrDefault(r => r.GetDomAttribute("data-id") == caseId));
        var cells = row.FindElements(By.TagName("td"));
        // TODO: Name, Herkunft, Tage und Entscheidung nach Neuladen prüfen.
        // Gespeicherte Werte prüfen
        Assert.AreEqual("Sam Browser", cells[1].Text);
        Assert.AreEqual("Orion", cells[2].Text);
        Assert.AreEqual("30 Tage", cells[3].Text);
        Assert.AreEqual("GENEHMIGT", cells[4].Text);
    }
}
