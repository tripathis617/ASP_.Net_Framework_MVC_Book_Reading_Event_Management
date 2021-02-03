using BookReadingEventManagement.Controllers;
using BookReadingEventManagement.Data;
using Moq;
using NUnit.Framework;
using BookReadingEventManagement.Models;
using System.Collections.Generic;

namespace UnitTestingProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSumIndex()
        {
            var test1 = new FirstTest();
            int result = test1.SumIndex(2, 3);
            Assert.IsTrue(result == 5);
        }

        [Test]
        public void TestMinusIndex()
        {
            var test1 = new FirstTest();
            int result = test1.MinusIndex(2, 3);
            Assert.IsTrue(result == -1);
        }

        [Test]
        public void TestMultiplyIndex()
        {
            var test1 = new FirstTest();
            int result = test1.MultiplyIndex(2, 3);
            Assert.IsTrue(result == 6);
        }
    }
}