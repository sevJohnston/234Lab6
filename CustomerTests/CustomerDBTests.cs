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

namespace CustomerTests
{
    [TestFixture]
    public class CustomerDBTests
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
        public void TestRetrieve()
        {
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("1108 Johanna Bay Drive", props.address);
            Assert.AreEqual("AL", props.state);
            Assert.AreEqual("35216-6909", props.zipCode);
        }

        [Test]
        public void TestRetrieveInvalidKey()
        {
            //CustomerProps props = (CustomerProps)db.Retrieve(800);
            Assert.Throws<Exception>(() => db.Retrieve(900));
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> props = (List<CustomerProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(props.Count, 696);
        }

        [Test]
        public void TestCreate()
        {
            CustomerProps props = new CustomerProps();
            props.name = "Bob Bob";
            props.address = "McKenzie View Drive";
            props.city = "Springfield";
            props.state = "OR";
            props.zipCode = "97477";
            CustomerProps newProps = (CustomerProps)db.Create(props);
            props = (CustomerProps)db.Retrieve(newProps.ID);
            Assert.AreEqual(props.name, newProps.name);
            Assert.AreEqual(props.state, newProps.state);
            Assert.AreEqual(props.zipCode, newProps.zipCode);
        }

        [Test]
        public void TestUpdate()
        {
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            props.name = "Frog, Frog";
            props.address = "123 Saturday Market";
            props.city = "Eugene";
            props.state = "OR";
            props.zipCode = "97403";
            db.Update(props);
            CustomerProps updatedProps = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("Frog, Frog", updatedProps.name);
            Assert.AreEqual("Eugene", updatedProps.city);
        }

        [Test]
        public void TestDelete()
        {     
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            db.Delete(props);
            List<CustomerProps> otherprops = (List<CustomerProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(otherprops.Count, 695);
        }
    }
}
