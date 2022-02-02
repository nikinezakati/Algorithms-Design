using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestCommon;

namespace C6.Tests
{
    [DeploymentItem("TestData", "C6_TestData")]
    [TestClass()]
    public class GradedTests
    {

        [TestMethod(), Timeout(7000)]
        public void Q1BoyerMoore()
        {
            RunTest(new Q1BoyerMoore("TD1"));
        }

        public static void RunTest(Processor p)
        {
            TestTools.RunLocalTest("C6", p.Process, p.TestDataName, p.Verifier,
                VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }
    }
}
