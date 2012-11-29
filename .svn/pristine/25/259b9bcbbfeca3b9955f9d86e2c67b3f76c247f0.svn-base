using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySQLData.DataAccess
{
    class DatabaseInfo
    {
        public DatabaseInfo()
        {
            this.ServerName = "localhost";
            this.DatabaseName = "test";
            this.TableName = "Loan_Master";
        }


        public string ServerName
        {
            get
            {
                return m_serverName;
            }
            set
            {
                if (!string.Equals(m_serverName, value))
                {
                    m_serverName = value;
                }
            }
        }

        public string DatabaseName
        {
            get
            {
                return m_databaseName;
            }
            private set
            {
                if (!string.Equals(m_databaseName, value))
                {
                    m_databaseName = value;
                }
            }
        }

        public string TableName
        {
            get
            {
                return m_tableName;
            }
            set
            {
                if (!string.Equals(m_tableName, value))
                {
                    m_tableName = value;
                }
            }
        }

        public string Username
        {
            get
            {
                return m_username;
            }
            set
            {
                m_username = value;
            }
        }


        #region DATABASE TOOLS

        private string GetRootConnectionString()
        {
            return this.GetConnectionString("test");
        }

        internal string GetConnectionString()
        {
            return this.GetConnectionString(m_databaseName);
        }

        private string GetConnectionString(string initialCatalog)
        {
            string connectionString = "server=localhost;User Id=root;port=5029;database=test"; // "Data Source=" + m_serverName + ";Initial Catalog=" + initialCatalog;

            return connectionString;
        }


        internal void TestConnection()
        {
            MySqlConnection connection = new MySqlConnection(this.GetRootConnectionString());

            connection.Open();
            connection.Close();
        }

        internal void CheckExistance()
        {
            MySqlConnection connection = new MySqlConnection(this.GetConnectionString());
            connection.Open();
            try
            {
                string countQuery = "SELECT COUNT( * ) FROM " + m_tableName;

                MySqlCommand command = new MySqlCommand(countQuery, connection);
                command.ExecuteScalar();
            }
            finally
            {
                connection.Close();
            }
        }

        private MySqlConnection CreateSqlConnection()
        {
            return new MySqlConnection(this.GetConnectionString());
        }

        #endregion DATABASE TOOLS


        #region PRIVATE FIELDS

        private string m_serverName;
        private string m_databaseName;
        private string m_tableName;

        private string m_username;

        #endregion PRIVATE FIELDS

    }
}
