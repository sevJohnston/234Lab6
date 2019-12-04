﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using statements from Mari's Event Stuff Project
using System.Xml.Serialization;
using System.IO;
using ToolsCSharp;
//create an alias for the data reader make it easy to change DB types
using DBDataReader = System.Data.SqlClient.SqlDataReader;

namespace CustomerProductPropsClasses
{
    //copy and paste from Mari's Event Stuff-- Event Props
    [Serializable()]
    public class CustomerProps : IBaseProps
    {

        #region instance variables
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public string name = "";

        /// <summary>
        /// 
        /// </summary>
        public string address = "";
        

        /// <summary>
        /// 
        /// </summary>
        public string city = "";

        /// <summary>
        /// 
        /// </summary>
        public string state = "";

        /// <summary>
        /// 
        /// </summary>
        public string zipCode = "";

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor. This object should only be instantiated by Customer, not used directly.
        /// </summary>
        public CustomerProps()
        {
        }

        #endregion

        #region BaseProps Members
        /// <summary>
        /// Serializes this props object to XML, and writes the key-value pairs to a string.
        /// </summary>
        /// <returns>String containing key-value pairs</returns>	
        public string GetState()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.GetStringBuilder().ToString();
        }

        // I don't always want to generate xml in the db class so the 
        // props class can read in from xml
        public void SetState(DBDataReader dr)
        {
            this.ID = (Int32)dr["CustomerID"];
            this.name = (string)dr["Name"];
            this.address = dr["Address"].ToString();  
            this.city = (string)dr["City"];
            this.state = ((string)dr["State"]).Trim();
            this.zipCode = ((string)dr["ZipCode"]).Trim();
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            CustomerProps p = (CustomerProps)serializer.Deserialize(reader);
            this.ID = p.ID;
            this.name = p.name;
            this.address = p.address;
            this.city = p.city;
            this.state = p.state;
            this.zipCode = p.zipCode;
            this.ConcurrencyID = p.ConcurrencyID;
        }
        #endregion

        #region ICloneable Members
        /// <summary>
        /// Clones this object.
        /// </summary>
        /// <returns>A clone of this object.</returns>
        public Object Clone()
        {
            CustomerProps p = new CustomerProps();
            p.ID = this.ID;
            p.name = this.name;
            p.address = this.address;
            p.city = this.city;
            p.state = this.state;
            p.zipCode = this.zipCode;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }
        #endregion

    }
}
