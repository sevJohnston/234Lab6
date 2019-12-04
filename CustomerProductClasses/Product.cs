using System;
using System.Collections.Generic;

using ToolsCSharp;
using CustomerProductPropsClasses;
using CustomerProductDBClasses;

namespace CustomerProductClasses
{
    public class Product : BaseBusiness
    {
        //properties
        /// Read-only ID property.
        /// </summary>
        public int ProductID
        {
            get
            {
                return ((ProductProps)mProps).ID;
            }
        }

        /// Read/Write property. 
        /// </summary>
        public string ProductCode
        {
            get
            {
                return ((ProductProps)mProps).productCode;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).productCode))
                {
                    if (value.Length > 0 && value.Length <= 10)  //validation
                    {
                        mRules.RuleBroken("ProductCode", false);
                        ((ProductProps)mProps).productCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("ProductCode must be a string of up to 10 characters.");
                    }
                }
            }
        }

        /// Read/Write property. 
        /// </summary>
        public string Description
        {
            get
            {
                return ((ProductProps)mProps).description;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).description))
                {
                    if (value.Length > 0 && value.Length <= 50)  //validation, name is a string
                    {
                        mRules.RuleBroken("Description", false);
                        ((ProductProps)mProps).description = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Description must be a string of from 1 to 50 characters.");
                    }
                }
            }
        }

        public decimal UnitPrice
        {
            get
            {
                return ((ProductProps)mProps).unitPrice;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).unitPrice))
                {
                    if (value > 0m && value < 500m)  //validation, name is a string
                    {
                        mRules.RuleBroken("UnitPrice", false);
                        ((ProductProps)mProps).unitPrice = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Description must be a string of from 1 to 50 characters.");
                    }
                }
            }
        }

        public int OnHandQuantity
        {
            get
            {
                return ((ProductProps)mProps).onHandQuantity;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).onHandQuantity))
                {
                    if (value > 0 && value <= 10000)  //validation
                    {
                        mRules.RuleBroken("OnHandQuantity", false);
                        ((ProductProps)mProps).onHandQuantity = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("OnHandQuantity must be a number from 1 to 1000.");
                    }
                }
            }
        }

        //constructors
        public Product() : base()
        {
            SetUp();
            SetRequiredRules();
            SetDefaultProperties();
        }

        /// <summary>
        /// One arg constructor.
        /// Calls methods SetUp(), SetRequiredRules(), 
        /// SetDefaultProperties() and BaseBusiness one arg constructor.
        /// </summary>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Product(string cnString)
            : base(cnString)
        {
            mConnectionString = cnString;
            SetUp();
            SetRequiredRules();
            SetDefaultProperties();
        }

        /// <summary>
        /// Two arg constructor.
        /// Calls methods SetUp() and Load().
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Product(int key, string cnString)
            : base(key, cnString)
        {
            mConnectionString = cnString;
            SetUp();
            Load(key);
        }

        public Product(int key)
            : base(key)
        {
            SetUp();
            Load(key);
        }

        // *** I added these 2 so that I could create a 
        // business object from a properties object
        // I added the new constructors to the base class
        public Product(ProductProps props)
            : base(props)
        {
            SetUp();
            LoadProps(props);
        }

        public Product(ProductProps props, string cnString)
            : base(props, cnString)
        {
            mConnectionString = cnString;
            SetUp();
            LoadProps(props);
        }

        //setUp		
        protected override void SetDefaultProperties()
        {
        }

        /// <summary>
        /// Sets required fields for a record.
        /// </summary>
        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("ProductCode", true);
            mRules.RuleBroken("Description", true);
            mRules.RuleBroken("UnitPrice", true);
            mRules.RuleBroken("OnHandQuantity", true);           
        }

        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects.
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new ProductDB();
                mdbWriteable = new ProductDB();
            }

            else
            {
                mdbReadable = new ProductDB(this.mConnectionString);
                mdbWriteable = new ProductDB(this.mConnectionString);
            }
        }

        public override object GetList()
        {
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();


            props = (List<ProductProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (ProductProps prop in props)
            {
                Product p = new Product(prop, this.mConnectionString);
                products.Add(p);
            }

            return products;
        }
    }
}