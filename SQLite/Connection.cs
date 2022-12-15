using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Database.SQLite
{
    internal class Connection : System.IDisposable
    {
        private System.String _fileName = null;
        private System.String _filePath = null;
        private System.String _fullPath = null;

        //:: Objeto manipulador da base de dados do LOG
        internal Core.Database.SQLite.SqliteConnection _sqliteConnectionHandler { get; set; }

        public Connection() : this(System.Reflection.Assembly.GetEntryAssembly().GetName().Name, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)) { }

        public Connection(System.String Name) : this(Name, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)) { }

        public Connection(System.String Name, System.String Path)
        {
            this._fileName = Name;
            this._filePath = Path;
            this._filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (!this._filePath.EndsWith(@"\"))
                this._filePath = this._filePath + @"\";
            if (!this._fileName.EndsWith(@".db"))
                this._fileName = this._fileName + @".db";
            this._fullPath = this._filePath + this._fileName;
            this._sqliteConnectionHandler = null;
        }
        internal System.String ConnectionString()
        {
            return "Version=3,uri=file:" + this._fullPath;
        }
        internal System.Boolean Open(System.Int32 Retry = 0)
        {
            System.Boolean _hasOpened = true;
            if (this._sqliteConnectionHandler != null)
            {
                switch (this._sqliteConnectionHandler.State)
                {
                    case System.Data.ConnectionState.Closed:
                    case System.Data.ConnectionState.Broken:
                        _hasOpened = false;
                        break;
                    default:
                        _hasOpened = true;
                        break;
                }
            }
            else
            {
                _hasOpened = false;
            }
            if (_hasOpened == false)
            {
                this.Dispose();
                this._sqliteConnectionHandler = new Core.Database.SQLite.SqliteConnection();
                this._sqliteConnectionHandler.ConnectionString = this.ConnectionString();
                try
                {
                    this._sqliteConnectionHandler.Open();
                }
                catch { }
                switch (this._sqliteConnectionHandler.State)
                {
                    case System.Data.ConnectionState.Closed:
                    case System.Data.ConnectionState.Broken:
                        _hasOpened = false;
                        break;
                    default:
                        _hasOpened = true;
                        break;
                }
            }
            return _hasOpened;
        }
        public bool Close()
        {
            System.Boolean _hasClosed = false;
            if (this._sqliteConnectionHandler == null)
            {
                _hasClosed = true;
            }
            else
            {
                switch (this._sqliteConnectionHandler.State)
                {
                    case System.Data.ConnectionState.Closed:
                        _hasClosed = true;
                        break;
                }
                if (_hasClosed == false)
                {
                    try
                    {
                        this._sqliteConnectionHandler.Close();
                    }
                    catch { }
                }
            }
            return _hasClosed;
        }
        public void Dispose()
        {
            if (this._sqliteConnectionHandler != null)
                this.Close();
            try
            {
                this._sqliteConnectionHandler.Dispose();
                this._sqliteConnectionHandler = null;
            }
            catch { }
        }
    }
}
