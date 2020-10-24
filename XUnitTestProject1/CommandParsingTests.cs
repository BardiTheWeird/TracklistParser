using Sprache;
using System;
using Xunit;
using TracklistParser.Parser;
using System.Collections.Generic;
using System.IO;

namespace XUnitTestProject1
{
    public class CommandParsingTests
    {
        [Theory]
        [InlineData("\n\nBroi lerRom", "Broi")]
        [InlineData("\n\nBroilerRom", "BroilerRom")]
        [InlineData("  PropertyName=\"Property Value\"", "PropertyName")]
        public void PropertyNameTest(string input, string name) =>
            Assert.Equal(name, SpracheParser.PropertyName.Parse(input));

        [Theory]
        [InlineData("\"Nainer\"", "Nainer")]
        [InlineData("\"Nai1 ner__ \"", "Nai1 ner__ ")]
        public void PropertyValueTest(string input, string value) =>
            Assert.Equal(value, SpracheParser.PropertyValue.Parse(input));

        [Theory]
        [InlineData("PropertyName=\"Property Value\"", "PropertyName", "Property Value")]
        [InlineData("        PropertyName = \"Property Value\"", "PropertyName", "Property Value")]
        [InlineData("PropertyName=\"Property Value\" MorePropertyName=\"Some Other Value\"  ", "PropertyName", "Property Value")]
        public void CommandPropertyTest(string input, string name, string value)
        {
            var parsed = SpracheParser.CommandProperty.Parse(input);

            Assert.Equal(name, parsed.Name);
            Assert.Equal(value, parsed.Value);
        }

        [Theory]
        [InlineData("<CommandoBravado>", "CommandoBravado", false)]
        [InlineData("<CommandoBravado/>", "CommandoBravado", true)]
        [InlineData("<  CommandoBravado  >", "CommandoBravado", false)]
        [InlineData("<   CommandoBravado  />", "CommandoBravado", true)]
        public void CommandNoPropertiesTest(string input, string name, bool isClosed)
        {
            var parsed = SpracheParser.Command.Parse(input);

            Assert.Equal(name, parsed.Name);
            Assert.Equal(isClosed, parsed.IsClosed);
        }

        [Fact]
        public void CommandWithPropertiesTest1()
        {
            var input = "<SetTagMatch TagName=\"Artist\" TagPattern=\"" + @"(.+)(?= (\s\-\s))" + "\"/>";
            var expectedName = "SetTagMatch";
            var expectedClosed = true;
            var expectedProperties = new List<CommandProperty> 
            { 
                new CommandProperty("TagName", "Artist"),
                new CommandProperty("TagPattern", @"(.+)(?= (\s\-\s))")
            };

            var parsed = SpracheParser.Command.Parse(input);

            Assert.Equal(expectedName, parsed.Name);
            Assert.Equal(expectedClosed, parsed.IsClosed);

            Assert.Equal(expectedProperties.Count, parsed.Properties.Count);
            for (int i = 0; i < expectedProperties.Count; i++)
            {
                var expectedProperty = expectedProperties[i];
                var parsedProperty = parsed.Properties[i];

                Assert.Equal(expectedProperty.Name, parsedProperty.Name);
                Assert.Equal(expectedProperty.Value, parsedProperty.Value);
            }
        }

        [Fact]
        public void CommanListTest1()
        {
            string filepath = @"..\..\..\TestInput\CommandListTestInput1.txt";
            var input = File.ReadAllText(filepath)/*.Replace("\r\n", "\n").Replace("\t", "").Replace('\r', '\n').Replace("\n", "")*/;

            var parsed = SpracheParser.Commands.Parse(input);

            Assert.True(true);
        }
    }
}
