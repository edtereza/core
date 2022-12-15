/*
*************************************************************************
**  Included in SQLite3 port to C#-SQLite;  2008 Noah B Hart
**  C#-SQLite is an independent reimplementation of the SQLite software library
**
**  SQLITE_SOURCE_ID: 2010-08-23 18:52:01 42537b60566f288167f1b5864a5435986838e3a3
**
*************************************************************************
*/
namespace Core.Database.SQLite
{
  internal partial class Sqlite3
  {
    /* Automatically generated.  Do not edit */
    /* See the mkopcodec.awk script for details. */
#if !SQLITE_OMIT_EXPLAIN || !NDEBUG || VDBE_PROFILE || SQLITE_DEBUG
    static string sqlite3OpcodeName( int i )
    {
      string[] azName =  { "?",
     /*   1 */ "Goto",
     /*   2 */ "Gosub",
     /*   3 */ "Return",
     /*   4 */ "Yield",
     /*   5 */ "HaltIfNull",
     /*   6 */ "Halt",
     /*   7 */ "Integer",
     /*   8 */ "Int64",
     /*   9 */ "String",
     /*  10 */ "Null",
     /*  11 */ "Blob",
     /*  12 */ "Variable",
     /*  13 */ "Move",
     /*  14 */ "Copy",
     /*  15 */ "SCopy",
     /*  16 */ "ResultRow",
     /*  17 */ "CollSeq",
     /*  18 */ "Function",
     /*  19 */ "Not",
     /*  20 */ "AddImm",
     /*  21 */ "MustBeInt",
     /*  22 */ "RealAffinity",
     /*  23 */ "Permutation",
     /*  24 */ "Compare",
     /*  25 */ "Jump",
     /*  26 */ "If",
     /*  27 */ "IfNot",
     /*  28 */ "Column",
     /*  29 */ "Affinity",
     /*  30 */ "MakeRecord",
     /*  31 */ "Count",
     /*  32 */ "Savepoint",
     /*  33 */ "AutoCommit",
     /*  34 */ "Transaction",
     /*  35 */ "ReadCookie",
     /*  36 */ "SetCookie",
     /*  37 */ "VerifyCookie",
     /*  38 */ "OpenRead",
     /*  39 */ "OpenWrite",
     /*  40 */ "OpenAutoindex",
     /*  41 */ "OpenEphemeral",
     /*  42 */ "OpenPseudo",
     /*  43 */ "Close",
     /*  44 */ "SeekLt",
     /*  45 */ "SeekLe",
     /*  46 */ "SeekGe",
     /*  47 */ "SeekGt",
     /*  48 */ "Seek",
     /*  49 */ "NotFound",
     /*  50 */ "Found",
     /*  51 */ "IsUnique",
     /*  52 */ "NotExists",
     /*  53 */ "Sequence",
     /*  54 */ "NewRowid",
     /*  55 */ "Insert",
     /*  56 */ "InsertInt",
     /*  57 */ "Delete",
     /*  58 */ "ResetCount",
     /*  59 */ "RowKey",
     /*  60 */ "RowData",
     /*  61 */ "Rowid",
     /*  62 */ "NullRow",
     /*  63 */ "Last",
     /*  64 */ "Sort",
     /*  65 */ "Rewind",
     /*  66 */ "Prev",
     /*  67 */ "Next",
     /*  68 */ "Or",
     /*  69 */ "And",
     /*  70 */ "IdxInsert",
     /*  71 */ "IdxDelete",
     /*  72 */ "IdxRowid",
     /*  73 */ "IsNull",
     /*  74 */ "NotNull",
     /*  75 */ "Ne",
     /*  76 */ "Eq",
     /*  77 */ "Gt",
     /*  78 */ "Le",
     /*  79 */ "Lt",
     /*  80 */ "Ge",
     /*  81 */ "IdxLT",
     /*  82 */ "BitAnd",
     /*  83 */ "BitOr",
     /*  84 */ "ShiftLeft",
     /*  85 */ "ShiftRight",
     /*  86 */ "Add",
     /*  87 */ "Subtract",
     /*  88 */ "Multiply",
     /*  89 */ "Divide",
     /*  90 */ "Remainder",
     /*  91 */ "Concat",
     /*  92 */ "IdxGE",
     /*  93 */ "BitNot",
     /*  94 */ "String8",
     /*  95 */ "Destroy",
     /*  96 */ "Clear",
     /*  97 */ "CreateIndex",
     /*  98 */ "CreateTable",
     /*  99 */ "ParseSchema",
     /* 100 */ "LoadAnalysis",
     /* 101 */ "DropTable",
     /* 102 */ "DropIndex",
     /* 103 */ "DropTrigger",
     /* 104 */ "IntegrityCk",
     /* 105 */ "RowSetAdd",
     /* 106 */ "RowSetRead",
     /* 107 */ "RowSetTest",
     /* 108 */ "Program",
     /* 109 */ "Param",
     /* 110 */ "FkCounter",
     /* 111 */ "FkIfZero",
     /* 112 */ "MemMax",
     /* 113 */ "IfPos",
     /* 114 */ "IfNeg",
     /* 115 */ "IfZero",
     /* 116 */ "AggStep",
     /* 117 */ "AggFinal",
     /* 118 */ "Checkpoint",
     /* 119 */ "JournalMode",
     /* 120 */ "Vacuum",
     /* 121 */ "IncrVacuum",
     /* 122 */ "Expire",
     /* 123 */ "TableLock",
     /* 124 */ "VBegin",
     /* 125 */ "VCreate",
     /* 126 */ "VDestroy",
     /* 127 */ "VOpen",
     /* 128 */ "VFilter",
     /* 129 */ "VColumn",
     /* 130 */ "Real",
     /* 131 */ "VNext",
     /* 132 */ "VRename",
     /* 133 */ "VUpdate",
     /* 134 */ "Pagecount",
     /* 135 */ "MaxPgcnt",
     /* 136 */ "Trace",
     /* 137 */ "Noop",
     /* 138 */ "Explain",
     /* 139 */ "NotUsed_139",
     /* 140 */ "NotUsed_140",
     /* 141 */ "ToText",
     /* 142 */ "ToBlob",
     /* 143 */ "ToNumeric",
     /* 144 */ "ToInt",
     /* 145 */ "ToReal",
};
      return azName[i];
    }
#endif
  }
}
