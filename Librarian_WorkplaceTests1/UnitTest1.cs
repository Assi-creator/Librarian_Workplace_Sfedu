using AРМ_Библиотекаря;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Librarian_WorkplaceTests1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IsValidText()
        {
            //arrange
            string text = "Мария";
            bool isMatch = true;

            //act 
            bool actual = BasicFunction.validateName(text);

            //assert
            Assert.AreEqual(isMatch, actual);
        }
    
        [TestMethod]
        public void IsValidNumber()
        {
            //arrange
            string text = "135";
            bool isMatch = true;

            //act 
            bool actual = BasicFunction.validateNumber(text);

            //assert
            Assert.AreEqual(isMatch, actual);
        }
    }
}
