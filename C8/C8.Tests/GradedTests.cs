using Microsoft.VisualStudio.TestTools.UnitTesting;
using C8;
using TestCommon;

namespace C8.Tests
{
    [DeploymentItem("TestData", "C8_TestData")]
    [TestClass()]
    public class GradedTests
    {

        [TestMethod(), Timeout(1000)]
        public void Q1SuperPalindromesTest()
        {
            RunTest(new Q1SuperPalindromes("TD1"));
        }

        public static void RunTest(Processor p)
        { 
            TestTools.RunLocalTest("C8", p.Process, p.TestDataName, p.Verifier, VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }
    }
}