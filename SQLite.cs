using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class SQLite : System.IDisposable
    {
        private System.String _fileName = null;
        private System.String _filePath = null;
        private System.String _fullPath = null;

        private Core.Database.SQLite.Connection _sqliteConnection = null;

        public SQLite() : this(System.Reflection.Assembly.GetEntryAssembly().GetName().Name, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)) { }

        public SQLite(System.String Name) : this(Name, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)) { }

        public SQLite(System.String Name, System.String Path)
        {
            this._fileName = Name;
            this._filePath = Path;
            this._filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (!this._filePath.EndsWith(@"\"))
                this._filePath = this._filePath + @"\";
            if (!this._fileName.EndsWith(@".db"))
                this._fileName = this._fileName + @".db";
            this._fullPath = this._filePath + this._fileName;
            this._sqliteConnection = new Core.Database.SQLite.Connection(this._fileName, this._filePath);
        }

        public System.Boolean Open()
        {
            return this._sqliteConnection.Open();
        }
        public System.Boolean Close()
        {
            return this._sqliteConnection.Close();
        }

        public void Dispose()
        {
            this._sqliteConnection.Dispose();
        }

        public System.Boolean TableExists(Core.Database.SQLite.Table Table)
        {
            System.Boolean _tableExists = false;
            if (this.Open() == true)
            {
                System.Data.IDbCommand _pragmaCommand = null;
                System.Data.IDataReader _pragmaColumns = null;
                try
                {
                    _pragmaCommand = this._sqliteConnection._sqliteConnectionHandler.CreateCommand();
                }
                catch
                {
                    _pragmaCommand = null;
                }
                if (_pragmaCommand != null)
                {
                    _pragmaCommand.CommandText = "PRAGMA table_info('" + Table.Name + "');";
                    try
                    {
                        _pragmaCommand.Connection.Open();
                        _pragmaColumns = _pragmaCommand.ExecuteReader();
                    }
                    catch
                    {
                        _pragmaColumns = null;
                    }
                    if (_pragmaColumns != null)
                    {
                        if (_pragmaColumns.Read() != false)
                        {
                            _tableExists = true;
                        }
                        try
                        {
                            _pragmaColumns.Close();
                            _pragmaColumns.Dispose();
                        }
                        catch { }
                    }
                    try
                    {
                        _pragmaCommand.Connection.Close();
                        _pragmaCommand.Connection.Dispose();
                        _pragmaCommand.Dispose();
                    }
                    catch { }
                }
                this.Close();
            }
            return _tableExists;
        }
        public System.Boolean TableIntegrity(Core.Database.SQLite.Table Table)
        {
            System.Boolean _tableIntegrity = false;
            if (this.Open() == true)
            {
                int _columnMatch = 0;
                int _columnTotal = 0;
                System.Data.IDbCommand _pragmaCommand = null;
                System.Data.IDataReader _pragmaColumns = null;
                if (Table.Columns.Count() != 0)
                {
                    try
                    {
                        _pragmaCommand = this._sqliteConnection._sqliteConnectionHandler.CreateCommand();
                    }
                    catch
                    {
                        _pragmaCommand = null;
                    }
                    if (_pragmaCommand != null)
                    {
                        _pragmaCommand.CommandText = "PRAGMA table_info('" + Table.Name + "');";
                        try
                        {
                            _pragmaCommand.Connection.Open();
                            _pragmaColumns = _pragmaCommand.ExecuteReader();
                        }
                        catch { _pragmaColumns = null; }
                        if (_pragmaColumns != null)
                        {
                            using (Core.Database.SQLite.TableColumn _tableColumn = new Core.Database.SQLite.TableColumn())
                            {
                                while (_pragmaColumns.Read() == true)
                                {
                                    _columnTotal++;
                                    _tableColumn.Name = Convert.ToString(_pragmaColumns.GetValue(_pragmaColumns.GetOrdinal("name")));
                                    switch (Convert.ToString(_pragmaColumns.GetValue(_pragmaColumns.GetOrdinal("type"))).ToUpper())
                                    {
                                        case "TEXT":
                                            _tableColumn.Type = Database.SQLite.TableColumn.ColumnType.Text;
                                            break;
                                        case "INTEGER":
                                            _tableColumn.Type = Database.SQLite.TableColumn.ColumnType.Integer;
                                            break;
                                        case "NUMERIC":
                                            _tableColumn.Type = Database.SQLite.TableColumn.ColumnType.Numeric;
                                            break;
                                        case "REAL":
                                            _tableColumn.Type = Database.SQLite.TableColumn.ColumnType.Real;
                                            break;
                                        case "DATETIME":
                                            _tableColumn.Type = Database.SQLite.TableColumn.ColumnType.Datetime;
                                            break;
                                        default:
                                            _tableColumn.Type = Database.SQLite.TableColumn.ColumnType.Text;
                                            break;
                                    }
                                    for (int _tableColumns = 0; _tableColumns < Table.Columns.Count(); _tableColumns++)
                                    {
                                        if (Table.Columns[_tableColumns].Name.Equals(_tableColumn.Name) == true && Table.Columns[_tableColumns].Type == _tableColumn.Type)
                                            _columnMatch++;
                                    }
                                }
                                if (_columnMatch == Table.Columns.Count() && _columnTotal == Table.Columns.Count())
                                    _tableIntegrity = true;
                            }
                            try
                            {
                                _pragmaColumns.Close();
                            }
                            catch { }
                            try
                            {
                                _pragmaColumns.Dispose();
                            }
                            catch { }
                        }
                        try
                        {
                            _pragmaCommand.Connection.Close();
                        }
                        catch { }
                        try
                        {
                            _pragmaCommand.Dispose();
                        }
                        catch { }
                    }
                }
                this.Close();
            }
            return _tableIntegrity;
        }
        public System.Data.DataTable DataTable(System.String Query)
        {
            System.Data.DataTable _dataTable = null;
            if (this.Open())
            {
                System.Data.Common.DbDataReader _dbDataReader = null;
                System.Data.Common.DbCommand _dbCommand = null;
                System.Data.DataTable _dataTableSchema = new System.Data.DataTable();
                try
                {
                    _dbCommand = this._sqliteConnection._sqliteConnectionHandler.CreateCommand();
                }
                catch
                {
                    _dbCommand = null;
                }
                if (_dbCommand != null)
                {
                    _dbCommand.CommandText = Query.Trim();
                    try
                    {
                        _dbCommand.Connection.Open();
                        _dbDataReader = _dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        _dataTableSchema = _dbDataReader.GetSchemaTable();
                    }
                    catch
                    {
                        _dbDataReader = null;
                        _dataTableSchema = null;

                    }
                    if (_dataTableSchema != null)
                    {
                        _dataTable = new System.Data.DataTable();
                        //System.String _dataTableColumnName = null;
                        System.Data.DataColumn _dataTableColumn = null;
                        List<System.Data.DataColumn> _dataTableColumns = new List<System.Data.DataColumn>();
                        foreach (System.Data.DataRow _dataRow in _dataTableSchema.Rows)
                        {
                            //_dataTableColumnName = System.Convert.ToString(_dataRow["ColumnName"]);
                            _dataTableColumn = new System.Data.DataColumn(System.Convert.ToString(_dataRow["ColumnName"]), (Type)(_dataRow["DataType"]));
                            _dataTableColumn.Unique = System.Convert.ToBoolean(_dataRow["IsUnique"]);
                            _dataTableColumn.AllowDBNull = System.Convert.ToBoolean(_dataRow["AllowDBNull"]);
                            _dataTableColumn.AutoIncrement = System.Convert.ToBoolean(_dataRow["IsAutoIncrement"]);
                            _dataTableColumns.Add(_dataTableColumn);
                            _dataTable.Columns.Add(_dataTableColumn);
                        }
                        while (_dbDataReader.Read())
                        {
                            System.Data.DataRow _dataRow = _dataTable.NewRow();
                            for (int _dataRowLoop = 0; _dataRowLoop < _dataTableColumns.Count; _dataRowLoop++)
                            {
                                _dataRow[((System.Data.DataColumn)_dataTableColumns[_dataRowLoop])] = _dbDataReader[_dataRowLoop];
                            }
                            _dataTable.Rows.Add(_dataRow);
                        }
                        System.String _queryPart = Query.Substring((Query.IndexOf("FROM") + 4)).Trim();
                        if (_queryPart.IndexOf("INNER JOIN") != -1)
                            _queryPart = _queryPart.Substring(0, _queryPart.IndexOf("INNER JOIN"));
                        if (_queryPart.IndexOf("WHERE") != -1)
                            _queryPart = _queryPart.Substring(0, _queryPart.IndexOf("WHERE"));
                        if (_queryPart.IndexOf(";") != -1)
                            _queryPart = _queryPart.Substring(0, _queryPart.IndexOf(";"));
                        _dataTable.TableName = _queryPart.Trim();
                        _dataTable.Namespace = "Core.Database.SQLite";
                        try
                        {
                            _dataTableSchema.Dispose();
                        }
                        catch { }
                        try
                        {
                            _dbDataReader.Close();
                            _dbDataReader.Dispose();
                        }
                        catch { }
                    }
                    try
                    {
                        _dbCommand.Connection.Close();
                        _dbCommand.Connection.Dispose();
                    }
                    catch { }
                    try
                    {
                        _dbCommand.Dispose();
                    }
                    catch { }
                }
                this.Close();
            }
            return _dataTable;
        }
        public System.Data.Common.DbDataReader DBDataReader(string Query)
        {
            System.Data.Common.DbDataReader _dbDataReader = null;
            if (this.Open() == true)
            {
                System.Data.Common.DbCommand _dbCommand = null;
                try
                {
                    _dbCommand = this._sqliteConnection._sqliteConnectionHandler.CreateCommand();
                }
                catch { _dbCommand = null; }
                if (_dbCommand != null)
                {
                    _dbCommand.CommandText = Query.Trim();
                    try
                    {
                        _dbCommand.Connection.Open();
                        _dbDataReader = _dbCommand.ExecuteReader();
                        System.Threading.Thread.Sleep(100);
                    }
                    catch { _dbDataReader = null; }
                }
                else
                    _dbDataReader = null;
                this.Close();
            }
            return _dbDataReader;
        }
        public System.Boolean NonResultQuery(string Query)
        {
            System.Boolean _nonResultQuery = false;
            if (this.Open() == true)
            {
                System.Data.IDbCommand _executeNonResultQueryCommand = null;
                try
                {
                    _executeNonResultQueryCommand = this._sqliteConnection._sqliteConnectionHandler.CreateCommand();
                }
                catch
                {
                    _executeNonResultQueryCommand = null;
                }
                if (_executeNonResultQueryCommand != null)
                {
                    _executeNonResultQueryCommand.CommandText = Query.Trim();
                    try
                    {
                        _executeNonResultQueryCommand.Connection.Open();
                        _executeNonResultQueryCommand.ExecuteNonQuery();
                        _nonResultQuery = true;
                    }
                    catch
                    {
                        _nonResultQuery = false;
                    }
                }
                this.Close();
            }
            return _nonResultQuery;
        }
    }
}