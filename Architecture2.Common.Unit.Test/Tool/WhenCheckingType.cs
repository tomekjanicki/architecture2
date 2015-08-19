using System;
using Architecture2.Common.Exception.Logic;
using Architecture2.Common.Exception.Logic.Base;
using Architecture2.Common.Test;
using Architecture2.Common.Tool;
using NUnit.Framework;

namespace Architecture2.Common.Unit.Test.Tool
{
    public class WhenCheckingType : BaseTest
    {
        [Test]
        public void ShouldReturnTrue_IfArgumentIsTheSameTypeAsExpectedType()
        {
            var exception = new ArgumentNullException();

            Assert.That(Extension.IsType(exception, typeof(ArgumentNullException)));
        }

        [Test]
        public void ShouldReturnFalse_IfArgumentIsNotTheSameTypeAsExpectedTypeOrSubTypeOfExpectedType()
        {
            var exception = new ArgumentNullException();

            Assert.That(!Extension.IsType(exception, typeof(NotSupportedException)));
        }

        [Test]
        public void ShouldReturnTrue_IfArgumentIsSubTypeOfExpectedType()
        {
            var exception = new ArgumentNullException();

            Assert.That(Extension.IsType(exception, typeof(System.Exception)));
        }

        [Test]
        public void ShouldReturnTrue_IfGenericArgumentIsTheSameTypeAsExpectedType()
        {
            var exception = new NotFoundException<object>("");

            Assert.That(Extension.IsType(exception, typeof(NotFoundException<>)));
        }

        [Test]
        public void ShouldReturnTrue_IfGenericArgumentIsSubTypeOfExpectedType()
        {
            var exception = new NotFoundException<object>("");

            Assert.That(Extension.IsType(exception, typeof(BaseLogicException<>)));
        }

    }
}
