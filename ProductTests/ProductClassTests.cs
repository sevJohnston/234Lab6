using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using CustomerProductClasses;
using CustomerProductDBClasses;
using CustomerProductPropsClasses;
using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;


namespace ProductTests
{
    [TestFixture]
    public class ProductClassTests
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
        public void TestProductConstructor()
        {
            Product p = new Product(dataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
        }

        [Test]
        public void TestRetrieveProductFromDataStoreConstructor()
        {
            // retrieves from Data Store
            Product p = new Product(1, dataSource);
            Assert.AreEqual(p.ProductID, 1);
            Assert.AreEqual(p.ProductCode, "A4CS");
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestSaveProductToDataStore()
        {
            Product p = new Product(dataSource);          
            p.ProductCode = "BLAH";
            p.Description = "Coolest book ever.";
            p.UnitPrice = 23.95m;
            p.OnHandQuantity = 1000;
            p.Save();
            Assert.AreEqual(17, p.ProductID);
        }

        [Test]
        public void TestProductUpdate()
        {
            Product p = new Product(1, dataSource);
           
            p.ProductCode = "BLAH";
            p.Description = "Second coolest book ever.";
            p.UnitPrice = 45.95m;
            p.OnHandQuantity = 600;
            p.Save();

            p = new Product(1, dataSource);
            Assert.AreEqual(p.ProductID, 1);
            Assert.AreEqual(p.Description, "Second coolest book ever.");
            Assert.AreEqual(p.UnitPrice, 45.95m);
            Assert.AreEqual(p.OnHandQuantity, 600);
        }

        [Test]
        public void TestProductDelete()
        {
            Product p = new Product(2, dataSource);
            p.Delete();
            p.Save();          
            Assert.Throws<Exception>(() => new Product(2, dataSource));
        }

        [Test]
        public void TestProductGetList()
        {
            Product p = new Product(dataSource);
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            Assert.AreEqual(4637, products[0].OnHandQuantity);
        }

        [Test]
        public void TestSomeRequiredProductPropertiesNotSet()
        {
            // not in Data Store - productCode, description, unitPrice, and onHandQuantity must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
            p.ProductCode = "BLOB";
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "this is a test";
            Assert.Throws<Exception>(() => p.Save());
            p.UnitPrice = 199.99m;
            Assert.Throws<Exception>(() => p.Save());
            p.OnHandQuantity = 2;
            //How could you assert its all here???
            //where is this console.writeline going?
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestInvalidProductPropertySet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "9999999999999");
            Assert.Throws<ArgumentOutOfRangeException>(() => p.Description = "This product is a delightful piece of garbage" +
            " poorly organized, with sharp edges, a repulsive smell, and very small print. Guaranteed to give you a headache. Enjoy!");
            Assert.Throws<ArgumentOutOfRangeException>(() => p.UnitPrice = 999m);
            Assert.Throws<ArgumentOutOfRangeException>(() => p.OnHandQuantity = 10001);
        }
    }
}
