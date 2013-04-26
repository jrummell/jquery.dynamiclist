using System;
using System.IO;
using Moq;
using NUnit.Framework;

namespace Minify.Tests
{
    [TestFixture]
    public class BundleServiceFixture
    {
        private static void AssertScriptBundle(BundleType bundleType)
        {
            BundleService service = new BundleService(new FileService());
            string outputFile = "bundled.js";
            service.Bundle(outputFile, new[] {@"assets\one.js", @"assets\two.js"}, bundleType);

            string actual = File.ReadAllText(outputFile);

            Console.WriteLine(actual);

            Assert.That(actual.Contains("function one()"));
            Assert.That(actual.Contains("console.log(\"one\")"));
            Assert.That(actual.Contains("function two()"));
            Assert.That(actual.Contains("console.log(\"two\")"));
        }

        private static void AssertStyleBundle(BundleType bundleType)
        {
            BundleService service = new BundleService(new FileService());
            string outputFile = "bundled.css";

            service.Bundle(outputFile, new[] {@"assets\one.css", @"assets\two.css"}, bundleType);

            string actual = File.ReadAllText(outputFile);

            Console.WriteLine(actual);

            Assert.That(actual.Contains(".one"));
            Assert.That(actual.Contains(".two"));
        }

        [Test]
        public void BundleArguments()
        {
            Mock<IFileService> mockFileService = new Mock<IFileService>();
            mockFileService.Setup(file => file.Exists(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            BundleService service = new BundleService(mockFileService.Object);

            Assert.That(() => service.Bundle(null, new[] {"one.js", "two.js"}, BundleType.Script),
                        Throws.ArgumentException);
            Assert.That(() => service.Bundle("bundle.js", null, BundleType.Script),
                        Throws.ArgumentException);
            Assert.That(() => service.Bundle("bundle.js", new string[0], BundleType.Script),
                        Throws.ArgumentException);
            Assert.That(() => service.Bundle("bundle.js", new[] {"one.js", "two.js"}, BundleType.None),
                        Throws.Nothing);
            Assert.That(() => service.Bundle("bundle.jsx", new[] {"one.js", "two.js"}, BundleType.None),
                        Throws.ArgumentException);
        }

        [Test]
        public void BundleScript()
        {
            AssertScriptBundle(BundleType.Script);
        }

        [Test]
        public void BundleScriptNoType()
        {
            AssertScriptBundle(BundleType.None);
        }

        [Test]
        public void BundleStyle()
        {
            AssertStyleBundle(BundleType.Style);
        }

        [Test]
        public void BundleStyleNoType()
        {
            AssertStyleBundle(BundleType.None);
        }
    }
}