using System;
using Architecture2.Common.Exception.Logic;
using Architecture2.Common.Exception.Logic.Base;
using Architecture2.Common.Tool;
using Architecture2.Common.Test;
using NUnit.Framework;

namespace Architecture2.Common.Unit.Test.Tool
{
    public class ExtensionTest : BaseTest
    {
        [Test]
        public void IsType_Type_ReturnsTrue()
        {
            var exception = new ArgumentNullException();

            Assert.That(Extension.IsType(exception, typeof(ArgumentNullException)));
        }

        [Test]
        public void IsType_Type_ReturnsFalse()
        {
            var exception = new ArgumentNullException();

            Assert.That(!Extension.IsType(exception, typeof(NotSupportedException)));
        }

        [Test]
        public void IsType_SubType_ReturnsTrue()
        {
            var exception = new ArgumentNullException();

            Assert.That(Extension.IsType(exception, typeof(System.Exception)));
        }

        [Test]
        public void IsType_GenericType_ReturnsTrue()
        {
            var exception = new NotFoundException<object>("");

            Assert.That(Extension.IsType(exception, typeof(NotFoundException<>)));
        }

        [Test]
        public void IsType_SubGenericType_ReturnsTrue()
        {
            var exception = new NotFoundException<object>("");

            Assert.That(Extension.IsType(exception, typeof(BaseLogicException<>)));
        }

    }
}
