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

namespace CustomerTests
{
    [TestFixture]
    public class CustomerClassTests
    {
        CustomerDB db;
        private string dataSource = "Data Source=LAPTOP-9AAN6UE4\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            db = new CustomerDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestCustomerConstructor()
        {
            Customer c = new Customer(dataSource);
            Console.WriteLine(c.ToString());
            Assert.Greater(c.ToString().Length, 1);
        }

        [Test]
        public void TestRetrieveCustomerFromDataStoreConstructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(157, dataSource);
            Assert.AreEqual(c.ZipCode, "14514");
            Assert.AreEqual(c.State, "NY");
            Console.WriteLine(c.ToString());
        }

        [Test]
        public void TestSaveCustomerToDataStore()
        {
            Customer c = new Customer(dataSource);
            c.Name = "Aaron, Bob";
            c.Address = "23 Main Street";
            c.City = "Eugene";
            c.State = "OR";
            c.ZipCode = "97403";
            c.Save();
            Assert.AreEqual(700, c.CustomerID);
        }

        [Test]
        public void TestCustomerUpdate()
        {
            Customer c = new Customer(157, dataSource);

            c.Name = "Aaron, Babs";
            c.Address = "23 Main Street";
            c.City = "Eugene";
            c.State = "OR";
            c.ZipCode = "97403";
            c.Save();

            c = new Customer(157, dataSource);
            Assert.AreEqual(c.Name, "Aaron, Babs");
            Assert.AreEqual(c.Address, "23 Main Street");
            Assert.AreEqual(c.City, "Eugene");
            Assert.AreEqual(c.State, "OR");
            Assert.AreEqual(c.ZipCode, "97403");
        }

        [Test]
        public void TestCustomerDelete()
        {
            Customer c = new Customer(2, dataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(2, dataSource));
        }

        [Test]
        public void TestCustomerGetList()
        {
            Customer c = new Customer(dataSource);
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("North Chili", customers[0].City);
            Assert.AreEqual("NY", customers[0].State);
        }

        [Test]
        public void TestSomeRequiredCustomerPropertiesNotSet()
        {
            // not in Data Store - name, address, city, state, and zipcode must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "Blow, Joe";
            Assert.Throws<Exception>(() => c.Save());
            c.Address = "292 Test Lane";
            Assert.Throws<Exception>(() => c.Save());
            c.City = "Hometown";
            Assert.Throws<Exception>(() => c.Save());
            c.State = "OR";
            Assert.Throws<Exception>(() => c.Save());
            c.ZipCode = "97401";
            //How could you assert its all here???
            //where is this console.writeline going?
            Console.WriteLine(c.ToString());
        }

        [Test]
        public void TestInvalidCustomerPropertySet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = "Johnston, Alice Amelia Bedelia Josephine Hester " +
            "Alma Severena Mary Elizabeth Janice Jennifer Gloria Francis Madigan Isabella May Lou Dawn");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Address = "33676 McKenzie View Lane Drive Alley Street Highway" +
            "Boulevard Rue Road Throughfare Way Avenue Loop");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.City = "Supercalifragalisticexpealadocius-Bumdiddlydiddlydee");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.State = "Oregon");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.ZipCode = "1234567890123456");
        }
    }
}
