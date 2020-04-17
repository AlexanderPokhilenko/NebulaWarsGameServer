using NUnit.Framework;
using Server.GameEngine;

namespace Tests
{
    public class NewTestScript
    {
        [Test]
        public void NewTestScriptSimplePasses()
        {
            //Arrange
            MatchStorage matchStorage = new MatchStorage();
            //Act
            bool success = matchStorage.HasMatch(15651);
            //Assert
            Assert.IsFalse(success);
        }
    }
}
