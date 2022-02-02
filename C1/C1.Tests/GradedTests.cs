using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestCommon;

namespace C1.Tests
{
    [DeploymentItem("TestData", "PS1_TestData")]
    [TestClass()]
    public class GradedTests
    {
        
        [TestMethod(), Timeout(1000)]
        public void SolveTest_Q1GumballMachine()
        {
            RunTest(new Q1GumballMachine("TD1"));
        }

        public static void RunTest(Processor p)
        {
            TestTools.RunLocalTest("PS1", p.Process, p.TestDataName, p.Verifier,
                VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }
    }
}
