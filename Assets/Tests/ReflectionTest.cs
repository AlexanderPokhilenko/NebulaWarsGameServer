using System;
using DesperateDevs.Utils;
using NUnit.Framework;

public class ContextCopyFactory
{
    public GameContext Create(GameContext original)
    {
        GameContext copy = new GameContext();
        foreach (var gameEntity in original.GetEntities())
        {
            GameEntity gameEntityCopy = new GameEntity();
            gameEntity.CopyPublicMemberValues(gameEntityCopy);
        }

        return copy;
    }
}
namespace Tests
{
   
    public class ReflectionTest
    {
        [Test]
        public void ReflectionTestSimplePasses()
        {
            GameContext gameContext = new GameContext();
            GameEntity entity = gameContext.CreateEntity();
            entity.AddDamage(84);

            var factory = new ContextCopyFactory();
            var copy = factory.Create(gameContext);
          
            int entitiesCount = gameContext.count;
            int entitiesCountCopy = copy.count;

            if (entitiesCount != entitiesCountCopy)
            {
                Assert.Fail($"{entitiesCount} {entitiesCountCopy}");
            }

            var originalEntities = gameContext.GetEntities();
            var copyEntities = gameContext.GetEntities();

            for (int i = 0; i < originalEntities.Length; i++)
            {
                var originalEntity = originalEntities[i];
                var copyEntity = copyEntities[i];

                if (Math.Abs(copyEntity.damage.value - originalEntity.damage.value) > 0.001)
                {
                    Assert.Fail();
                }
            }
        }
    }
}
