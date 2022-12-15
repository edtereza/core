using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Database.SQLite
{
    public class TableChild : System.IDisposable
    {
        public string Table { get; set; }
        public string Field { get; set; }
        public string Ownership { get; set; }
        public TableChild()
        {
            this.Table = null;
            this.Field = null;
            this.Ownership = null;
        }

        public void Dispose()
        {
            
        }
    }
}
