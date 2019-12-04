using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;
using CustomerProductDBClasses;
using System.Data;
using System.Data.SqlClient;
using DBCommand = System.Data.SqlClient.SqlCommand;

namespace ProductTests
{
    [TestFixture]
    public class ProductDBTests
    {
        ProductDB db;
        private string dataSource = "Data Source=LAPTOP-9AAN6UE4\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            db = new ProductDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            //fix for product db
            ProductProps props = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(56.50m, props.unitPrice);
            Assert.AreEqual(4637, props.onHandQuantity);
            Assert.AreEqual("A4CS", props.productCode);
        }

        [Test]
        public void TestRetrieveInvalidKey()
        {    
            //should throw exception if asked for record that doesn't exist
            Assert.Throws<Exception>(() => db.Retrieve(100));
        }

        [Test]
        public void TestRetrieveAll()
        {
            //products db has 16 records
            List<ProductProps> props = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(props.Count, 16);
        }

        [Test]
        public void TestCreate()
        {
            ProductProps props = new ProductProps();
            props.productCode = "B4UC";
            props.description = "Great Book";
            props.unitPrice = 5.99m;
            props.onHandQuantity = 25;
            
            ProductProps newProps = (ProductProps)db.Create(props);
            props = (ProductProps)db.Retrieve(newProps.ID);
            Assert.AreEqual(props.productCode, newProps.productCode);
            Assert.AreEqual(props.description, newProps.description);
            Assert.AreEqual(props.onHandQuantity, newProps.onHandQuantity);
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps props = (ProductProps)db.Retrieve(1);           
            props.productCode = "FROG";
            props.description = "Another great book";
            props.unitPrice = 9.99m;
            props.onHandQuantity = 25;           
            db.Update(props);
            ProductProps updatedProps = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("FROG", updatedProps.productCode);
            Assert.AreEqual("Another great book", updatedProps.description);
        }

        [Test]
        public void TestDelete()
        {
            List<ProductProps> otherprops = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(16, otherprops.Count);
            ProductProps props = (ProductProps)db.Retrieve(1);
            db.Delete(props);
            otherprops = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(15, otherprops.Count);
        }
    }
}
