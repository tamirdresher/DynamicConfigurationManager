using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ConfigurationManager.Tests
{
    [TestFixture]
    public class PathDescriberTests
    {
        [Test]
        public void DotOpertaor_1Level_1LevelInString()
        {
            var pathDescriber = new PathDescriber();
            dynamic dynamicPathDescriber = pathDescriber;

            var _ = dynamicPathDescriber.Level1;

            Assert.AreEqual("Level1",pathDescriber.Path);
        }
        
        [Test]
        public void PathProperty_NoLevels_EmptyPathString()
        {
            var pathDescriber = new PathDescriber();

            Assert.AreEqual(string.Empty,pathDescriber.Path);
        }
        
        [Test]
        public void DotOpertaor_2Level_2LevelInString()
        {
            var pathDescriber = new PathDescriber();
            dynamic dynamicPathDescriber = pathDescriber;

            var _ = dynamicPathDescriber.Level1.Level2;

            Assert.AreEqual("Level1.Level2",pathDescriber.Path);
        }

        [Test]
        public void IndexerOpertaor_2Level_2LevelInString()
        {
            var pathDescriber = new PathDescriber();
            dynamic dynamicPathDescriber = pathDescriber;

            var _ = dynamicPathDescriber["Level1"]["Level2"];

            Assert.AreEqual("Level1.Level2", pathDescriber.Path);
        }

        [Test]
        public void IndexerAndDotOpertaor_FirstIndexerSecondDot_2LevelInString()
        {
            var pathDescriber = new PathDescriber();
            dynamic dynamicPathDescriber = pathDescriber;

            var _ = dynamicPathDescriber["Level1"].Level2;

            Assert.AreEqual("Level1.Level2", pathDescriber.Path);
        } 
        [Test]
        public void IndexerAndDotOpertaor_FirstDotSecondIndexer_2LevelInString()
        {
            var pathDescriber = new PathDescriber();
            dynamic dynamicPathDescriber = pathDescriber;

            var _ = dynamicPathDescriber.Level1["Level2"];

            Assert.AreEqual("Level1.Level2", pathDescriber.Path);
        }
    }
}
