using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestCommon;

namespace C5.Tests
{
    [DeploymentItem("TestData", "C5_TestData")]
    [TestClass()]
    public class GradedTests
    {

        [TestMethod(), Timeout(7000)]
        public void Q1LazyTypistTest()
        {
            RunTest(new Q1LazyTypist("TD1"));
        }

        public static void RunTest(Processor p)
        {
            TestTools.RunLocalTest("C5", p.Process, p.TestDataName, p.Verifier,
                VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }
    }
}
