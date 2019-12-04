using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;

namespace ProductTests
{
    [TestFixture]
    public class ProductPropsTests
    {
        ProductProps props = new ProductProps();

        [SetUp]
        public void SetUp()
        {
            props = new ProductProps();
            props.ID = 1;
            props.productCode = "G0012";
            props.description = "Great book for class.";
            props.unitPrice = 49.99m;
            props.onHandQuantity = 11;           
            props.ConcurrencyID = 4;
        }

        [Test]
        public void GetStateTest()
        {
            ProductProps props = new ProductProps();
            props.ID = 1;
            props.productCode = "G0012";
            props.description = "Great book for class.";
            props.unitPrice = 49.99m;
            props.onHandQuantity = 11;
            props.ConcurrencyID = 4;

            string output = props.GetState();
            Console.WriteLine(output);
        }

        [Test]
        public void SetStateTest()
        {
            ProductProps props2 = new ProductProps();
            props2.SetState(props.GetState());
            Assert.AreEqual(props.ID, props2.ID);
            Assert.AreEqual(props.productCode, props2.productCode);
            Assert.AreEqual(props.description, props2.description);
            Assert.AreEqual(props.unitPrice, props2.unitPrice);
            Assert.AreEqual(props.onHandQuantity, props2.onHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, props2.ConcurrencyID);
        }

        [Test]
        public void CloneTest()
        {
            ProductProps cloneProps = (ProductProps)props.Clone();
            Assert.AreEqual(props.ID, cloneProps.ID);
            Assert.AreEqual(props.productCode, cloneProps.productCode);
            Assert.AreEqual(props.description, cloneProps.description);
            Assert.AreEqual(props.unitPrice, cloneProps.unitPrice);
            Assert.AreEqual(props.onHandQuantity, cloneProps.onHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, cloneProps.ConcurrencyID);
        }
    }
}
