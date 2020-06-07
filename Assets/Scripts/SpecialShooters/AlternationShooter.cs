using Server.GameEngine;
using System.Collections.Generic;
using System.Linq;

public class AlternationShooter : SpecialShooter
{
    private readonly float timeDelay;
    private readonly int groupCount;
    private int framesCount = int.MaxValue;
    private bool odd;

    public AlternationShooter(float timeDelay, int groupCount)
    {
        this.timeDelay = timeDelay;
        this.groupCount = groupCount;
    }

    protected override IEnumerable<GameEntity> GetSpecialCannons(List<GameEntity> cannons)
    {
        if (framesCount >= timeDelay / Chronometer.DeltaTime)
        {
            odd = !odd;
            framesCount = 0;
            var i = 0;
            if (cannons.Count % 2 != 0)
            {
                i++;
                yield return cannons.First();
            }

            if (odd) i += groupCount;

            var twinGroupCount = 2 * groupCount;

            for (; i < cannons.Count; i+= twinGroupCount)
            {
                for (var j = i; j < i + groupCount; j++)
                {
                    yield return cannons[j];
                }
            }
        }

        framesCount++;
    }
}