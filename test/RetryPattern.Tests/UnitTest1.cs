using System;
using System.Threading.Tasks;
using Xunit;

namespace RetryPattern.Tests {
    public class UnitTest1 {
        [Fact]
        public async Task Test1() {
            RetryResult sut = await new Retry()
                .CanHandle<ArgumentException>()
                .RunAsync(() => throw new ArgumentException());
            
            Assert.False(sut.IsSuccessful);
        }
    }
}