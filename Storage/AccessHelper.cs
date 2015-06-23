using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCloseComputer.Storage
{
    public class AccessHelper
    {
        public static OleDbConnection getConn()
        {
            string connstr = "Provider= Microsoft.Jet.OLEDB.4.0;Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AutoCloseSetup/AutoCloseComputerDB.mdb";
            OleDbConnection tempconn = new OleDbConnection(connstr);
            return (tempconn);
        }
    }
}
