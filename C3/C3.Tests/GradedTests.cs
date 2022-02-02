using C2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestCommon;

namespace C3.Tests
{
    [DeploymentItem("TestData", "C3_TestData")]
    [TestClass()]
    public class GradedTests
    {

        [TestMethod(), Timeout(5000)]
        public void SolveTest_Q1RoadsInHackerLand()
        {
            RunTest(new Q1RoadsInHackerLand("TD1"));
        }

        public static void RunTest(Processor p)
        {
            TestTools.RunLocalTest("C3", p.Process, p.TestDataName, p.Verifier,
                VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }
    }
}
