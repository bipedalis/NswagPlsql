//-----------------------------------------------------------------------
// <copyright file="PlSqlOperationModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.PlSql;
using NSwag.CodeGeneration.Models;

namespace NSwag.CodeGeneration.PlSql.Models
{
    /// <summary>The PlSql operation model.</summary>
    public class PlSqlOperationModel : OperationModelBase<PlSqlParameterModel, PlSqlResponseModel>
    {
        private static readonly string[] ReservedKeywords =
        {
            "access", "account", "activate", "add", "admin", "advise", "after", "all", "all_rows", "allocate", "alter", "analyze", "and", "any", "archive", "archivelog", "array", "as", "asc", "at", "audit", "authenticated", "authorization", "autoextend", "automatic", "backup", "become", "before", "begin", "between", "bfile", "bitmap", "blob", "block", "body", "by", "cache", "cache_instances", "cancel", "cascade", "cast", "cfile", "chained", "change", "char", "char_cs", "character", "check", "checkpoint", "choose", "chunk", "clear", "clob", "clone", "close", "close_cached_open_cursors", "cluster", "coalesce", "column", "columns", "comment", "commit", "committed", "compatibility", "compile", "complete", "composite_limit", "compress", "compute", "connect", "connect_time", "constraint", "constraints", "contents", "continue", "controlfile", "convert", "cost", "cpu_per_call", "cpu_per_session", "create", "current", "current_schema", "curren_user", "cursor", "cycle", "dangling", "database", "datafile", "datafiles", "dataobjno", "date", "dba", "dbhigh", "dblow", "dbmac", "deallocate", "debug", "dec", "decimal", "declare", "default", "deferrable", "deferred", "degree", "delete", "deref", "desc", "directory", "disable", "disconnect", "dismount", "distinct", "distributed", "dml", "double", "drop", "dump", "each", "else", "enable", "end", "enforce", "entry", "error", "escape", "except", "exceptions", "exchange", "excluding", "exclusive", "execute", "exists", "expire", "explain", "extent", "extents", "externally", "failed_login_attempts", "false", "fast", "file", "first_rows", "flagger", "float", "flob", "flush", "for", "force", "foreign", "freelist", "freelists", "from", "full", "function", "global", "globally", "global_name", "grant", "group", "groups", "hash", "hashkeys", "having", "header", "heap", "identified", "idgenerators", "idle_time", "if", "immediate", "in", "including", "increment", "index", "indexed", "indexes", "indicator", "ind_partition", "initial", "initially", "initrans", "insert", "instance", "instances", "instead", "int", "integer", "intermediate", "intersect", "into", "is", "isolation", "isolation_level", "keep", "key", "kill", "label", "layer", "less", "level", "library", "like", "limit", "link", "list", "lob", "local", "lock", "locked", "log", "logfile", "logging", "logical_reads_per_call", "logical_reads_per_session", "long", "manage", "master", "max", "maxarchlogs", "maxdatafiles", "maxextents", "maxinstances", "maxlogfiles", "maxloghistory", "maxlogmembers", "maxsize", "maxtrans", "maxvalue", "min", "member", "minimum", "minextents", "minus", "minvalue", "mlslabel", "mls_label_format", "mode", "modify", "mount", "move", "mts_dispatchers", "multiset", "national", "nchar", "nchar_cs", "nclob", "needed", "nested", "network", "new", "next", "noarchivelog", "noaudit", "nocache", "nocompress", "nocycle", "noforce", "nologging", "nomaxvalue", "nominvalue", "none", "noorder", "nooverride", "noparallel", "noparallel", "noreverse", "normal", "nosort", "not", "nothing", "nowait", "null", "number", "numeric", "nvarchar2", "object", "objno", "objno_reuse", "of", "off", "offline", "oid", "oidindex", "old", "on", "online", "only", "opcode", "open", "optimal", "optimizer_goal", "option", "or", "order", "organization", "oslabel", "overflow", "own", "package", "parallel", "partition", "password", "password_grace_time", "password_life_time", "password_lock_time", "password_reuse_max", "password_reuse_time", "password_verify_function", "pctfree", "pctincrease", "pctthreshold", "pctused", "pctversion", "percent", "permanent", "plan", "plsql_debug", "post_transaction", "precision", "preserve", "primary", "prior", "private", "private_sga", "privilege", "privileges", "procedure", "profile", "public", "purge", "queue", "quota", "range", "raw", "rba", "read", "readup", "real", "rebuild", "recover", "recoverable", "recovery", "ref", "references", "referencing", "refresh", "rename", "replace", "reset", "resetlogs", "resize", "resource", "restricted", "return", "returning", "reuse", "reverse", "revoke", "role", "roles", "rollback", "row", "rowid", "rownum", "rows", "rule", "sample", "savepoint", "sb4", "scan_instances", "schema", "scn", "scope", "sd_all", "sd_inhibit", "sd_show", "segment", "seg_block", "seg_file", "select", "sequence", "serializable", "session", "session_cached_cursors", "sessions_per_user", "set", "share", "shared", "shared_pool", "shrink", "size", "skip", "skip_unusable_indexes", "smallint", "snapshot", "some", "sort", "specification", "split", "sql_trace", "standby", "start", "statement_id", "statistics", "stop", "storage", "store", "structure", "successful", "switch", "sys_op_enforce_not_null$", "sys_op_ntcimg$", "synonym", "sysdate", "sysdba", "sysoper", "system", "table", "tables", "tablespace", "tablespace_no", "tabno", "temporary", "than", "the", "then", "thread", "timestamp", "time", "to", "toplevel", "trace", "tracing", "transaction", "transitional", "trigger", "triggers", "true", "truncate", "tx", "type", "ub2", "uba", "uid", "unarchived", "undo", "union", "unique", "unlimited", "unlock", "unrecoverable", "until", "unusable", "unused", "updatable", "update", "usage", "use", "user", "using", "validate", "validation", "value", "values", "varchar", "varchar2", "varying", "view", "when", "whenever", "where", "with", "without", "work", "write", "writedown", "writeup", "xid", "year", "zone", "case"
        };

        private readonly PlSqlGeneratorBaseSettings _settings;
        private readonly OpenApiOperation _operation;
        private readonly PlSqlGeneratorBase _generator;
        private readonly PlSqlTypeResolver _resolver;

        /// <summary>Initializes a new instance of the <see cref="PlSqlOperationModel" /> class.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        public PlSqlOperationModel(
            OpenApiOperation operation,
            PlSqlGeneratorBaseSettings settings,
            PlSqlGeneratorBase generator,
            PlSqlTypeResolver resolver)
            : base(resolver.ExceptionSchema, operation, resolver, generator, settings)
        {
            _settings = settings;
            _operation = operation;
            _generator = generator;
            _resolver = resolver;

            var parameters = GetActualParameters();

            if (settings.GenerateOptionalParameters)
            {
                    parameters = parameters
                        .OrderBy(p => p.Position ?? 0)
                        .OrderBy(p => !p.IsRequired)
                        .ToList();
            }

            Parameters = parameters
                .Select(parameter =>
                    new PlSqlParameterModel(parameter.Name, GetParameterVariableName(parameter, _operation.Parameters),
                        ResolveParameterType(parameter), parameter, parameters,
                        _settings.CodeGeneratorSettings,
                        _generator,
                        _resolver))
                .ToList();
        }

        /// <summary>Gets the method's access modifier.</summary>
        public string MethodAccessModifier
        {
            get
            {
                var controllerName = _settings.GenerateControllerName(ControllerName);
                var settings = _settings as PlSqlClientGeneratorSettings;
                if (settings != null && settings.ProtectedMethods?.Contains(controllerName + "." + ConversionUtilities.ConvertToUpperCamelCase(OperationName, false) + "Async") == true)
                {
                    return "protected";
                }

                return "public";
            }
        }

        /// <summary>Gets the actual name of the operation (language specific).</summary>
        public override string ActualOperationName
        {
            get
            {
                var name = ConversionUtilities.ConvertToUpperCamelCase(OperationName, true);
                if (ReservedKeywords.Contains(name.ToLower()))
                {
                    name = name + "_" + HttpMethod;
                }
                if (this._resolver.Types.Any(p => p.Value.ToLower() == name.ToLower()))
                {
                    name = name + "_" + HttpMethod;
                }
                if (name.Length >= 30)
                {
                    int postfixLength = 0;
                    string postfix;
                    if (name.EndsWith("Get") || name.EndsWith("Put"))
                        postfixLength = 3;
                    else if(name.EndsWith("Post"))
                        postfixLength = 4;
                    postfix = name.Substring(name.Length - postfixLength, postfixLength);
                    name = name.Substring(0,  29 -postfixLength) + postfix;                    
                }
                return name;
            }
        }
        /// <summary>Gets a value indicating whether this operation is rendered as interface method.</summary>
        public bool IsInterfaceMethod => MethodAccessModifier == "public";

        /// <summary>Gets a value indicating whether the operation has a result type.</summary>
        public bool HasResult => UnwrappedResultType != "void";

        /// <summary>
        /// The default value of the result type, i.e. default(T) or default(T)! depending on whether NRT are enabled.
        /// </summary>
        public string UnwrappedResultDefaultValue => $"default({UnwrappedResultType}){((_settings as PlSqlClientGeneratorSettings)?.PlSqlGeneratorSettings.GenerateNullableReferenceTypes == true ? "!" : "")}";

        /// <summary>Gets or sets the synchronous type of the result.</summary>
        public string SyncResultType
        {
            get
            {
                if (_settings != null && WrapResponse && UnwrappedResultType != "FileResponse")
                {
                    return UnwrappedResultType == "void"
                        ? _settings.ResponseClass.Replace("{controller}", ControllerName)
                        : _settings.ResponseClass.Replace("{controller}", ControllerName) + "<" + UnwrappedResultType + ">";
                }

                return UnwrappedResultType;
            }
        }

        /// <summary>Gets or sets the type of the result.</summary>
        public override string ResultType
        {
            get
            {
                string rt = SyncResultType;
                if (rt.StartsWith("VARCHAR2"))
                    rt = "VARCHAR2";
                if (_settings.PlSqlGeneratorSettings.ExcludedTypeNames.Contains(rt))
                {
                    return "nclob";
                }
                return rt;
            }
        }
        /// <summary>
        /// Too deep for Oracle
        /// </summary>
        public bool IsVeryComplex
        {
            get
            {
                return ResultType == "PagedResult_1OfCBCD44323C306" 
                    || ResultType== "PagedResult_1OfCBCD4540151E"
                    || ResultType == "PagedResult_1OfCBCD416DD7BC9"
                    || ResultType == "PagedResult_1OfCBCD48925D887"
                    || ResultType == "PagedResult_1OfCBCD490E014CF"
                    || ResultType == "PagedResult_1OfCBCD4EBC5F44E"
                 //   || ResultType == "PagedResult_1OfCBCD46A763897" // proc neatrod
                    ;
            }
        }
        /// <summary>Gets the type of the unwrapped result type (without Task).</summary>
        public string ItemsResultType
        {
            get
            {
                var response = GetSuccessResponse();
                JsonSchemaProperty items;
                if (response.Value.Schema.ActualTypeSchema.ActualProperties.TryGetValue("Items", out items))
                {
                    return   _generator.GetTypeName(items.ActualSchema, false, !response.Value.Schema.HasTypeNameTitle ? "Response" : null);
                }
                return null;
            }
        }
        /// <summary>Gets or sets the type of the exception.</summary>
        public override string ExceptionType
        {
            get
            {
                if (_operation.ActualResponses.Count(r => !HttpUtilities.IsSuccessStatusCode(r.Key)) != 1)
                {
                    return "System.Exception";
                }

                var response = _operation.ActualResponses.Single(r => !HttpUtilities.IsSuccessStatusCode(r.Key));
                var isNullable = response.Value.IsNullable(_settings.CodeGeneratorSettings.SchemaType);
                return _generator.GetTypeName(response.Value.Schema, isNullable, "Exception");
            }
        }

        /// <summary>Gets or sets the exception descriptions.</summary>
        public IEnumerable<PlSqlExceptionDescriptionModel> ExceptionDescriptions
        {
            get
            {
                var settings = (PlSqlClientGeneratorSettings)_settings;
                var controllerName = _settings.GenerateControllerName(ControllerName);
                return Responses
                    .Where(r => r.ThrowsException)
                    .SelectMany(r =>
                    {
                        if (r.ExpectedSchemas?.Any() == true)
                        {
                            return r.ExpectedSchemas
                                .Where(s => s.Schema.ActualSchema?.InheritsSchema(_resolver.ExceptionSchema) == true)
                                .Select(s =>
                                {
                                    var schema = s.Schema;
                                    var isNullable = schema.IsNullable(_settings.PlSqlGeneratorSettings.SchemaType);
                                    var typeName = _generator.GetTypeName(schema.ActualSchema, isNullable, "Response");
                                    return new PlSqlExceptionDescriptionModel(typeName, s.Description, controllerName, settings);
                                });
                        }
                        else if (r.InheritsExceptionSchema)
                        {
                            return new[]
                            {
                                new PlSqlExceptionDescriptionModel(r.Type, r.ExceptionDescription, controllerName, settings)
                            };
                        }
                        else
                        {
                            return new PlSqlExceptionDescriptionModel[] { };
                        }
                    });
            }
        }

        /// <summary>Gets a value indicating whether a route name is available.</summary>
        public bool HasRouteName => RouteName != null;

        /// <summary>Gets a value indicating whether operation has parameters.</summary>
        public bool HasParams => Parameters.Count>0;

        /// <summary>Gets the route name for this operation.</summary>
        public string RouteName
        {
            get
            {


                return null;
            }
        }

        /// <summary>True if the operation has any security schemes</summary>
        public bool RequiresAuthentication => (_operation.ActualSecurity?.Count() ?? 0) != 0;

        /// <summary>Gets the security schemas that apply to this operation</summary>
        public IEnumerable<OpenApiSecurityRequirement> Security => _operation.ActualSecurity;

        /// <summary>Gets the name of the parameter variable.</summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="allParameters">All parameters.</param>
        /// <returns>The parameter variable name.</returns>
        protected override string GetParameterVariableName(OpenApiParameter parameter, IEnumerable<OpenApiParameter> allParameters)
        {
            var name = base.GetParameterVariableName(parameter, allParameters);
            if (ReservedKeywords.Contains(name))
            {
                name = "pi_" + name; // "\"" + name + "\"";
            }
            if (name.Length >= 28)
            {
                name = name.Substring(0, 19) + name.GetHashCode().ToString("X");
            }
            //if (name.Length > 30)
            //    name = name.Substring(0, 30);
            return name;
        }

        /// <summary>Resolves the type of the parameter.</summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The parameter type name.</returns>
        protected override string ResolveParameterType(OpenApiParameter parameter)
        {
            var schema = parameter.ActualSchema;

            if (parameter.IsBinaryBodyParameter)
            {
                    return parameter.HasBinaryBodyWithMultipleMimeTypes ? "FileParameter" : "System.IO.Stream";
            }

            if (schema.Type == JsonObjectType.Array && schema.Item.IsBinary)
            {
                return "System.Collections.Generic.IEnumerable<FileParameter>";
            }

            if (schema.IsBinary)
            {
                if (parameter.CollectionFormat == OpenApiParameterCollectionFormat.Multi && !schema.Type.HasFlag(JsonObjectType.Array))
                {
                    return "System.Collections.Generic.IEnumerable<FileParameter>";
                }

                return "FileParameter";
            }
            var baseType= base.ResolveParameterType(parameter);
            if (_settings.PlSqlGeneratorSettings.ExcludedTypeNames.Contains(baseType) ||
                baseType.EndsWith("T") &&
                _settings.PlSqlGeneratorSettings.ExcludedTypeNames.Contains(baseType.TrimEnd('T')))
            {
                return "nclob";
            }
            return baseType; // base.ResolveParameterType(parameter);
                //.Replace(_settings.PlSqlGeneratorSettings.ArrayType + "<", _settings.ParameterArrayType + "<")
                //.Replace(_settings.PlSqlGeneratorSettings.DictionaryType + "<", _settings.ParameterDictionaryType + "<");
        }

        /// <summary>Creates the response model.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="response">The response.</param>
        /// <param name="exceptionSchema">The exception schema.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        protected override PlSqlResponseModel CreateResponseModel(OpenApiOperation operation, string statusCode, OpenApiResponse response, JsonSchema exceptionSchema, IClientGenerator generator, TypeResolverBase resolver, ClientGeneratorBaseSettings settings)
        {
            return new PlSqlResponseModel(this, operation, statusCode, response, response == GetSuccessResponse().Value, exceptionSchema, generator, resolver, settings.CodeGeneratorSettings);
        }
    }
}
