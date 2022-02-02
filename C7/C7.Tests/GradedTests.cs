using Microsoft.VisualStudio.TestTools.UnitTesting;
using C7;
using TestCommon;

namespace C7.Tests
{
    [DeploymentItem("TestData", "C7_TestData")]
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(1000)]
        public void Q1BuildHashTest()
        {
            string sample = "carpediem";
            var prefixHash = Q1SuffixArrayHashing.BuildHash(sample);
            CollectionAssert.AreEqual(prefixHash, new long[] { 99, 130, 17428, 494084, 5111689, 119628293, 107161350, 670230672, 253617613 });
        }

        [TestMethod(), Timeout(1000)]
        public void Q1SuffixArraySolveTest()
        {
            RunTest(new Q1SuffixArrayHashing("TD1"));
        }

        public static void RunTest(Processor p)
        { 
            TestTools.RunLocalTest("C7", p.Process, p.TestDataName, p.Verifier, VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }
    }
}