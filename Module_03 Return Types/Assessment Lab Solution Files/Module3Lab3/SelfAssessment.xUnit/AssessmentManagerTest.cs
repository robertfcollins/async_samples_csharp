using System;
using Xunit;
using System.Threading.Tasks;
using SelfAssessmentLibrary;

namespace SelfAssessment.xUnit
{
    public class AssessmentManagerTest
    {
        private AssessmentManager _manager;

        public AssessmentManagerTest()
        {
            _manager = new AssessmentManager();
        }

        // Write a unit test for a method that successfully returns a Task<T>
        [Fact]
        public async Task WebServiceCanReturnEpoch()
        {
            var wsResult = await _manager.CallWebServiceThatReturnsCustomClass();           
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var returnedDate = epoch.AddMilliseconds(wsResult.milliseconds_since_epoch);
            Assert.True(returnedDate >= DateTime.UtcNow.AddSeconds(-15), "The number of milliseconds are inaccurate");
        }

        // Write a unit test for a Task that passes when an exception is thrown
        [Fact]
        public void UnitTestCanPassOnException()
        {
            var ex = Assert.ThrowsAsync<Exception>(async () => await _manager.DontMineHereAsync());
        }

        // Create a single method that combines parallel and async code
        [Fact]
        public async Task AssessmentManagerCanCombineAsyncAndParallel()
        {
            var bigHours = await _manager.GetWebServiceResultsThenAddInParallel();
            var epochHours = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalHours;
            Assert.True(bigHours > epochHours, "Not enough adding");
        }
    }
}
