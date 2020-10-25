using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestProject1
{
    public class CommandTypeTests
    {
        [Theory]
        [InlineData("AddTrack", typeof(TracklistParser.Commands.AddTrack))]
        public void TypeTest1(string input, Type expectedType)
        {
            string typePath = "TracklistParser.Commands." + input + ", TracklistParser";
            var type = Type.GetType(typePath);
            string typeStringed = expectedType.ToString();

            // Assert.Equal(expectedType.ToString(), typePath);
            Assert.Equal(expectedType, type);
        }
    }
}
