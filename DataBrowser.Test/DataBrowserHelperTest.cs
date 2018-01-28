using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataBrowser.Helper.QueryBuilder;

namespace DataBrowser.Test
{
    [TestClass]
    public class DataBrowserHelperTest
    {
        [TestMethod]
        public void BasicTest()
        {
            var query = Builder.From("Employees").Select("Forename").Select("Surname").Where("Employees","Forename", ComparerOperator.EqualTo, "TestFirstName3");
        }
    }
}
