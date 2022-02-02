using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestCommon;
using C4;

namespace C4.Tests
{
    [DeploymentItem("TestData", "E1_TestData")]    
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(10000)]
        public void SolveTest_Q1BetweennessTest()
        {
            RunTest(new Q1Betweenness("TD1"));

        }

        [TestMethod(), Timeout(5000)]
        [DeploymentItem("TestData", "E1_TestData")]
        public void SolveTest_Q2RoadReconstruction()
        {
            
            RunTest(new Q2RoadReconstruction("TD2"));
        }


        public static void RunTest(Processor p)
        {
            TestTools.RunLocalTest("E1", p.Process, p.TestDataName, p.Verifier,
                VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }
    }
}
