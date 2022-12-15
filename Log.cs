using System;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Runtime.InteropServices;
using Core.Database.SQLite;

namespace Core
{
    public class Log
    {
        public enum EntryLevel
        {
            Exception = 0,
            Fatal = 1,
            Error = 2,
            Warning = 3,
            Information = 4,
            Debug = 5,
            Trace = 6
        }
        public struct EntryStorage
        {
            public System.Boolean File { get; set; }
            public System.Boolean SQLite { get; set; }
        }
        protected struct MethodBase
        {
            public System.String Name;
            public Core.Log.DeclaringType DeclaringType;
        }
        protected struct DeclaringType
        {
            public System.String Namespace;
            public System.String Name;
        }
        protected struct Entry
        {
            public Core.Log.EntryLevel Level;
            public System.String Message;
            public System.DateTime DateTime;
            public Core.Log.MethodBase MethodBase;
        }

        private System.String _fileName = null;
        private System.String _filePath = null;
        private System.String _fullPath = null;
        private System.Boolean _enabled = true;
        private Core.SQLite _sqlite = null;
        private Core.Log.EntryStorage _storage = new EntryStorage()
        {
            File = false,
            SQLite = false,
        };

        public Core.Log.EntryLevel Level = Core.Log.EntryLevel.Warning;

        public Log() : this(System.Reflection.Assembly.GetEntryAssembly().GetName().Name, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), (new Core.Log.EntryStorage() { File = true, SQLite = true })) { }

        public Log(System.String Name) : this(Name, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), (new Core.Log.EntryStorage() { File = true, SQLite = true })) { }

        public Log(System.String Name, System.String Path, Core.Log.EntryStorage Storage)
        {
            this._fileName = Name;
            this._filePath = Path;
            this._storage = new EntryStorage()
            {
                File = Storage.File,
                SQLite = Storage.SQLite,
            };
        }
        private System.String FilePath()
        {
            System.String _filePath = this._filePath;

            if (!_filePath.EndsWith(@"\"))
                _filePath = _filePath + @"\";
            if (!System.IO.Directory.Exists(_filePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(_filePath);
                }
                catch
                {
                    this._enabled = false;
                }
            }
            if (!_filePath.EndsWith(@"Log\"))
            {
                _filePath = _filePath + @"Log\";
                if (!System.IO.Directory.Exists(_filePath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(_filePath);
                    }
                    catch
                    {
                        this._enabled = false;
                    }
                }
            }
            System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();
            _stringBuilder.Append(("0000" + System.DateTime.Now.Year.ToString()).Substring(("0000" + System.DateTime.Now.Year.ToString()).Length - 4));
            _stringBuilder.Append("-");
            _stringBuilder.Append(("0" + System.DateTime.Now.Month.ToString()).Substring(("0" + System.DateTime.Now.Month.ToString()).Length - 2));
            _stringBuilder.Append("-");
            _stringBuilder.Append(("0" + System.DateTime.Now.Day.ToString()).Substring(("0" + System.DateTime.Now.Day.ToString()).Length - 2));
            _stringBuilder.Append(@"\");
            _filePath = _stringBuilder.ToString();
            if (!System.IO.Directory.Exists(_filePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(_filePath);
                }
                catch
                {
                    this._enabled = false;
                }
            }
            if (!_filePath.EndsWith(@"\"))
                _filePath = _filePath + @"\";
            return _filePath;
        }
        private System.String FullPath()
        {
            System.String _fullPath = this.FilePath() + this._fileName;
            if (!_fullPath.EndsWith(@".log"))
                _fullPath = _fullPath + @".log";
            return _fullPath;

        }
        private void LogToSQLite(Core.Log.Entry Entry)
        {
            if (!this._enabled || !this._storage.SQLite)
                return;
            try
            {
                using (Core.SQLite _sqlite = new Core.SQLite(this._fileName, this.FullPath()))
                {
                    using (Core.Database.SQLite.Table _table = new Core.Database.SQLite.Table())
                    {
                        _table.Name = "Log";
                        _table.AddColumn(new Core.Database.SQLite.TableColumn { Name = "ID", Type = Core.Database.SQLite.TableColumn.ColumnType.Integer, PrimaryKey = true, AutoIncrement = true });
                        _table.AddColumn(new Core.Database.SQLite.TableColumn { Name = "Level", Type = Core.Database.SQLite.TableColumn.ColumnType.Text, AllowNull = false });
                        _table.AddColumn(new Core.Database.SQLite.TableColumn { Name = "Datetime", Type = Core.Database.SQLite.TableColumn.ColumnType.Datetime, IsUnique = false, AllowNull = false, Default = "NOW" });
                        _table.AddColumn(new Core.Database.SQLite.TableColumn { Name = "Message", Type = Core.Database.SQLite.TableColumn.ColumnType.Text, AllowNull = false });
                        _table.AddColumn(new Core.Database.SQLite.TableColumn { Name = "Namespace", Type = Core.Database.SQLite.TableColumn.ColumnType.Text, AllowNull = false });
                        _table.AddColumn(new Core.Database.SQLite.TableColumn { Name = "Class", Type = Core.Database.SQLite.TableColumn.ColumnType.Text, AllowNull = false });
                        _table.AddColumn(new Core.Database.SQLite.TableColumn { Name = "Function", Type = Core.Database.SQLite.TableColumn.ColumnType.Text, AllowNull = false });
                        if (!_sqlite.TableExists(_table))
                            _sqlite.NonResultQuery(_table.ScriptCreate());
                        if (_sqlite.TableExists(_table) && !_sqlite.TableIntegrity(_table))
                        {
                            _sqlite.NonResultQuery(_table.ScriptDrop());
                            if (!_sqlite.TableExists(_table))
                                _sqlite.NonResultQuery(_table.ScriptCreate());
                        }
                        if (_sqlite.TableExists(_table) && _sqlite.TableIntegrity(_table))
                        {
                            System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();
                            _stringBuilder.Append("INSERT INTO ");
                            _stringBuilder.Append(_table.Name);
                            _stringBuilder.Append("(Level, Datetime, Message, Namespace, Class, Function)");
                            _stringBuilder.Append(" VALUES ");
                            _stringBuilder.Append("(");
                            if (Entry.Level == Core.Log.EntryLevel.Fatal)
                                _stringBuilder.Append("'FATAL',");
                            else if (Entry.Level == Core.Log.EntryLevel.Error)
                                _stringBuilder.Append("'ERROR',");
                            else if (Entry.Level == Core.Log.EntryLevel.Warning)
                                _stringBuilder.Append("'WARNING',");
                            else if (Entry.Level == Core.Log.EntryLevel.Information)
                                _stringBuilder.Append("'INFORMATION',");
                            else if (Entry.Level == Core.Log.EntryLevel.Debug)
                                _stringBuilder.Append("'DEBUG',");
                            else if (Entry.Level == Core.Log.EntryLevel.Trace)
                                _stringBuilder.Append("'TRACE',");
                            else if (Entry.Level == Core.Log.EntryLevel.Exception)
                                _stringBuilder.Append("'EXCEPTION',");
                            _stringBuilder.Append("'" + Entry.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + "',");
                            _stringBuilder.Append("'" + Entry.Message + "',");
                            _stringBuilder.Append("'" + Entry.MethodBase.DeclaringType.Namespace + "',");
                            _stringBuilder.Append("'" + Entry.MethodBase.DeclaringType.Name + "',");
                            _stringBuilder.Append("'" + Entry.MethodBase.Name + "'");
                            _stringBuilder.Append(")");
                            _sqlite.NonResultQuery(_stringBuilder.ToString());
                        }
                    }
                }
            } catch { }
        }
        private void LogToFile(Core.Log.Entry Entry, System.Exception Exception)
        {
            if (!this._enabled || !this._storage.File)
                return;
            this.LogToFile(Entry);
            try
            {
                using (System.IO.StreamWriter _streamWriter = new System.IO.StreamWriter(this.FullPath(), true))
                {
                    _streamWriter.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
                    _streamWriter.WriteLine(Exception.ToString());
                    if (Exception.InnerException != null)
                    {
                        _streamWriter.WriteLine("");
                        _streamWriter.WriteLine(Exception.InnerException.ToString());
                    }
                    _streamWriter.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
                    _streamWriter.Close();
                }
            }
            catch { }
        }
        private void LogToFile(Core.Log.Entry Entry)
        {
            if (!this._enabled || !this._storage.File)
                return;
            System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();
            _stringBuilder.Append("[");
            _stringBuilder.Append(Entry.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            _stringBuilder.Append("]");
            _stringBuilder.Append(" ");
            if (Entry.Level == Core.Log.EntryLevel.Fatal)
                _stringBuilder.Append("FATAL");
            else if (Entry.Level == Core.Log.EntryLevel.Error)
                _stringBuilder.Append("ERROR");
            else if (Entry.Level == Core.Log.EntryLevel.Warning)
                _stringBuilder.Append("WARNING");
            else if (Entry.Level == Core.Log.EntryLevel.Information)
                _stringBuilder.Append("INFORMATION");
            else if (Entry.Level == Core.Log.EntryLevel.Debug)
                _stringBuilder.Append("DEBUG");
            else if (Entry.Level == Core.Log.EntryLevel.Trace)
                _stringBuilder.Append("TRACE");
            else if (Entry.Level == Core.Log.EntryLevel.Exception)
                _stringBuilder.Append("EXCEPTION");
            _stringBuilder.Append(" ");
            _stringBuilder.Append(Entry.MethodBase.DeclaringType.Namespace);
            _stringBuilder.Append(".");
            _stringBuilder.Append(Entry.MethodBase.DeclaringType.Name);
            _stringBuilder.Append(".");
            _stringBuilder.Append(Entry.MethodBase.Name);
            _stringBuilder.Append(" ");
            _stringBuilder.Append(Entry.Message);
            try
            {
                using (System.IO.StreamWriter _streamWriter = new System.IO.StreamWriter(this.FullPath(), true))
                {
                    _streamWriter.WriteLine(_stringBuilder.ToString());
                    _streamWriter.Close();
                }
            }
            catch { }
        }
        private void Exception(Core.Log.Entry Entry, System.Exception Exception)
        {
            Entry.Level = Core.Log.EntryLevel.Warning;
            Entry.DateTime = System.DateTime.Now;
            this.LogToFile(Entry, Exception);
            
        }
        public void Exception(System.Reflection.MethodBase MethodBase, System.Exception Exception)
        {
            Core.Log.Entry _entry = new Core.Log.Entry()
            {
                Level = Core.Log.EntryLevel.Trace,
                DateTime = System.DateTime.Now,
                Message = Exception.ToString(),
                MethodBase = new Core.Log.MethodBase()
                {
                    Name = MethodBase.Name,
                    DeclaringType = new Core.Log.DeclaringType()
                    {
                        Name = MethodBase.DeclaringType.Name,
                        Namespace = MethodBase.DeclaringType.Namespace
                    }
                }
            };
            this.Exception(_entry, Exception);
        }
        private void Fatal(Core.Log.Entry Entry)
        {
            if (this.Level >= Core.Log.EntryLevel.Fatal)
            {
                this.LogToFile(Entry);
                this.LogToSQLite(Entry);
            }
        }
        public void Fatal(System.Reflection.MethodBase MethodBase, String Message)
        {
            Core.Log.Entry _entry = new Core.Log.Entry()
            {
                Level = Core.Log.EntryLevel.Fatal,
                DateTime = System.DateTime.Now,
                Message = Message,
                MethodBase = new Core.Log.MethodBase() {
                    Name = MethodBase.Name,
                    DeclaringType = new Core.Log.DeclaringType()
                    {
                        Name= MethodBase.DeclaringType.Name,
                        Namespace = MethodBase.DeclaringType.Namespace
                    }
                }
            };
            this.Fatal(_entry);
        }
        private void Error(Core.Log.Entry Entry)
        {
            if (this.Level >= Core.Log.EntryLevel.Error)
            {
                this.LogToFile(Entry);
                this.LogToSQLite(Entry);
            }
        }
        public void Error(System.Reflection.MethodBase MethodBase, String Message)
        {
            Core.Log.Entry _entry = new Core.Log.Entry()
            {
                Level = Core.Log.EntryLevel.Error,
                DateTime = System.DateTime.Now,
                Message = Message,
                MethodBase = new Core.Log.MethodBase()
                {
                    Name = MethodBase.Name,
                    DeclaringType = new Core.Log.DeclaringType()
                    {
                        Name = MethodBase.DeclaringType.Name,
                        Namespace = MethodBase.DeclaringType.Namespace
                    }
                }
            };
            this.Error(_entry);
        }
        private void Warning(Core.Log.Entry Entry)
        {
            if (this.Level >= Core.Log.EntryLevel.Warning)
            {
                this.LogToFile(Entry);
                this.LogToSQLite(Entry);
            }
        }
        public void Warning(System.Reflection.MethodBase MethodBase, String Message)
        {
            Core.Log.Entry _entry = new Core.Log.Entry()
            {
                Level = Core.Log.EntryLevel.Warning,
                DateTime = System.DateTime.Now,
                Message = Message,
                MethodBase = new Core.Log.MethodBase()
                {
                    Name = MethodBase.Name,
                    DeclaringType = new Core.Log.DeclaringType()
                    {
                        Name = MethodBase.DeclaringType.Name,
                        Namespace = MethodBase.DeclaringType.Namespace
                    }
                }
            };
            this.Warning(_entry);
        }
        public void Warn(System.Reflection.MethodBase MethodBase, String Message)
        {
            this.Warning(MethodBase, Message);
        }
        private void Information(Core.Log.Entry Entry)
        {
            if (this.Level >= Core.Log.EntryLevel.Information)
            {
                this.LogToFile(Entry);
                this.LogToSQLite(Entry);
            }
        }
        public void Information(System.Reflection.MethodBase MethodBase, String Message)
        {
            Core.Log.Entry _entry = new Core.Log.Entry()
            {
                Level = Core.Log.EntryLevel.Information,
                DateTime = System.DateTime.Now,
                Message = Message,
                MethodBase = new Core.Log.MethodBase()
                {
                    Name = MethodBase.Name,
                    DeclaringType = new Core.Log.DeclaringType()
                    {
                        Name = MethodBase.DeclaringType.Name,
                        Namespace = MethodBase.DeclaringType.Namespace
                    }
                }
            };
            this.Information(_entry);
        }
        public void Info(System.Reflection.MethodBase MethodBase, String Message)
        {
            this.Information(MethodBase, Message);
        }
        private void Debug(Core.Log.Entry Entry)
        {
            if (this.Level >= Core.Log.EntryLevel.Debug)
            {
                this.LogToFile(Entry);
                this.LogToSQLite(Entry);
            }
        }
        public void Debug(System.Reflection.MethodBase MethodBase, String Message)
        {
            Core.Log.Entry _entry = new Core.Log.Entry()
            {
                Level = Core.Log.EntryLevel.Debug,
                DateTime = System.DateTime.Now,
                Message = Message,
                MethodBase = new Core.Log.MethodBase()
                {
                    Name = MethodBase.Name,
                    DeclaringType = new Core.Log.DeclaringType()
                    {
                        Name = MethodBase.DeclaringType.Name,
                        Namespace = MethodBase.DeclaringType.Namespace
                    }
                }
            };
            this.Debug(_entry);
        }
        private void Trace(Core.Log.Entry Entry)
        {
            if (this.Level >= Core.Log.EntryLevel.Trace)
            {
                this.LogToFile(Entry);
                this.LogToSQLite(Entry);
            }
        }
        public void Trace(System.Reflection.MethodBase MethodBase, String Message)
        {
            Core.Log.Entry _entry = new Core.Log.Entry()
            {
                Level = Core.Log.EntryLevel.Trace,
                DateTime = System.DateTime.Now,
                Message = Message,
                MethodBase = new Core.Log.MethodBase()
                {
                    Name = MethodBase.Name,
                    DeclaringType = new Core.Log.DeclaringType()
                    {
                        Name = MethodBase.DeclaringType.Name,
                        Namespace = MethodBase.DeclaringType.Namespace
                    }
                }
            };
            this.Trace(_entry);
        }
    }
}