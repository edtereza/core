using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Database.SQLite
{
    public class TableColumn : System.IDisposable
    {
        public enum ColumnType
        {
            Text,
            Integer,
            Numeric,
            Real,
            Datetime,
        }
        public System.String Name { get; set; }
        public Core.Database.SQLite.TableColumn.ColumnType Type { get; set; }
        public System.String Default { get; set; }
        public System.Boolean IsUnique { get; set; }
        public System.Boolean AllowNull { get; set; }
        public System.Boolean PrimaryKey { get; set; }
        public System.Boolean AutoIncrement { get; set; }
        public TableColumn()
        {
            this.Name = null;
            this.Type = Core.Database.SQLite.TableColumn.ColumnType.Text;
            this.Default = null;
            this.IsUnique = false;
            this.AllowNull = false;
            this.PrimaryKey = false;
            this.AutoIncrement = false;
        }

        public void Dispose()
        {
            
        }
    }
}
