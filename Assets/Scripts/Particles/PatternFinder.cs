using System.Collections.Generic;
using Collisions;
using Zenject;

namespace Particles
{
    public class PatternFinder : IPatternFinder
    {
        private const int REPEAT_TIMES = 5;
        private readonly IDuplicatePatternFinder _duplicatePatternFinder;

        [Inject]
        public PatternFinder(IDuplicatePatternFinder duplicatePatternFinder)
        {
            _duplicatePatternFinder = duplicatePatternFinder;
        }

        public List<int> FindPattern(List<CollisionRecord> ids)
        {
            int maxPatternLength = ids.Count / REPEAT_TIMES;
            List<int> foundPatternLengths = new List<int>();

            for (var patternLength = 1; patternLength <= maxPatternLength; ++patternLength)
            {
                bool allCirclesEqual = true;

                for (var i = 0; i < patternLength; ++i)
                {
                    // Implemented PatternFinder_Tests counting from the end of the list because the end of the list is the latest
                    int lastIndex = ids.Count - 1 - i;
                    var lastId = ids[lastIndex].DotId;

                    if (lastId != ids[lastIndex - patternLength].DotId ||
                        lastId != ids[lastIndex - patternLength * 2].DotId ||
                        lastId != ids[lastIndex - patternLength * 3].DotId ||
                        lastId != ids[lastIndex - patternLength * 4].DotId)
                    {
                        allCirclesEqual = false;
                        break;
                    }
                }

                if (allCirclesEqual)
                {
                    if (foundPatternLengths.Count == 0)
                    {
                        foundPatternLengths.Add(patternLength);
                    }
                    else
                    {
                        var currentFoundPatterns = new List<int>(foundPatternLengths);
                        foreach (var currentPatternLength in currentFoundPatterns)
                        {
                            if (!_duplicatePatternFinder.IsRepeat(ids, patternLength, currentPatternLength))
                            {
                                foundPatternLengths.Add(patternLength);
                            }
                        }
                    }
                }
            }

            return foundPatternLengths;
        }
    }
}
