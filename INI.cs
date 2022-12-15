namespace Core
{
    public class INI : System.IDisposable
    {
        [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        static extern long WritePrivateProfileString(System.String Section, System.String Key, System.String Value, System.String FilePath);

        [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        static extern int GetPrivateProfileString(System.String Section, System.String Key, System.String Default, System.Text.StringBuilder RetVal, int Size, System.String FilePath);

        private System.String _fileName = null;
        private System.String _filePath = null;
        private System.String _fullPath = null;

        public INI() : this(System.Reflection.Assembly.GetEntryAssembly().GetName().Name, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)) { }

        public INI(System.String Name) : this(Name, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)) { }

        public INI(System.String Name, System.String Path)
        {
            this._fileName = Name;
            this._filePath = Path;
            this._filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (!this._filePath.EndsWith(@"\"))
                this._filePath = this._filePath + @"\";
            if (!this._fileName.EndsWith(@".ini"))
                this._fileName = this._fileName + @".ini";
            this._fullPath = this._filePath + this._fileName;
        }

        public System.String String(System.String Session, System.String Key, System.String Default = null)
        {
            System.Text.StringBuilder _StringBuilder = new System.Text.StringBuilder(255);
            try
            {
                GetPrivateProfileString(Session, Key, Default, _StringBuilder, 255, this._fullPath);
            }
            catch { }
            return ((_StringBuilder.ToString() == "") ? Default : _StringBuilder.ToString());
        }
        public System.Boolean Boolean(System.String Session, System.String Key, System.Boolean Default = false)
        {
            System.String _String = this.String(Session, Key, null);
            if (_String == "True" || _String == "true" || _String == "T" || _String == "1")
                return true;
            else if (_String == "False" || _String == "false" || _String == "F" || _String == "0")
                return false;
            return Default;
        }
        public System.Int32 Int32(System.String Session, System.String Key, System.Int32 Default = 0)
        {
            System.String _String = this.String(Session, Key, null);
            System.Int32 _Int32 = Default;
            if (_String != null)
            {
                System.Int32.TryParse(_String, out _Int32);
            }
            return _Int32;
        }
        public System.Int64 Int64(System.String Session, System.String Key, System.Int64 Default = 0)
        {
            System.String _String = this.String(Session, Key, null);
            System.Int64 _Int64 = Default;
            if (_String != null)
            {
                System.Int64.TryParse(_String, out _Int64);
            }
            return _Int64;
        }
        public System.Double Double(System.String Session, System.String Key, System.Double Default = 0)
        {
            System.String _String = this.String(Session, Key, null);
            System.Double _Double = Default;
            if (_String != null)
            {
                System.Double.TryParse(_String, out _Double);
            }
            return _Double;
        }
        public System.Decimal Decimal(System.String Session, System.String Key, System.Decimal Default = 0)
        {
            System.String _String = this.String(Session, Key, null);
            System.Decimal _Decimal = Default;
            if (_String != null)
            {
                System.Decimal.TryParse(_String, out _Decimal);
            }
            return _Decimal;
        }
        public void Write(System.String Session, System.String Key, System.String Value)
        {
            try
            {
                WritePrivateProfileString(Session, Key, Value, this._fullPath);
            }
            catch { }
        }
        public void Write(System.String Session, System.String Key, System.Int32 Value)
        {
            this.Write(Session, Key, Value.ToString());
        }
        public void Write(System.String Session, System.String Key, System.Int64 Value)
        {
            this.Write(Session, Key, Value.ToString());
        }
        public void Write(System.String Session, System.String Key, System.Decimal Value)
        {
            this.Write(Session, Key, Value.ToString());
        }
        public void Write(System.String Session, System.String Key, System.Double Value)
        {
            this.Write(Session, Key, Value.ToString());
        }
        public void Delete(System.String Session, System.String Key)
        {
            try
            {
                WritePrivateProfileString(Session, Key, null, this._fullPath);
            }
            catch { }
        }
        public System.Boolean Exists(System.String Session, System.String Key)
        {
            System.String _String = this.String(Session, Key, null);
            return (_String == null ? false : true);
        }

        public void Dispose()
        {
            
        }       
    }   
}