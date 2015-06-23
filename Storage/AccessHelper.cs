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
            //string connstr = "Provider= Microsoft.ACE.OLEDB.12.0;Data Source=./AutoCloseComputerDB.accdb";
            string connstr = "Provider= Microsoft.Jet.OLEDB.4.0;Data Source=./AutoCloseComputerDB.mdb";
            OleDbConnection tempconn = new OleDbConnection(connstr);
            return (tempconn);
        }
    }
}
