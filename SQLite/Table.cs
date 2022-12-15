using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Database.SQLite
{
    public class Table : System.IDisposable
    {
        public System.String Name { get; set; }
        public System.Collections.Generic.List<Core.Database.SQLite.TableChild> Childs { get; set; }
        public System.Collections.Generic.List<Core.Database.SQLite.TableColumn> Columns { get; set; }
        public Table()
        {
            this.Name = null;
            this.Childs = new List<Core.Database.SQLite.TableChild>();
            this.Columns = new List<Core.Database.SQLite.TableColumn>();
        }
        public System.Boolean AddColumn(Core.Database.SQLite.TableColumn Column)
        {
            System.Boolean _addColumn = true;
            for (int _addColumnLoop = 0; _addColumnLoop < this.Columns.Count(); _addColumnLoop++)
            {
                if (this.Columns[_addColumnLoop] != null && this.Columns[_addColumnLoop].Name == Column.Name)
                {
                    _addColumn = false;
                    break;
                }
            }
            if (_addColumn)
                this.Columns.Add(Column);
            return _addColumn;
        }
        public System.Boolean AddChild(Core.Database.SQLite.TableChild Child)
        {
            bool _addChild = true;
            for (int _loopAddChild = 0; _loopAddChild < this.Childs.Count(); _loopAddChild++)
            {
                if (this.Childs[_loopAddChild].Table == Child.Table)
                {
                    if (this.Childs[_loopAddChild].Field == Child.Field || this.Childs[_loopAddChild].Ownership == Child.Ownership)
                    {
                        _addChild = false;
                        break;
                    }
                }
            }
            if (_addChild)
            {
                this.Childs.Add(Child);
            }
            return _addChild;
        }
        internal string ScriptDrop()
        {
            string _scriptDrop = null;
            if (this.Name != null)
            {
                _scriptDrop = "DROP TABLE " + this.Name + ";";
            }
            return _scriptDrop;
        }
        internal string ScriptCreate()
        {
            string _scriptCreate = null;
            if (this.Name != null && this.Columns.Count() != 0)
            {
                _scriptCreate = "";
                for (int _columnIndex = 0; _columnIndex < this.Columns.Count(); _columnIndex++)
                {
                    if (_scriptCreate.Equals("") == false)
                        _scriptCreate = _scriptCreate + ", ";
                    _scriptCreate = _scriptCreate + this.Columns[_columnIndex].Name.ToUpper();
                    _scriptCreate = _scriptCreate + " ";
                    _scriptCreate = _scriptCreate + this.Columns[_columnIndex].Type.ToString().ToUpper();
                    if (this.Columns[_columnIndex].PrimaryKey == true)
                        _scriptCreate = _scriptCreate + " PRIMARY KEY";
                    if (this.Columns[_columnIndex].AutoIncrement == true)
                        _scriptCreate = _scriptCreate + " AUTOINCREMENT";
                    if (this.Columns[_columnIndex].AllowNull == false)
                        _scriptCreate = _scriptCreate + " NOT NULL";
                    if (this.Columns[_columnIndex].IsUnique == true)
                        _scriptCreate = _scriptCreate + " UNIQUE";
                    if (this.Columns[_columnIndex].Default != null && this.Columns[_columnIndex].Default.Length != 0)
                        switch (this.Columns[_columnIndex].Type)
                        {
                            case Core.Database.SQLite.TableColumn.ColumnType.Text:
                                _scriptCreate = _scriptCreate + " DEFAULT \"" + this.Columns[_columnIndex].Default.Trim() + "\"";
                                break;
                            case Core.Database.SQLite.TableColumn.ColumnType.Integer:
                                _scriptCreate = _scriptCreate + " DEFAULT " + Convert.ToInt32(this.Columns[_columnIndex].Default.Trim()).ToString();
                                break;
                            case Core.Database.SQLite.TableColumn.ColumnType.Numeric:
                                _scriptCreate = _scriptCreate + " DEFAULT " + Convert.ToDouble(this.Columns[_columnIndex].Default.Trim()).ToString();
                                break;
                            case Core.Database.SQLite.TableColumn.ColumnType.Real:
                                _scriptCreate = _scriptCreate + " DEFAULT " + Convert.ToDecimal(this.Columns[_columnIndex].Default.Trim()).ToString();
                                break;
                            case Core.Database.SQLite.TableColumn.ColumnType.Datetime:
                                if (this.Columns[_columnIndex].Default.ToUpper().Equals("NOW") == true)
                                    _scriptCreate = _scriptCreate + " DEFAULT (datetime('now','localtime'))";
                                else
                                    _scriptCreate = _scriptCreate + " DEFAULT " + this.Columns[_columnIndex].Default;
                                break;
                        }
                    else if (this.Columns[_columnIndex].Default == null && this.Columns[_columnIndex].AllowNull == true)
                        _scriptCreate = _scriptCreate + " DEFAULT NULL";
                }
                if (_scriptCreate.Equals("") == false)
                    _scriptCreate = "CREATE TABLE " + this.Name.ToUpper() + " (" + _scriptCreate + ")";
            }
            return _scriptCreate;
        }

        public void Dispose()
        {
        }
    }
}
