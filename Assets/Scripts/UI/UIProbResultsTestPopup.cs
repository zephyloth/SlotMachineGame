public class UIProbResultsTestPopup : UIPopup
{
    protected override void AddTextContent()
    {
        ProbabilityManager.DoProbabilityTest(ref ContentBuffer);
    }
}
