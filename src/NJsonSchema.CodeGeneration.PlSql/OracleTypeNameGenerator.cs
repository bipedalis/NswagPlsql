using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NJsonSchema.CodeGeneration.PlSql
{
    /// <summary>
    /// Oracle name generator
    /// </summary>
    public class OracleTypeNameGenerator : DefaultTypeNameGenerator
    {
        private static readonly string[] ReservedKeywords =
{
            "access", "account", "activate", "add", "admin", "advise", "after", "all", "all_rows", "allocate", "alter", "analyze", "and", "any", "archive", "archivelog", "array", "as", "asc", "at", "audit", "authenticated", "authorization", "autoextend", "automatic", "backup", "become", "before", "begin", "between", "bfile", "bitmap", "blob", "block", "body", "by", "cache", "cache_instances", "cancel", "cascade", "cast", "cfile", "chained", "change", "char", "char_cs", "character", "check", "checkpoint", "choose", "chunk", "clear", "clob", "clone", "close", "close_cached_open_cursors", "cluster", "coalesce", "column", "columns", "comment", "commit", "committed", "compatibility", "compile", "complete", "composite_limit", "compress", "compute", "connect", "connect_time", "constraint", "constraints", "contents", "continue", "controlfile", "convert", "cost", "cpu_per_call", "cpu_per_session", "create", "current", "current_schema", "curren_user", "cursor", "cycle", "dangling", "database", "datafile", "datafiles", "dataobjno", "date", "dba", "dbhigh", "dblow", "dbmac", "deallocate", "debug", "dec", "decimal", "declare", "default", "deferrable", "deferred", "degree", "delete", "deref", "desc", "directory", "disable", "disconnect", "dismount", "distinct", "distributed", "dml", "double", "drop", "dump", "each", "else", "enable", "end", "enforce", "entry", "error", "escape", "except", "exceptions", "exchange", "excluding", "exclusive", "execute", "exists", "expire", "explain", "extent", "extents", "externally", "failed_login_attempts", "false", "fast", "file", "first_rows", "flagger", "float", "flob", "flush", "for", "force", "foreign", "freelist", "freelists", "from", "full", "function", "global", "globally", "global_name", "grant", "group", "groups", "hash", "hashkeys", "having", "header", "heap", "identified", "idgenerators", "idle_time", "if", "immediate", "in", "including", "increment", "index", "indexed", "indexes", "indicator", "ind_partition", "initial", "initially", "initrans", "insert", "instance", "instances", "instead", "int", "integer", "intermediate", "intersect", "into", "is", "isolation", "isolation_level", "keep", "key", "kill", "label", "layer", "less", "level", "library", "like", "limit", "link", "list", "lob", "local", "lock", "locked", "log", "logfile", "logging", "logical_reads_per_call", "logical_reads_per_session", "long", "manage", "master", "max", "maxarchlogs", "maxdatafiles", "maxextents", "maxinstances", "maxlogfiles", "maxloghistory", "maxlogmembers", "maxsize", "maxtrans", "maxvalue", "min", "member", "minimum", "minextents", "minus", "minvalue", "mlslabel", "mls_label_format", "mode", "modify", "mount", "move", "mts_dispatchers", "multiset", "national", "nchar", "nchar_cs", "nclob", "needed", "nested", "network", "new", "next", "noarchivelog", "noaudit", "nocache", "nocompress", "nocycle", "noforce", "nologging", "nomaxvalue", "nominvalue", "none", "noorder", "nooverride", "noparallel", "noparallel", "noreverse", "normal", "nosort", "not", "nothing", "nowait", "null", "number", "numeric", "nvarchar2", "object", "objno", "objno_reuse", "of", "off", "offline", "oid", "oidindex", "old", "on", "online", "only", "opcode", "open", "optimal", "optimizer_goal", "option", "or", "order", "organization", "oslabel", "overflow", "own", "package", "parallel", "partition", "password", "password_grace_time", "password_life_time", "password_lock_time", "password_reuse_max", "password_reuse_time", "password_verify_function", "pctfree", "pctincrease", "pctthreshold", "pctused", "pctversion", "percent", "permanent", "plan", "plsql_debug", "post_transaction", "precision", "preserve", "primary", "prior", "private", "private_sga", "privilege", "privileges", "procedure", "profile", "public", "purge", "queue", "quota", "range", "raw", "rba", "read", "readup", "real", "rebuild", "recover", "recoverable", "recovery", "ref", "references", "referencing", "refresh", "rename", "replace", "reset", "resetlogs", "resize", "resource", "restricted", "return", "returning", "reuse", "reverse", "revoke", "role", "roles", "rollback", "row", "rowid", "rownum", "rows", "rule", "sample", "savepoint", "sb4", "scan_instances", "schema", "scn", "scope", "sd_all", "sd_inhibit", "sd_show", "segment", "seg_block", "seg_file", "select", "sequence", "serializable", "session", "session_cached_cursors", "sessions_per_user", "set", "share", "shared", "shared_pool", "shrink", "size", "skip", "skip_unusable_indexes", "smallint", "snapshot", "some", "sort", "specification", "split", "sql_trace", "standby", "start", "statement_id", "statistics", "stop", "storage", "store", "structure", "successful", "switch", "sys_op_enforce_not_null$", "sys_op_ntcimg$", "synonym", "sysdate", "sysdba", "sysoper", "system", "table", "tables", "tablespace", "tablespace_no", "tabno", "temporary", "than", "the", "then", "thread", "timestamp", "time", "to", "toplevel", "trace", "tracing", "transaction", "transitional", "trigger", "triggers", "true", "truncate", "tx", "type", "ub2", "uba", "uid", "unarchived", "undo", "union", "unique", "unlimited", "unlock", "unrecoverable", "until", "unusable", "unused", "updatable", "update", "usage", "use", "user", "using", "validate", "validation", "value", "values", "varchar", "varchar2", "varying", "view", "when", "whenever", "where", "with", "without", "work", "write", "writedown", "writeup", "xid", "year", "zone", "case"
        };

        /// <summary>Generates the type name for the given schema.</summary>
        /// <param name="schema">The schema.</param>
        /// <param name="typeNameHint">The type name hint.</param>
        /// <returns>The type name.</returns>
        protected override string Generate(JsonSchema schema, string typeNameHint)
        {
            string name = base.Generate(schema, typeNameHint);
            name = name.Replace('`', '_').Replace('+','_');
            if (name.Length>=29)
            {
                name = name.Substring(0,20) + name.GetHashCode().ToString("X");
            }
            else if (ReservedKeywords.Contains(name.ToLower()))
            {
                name = name  +name.GetHashCode().ToString("X");
            }
            System.Diagnostics.Debug.Assert(name.Length < 30);
            return name;
        }
        /// <summary>Generates the type name for the given schema respecting the reserved type names.</summary>
        /// <param name="schema">The schema.</param>
        /// <param name="typeNameHint">The type name hint.</param>
        /// <param name="reservedTypeNames">The reserved type names.</param>
        /// <returns>The type name.</returns>
        public override string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
        {
            if (string.IsNullOrEmpty(typeNameHint) && !string.IsNullOrEmpty(schema.DocumentPath))
            {
                typeNameHint = schema.DocumentPath.Replace("\\", "/").Split('/').Last();
            }

            typeNameHint = (typeNameHint ?? "")
                .Replace("[", " Of ")
                .Replace("]", " ")
                .Replace("<", " Of ")
                .Replace(">", " ")
                .Replace(",", " And ")
                .Replace("  ", " ");

            var parts = typeNameHint.Split(' ');
            typeNameHint = string.Join(string.Empty, parts.Select(p => Generate(schema, p)));

            var typeName = Generate(schema, typeNameHint);
            if (string.IsNullOrEmpty(typeName) || reservedTypeNames.Contains(typeName))
            {
                //typeName = null; 
            }

            return typeName;
        }
    }
}
