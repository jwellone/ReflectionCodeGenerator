using System;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;

#nullable enable

namespace jwelloneEditor
{
    public static class TypeToTextConvert
    {
        public static string MakeClassName(Type target)
        {
            return $"{target.Name}Reflection";
        }

        public static string GetCodeText(Type target, IEnumerable<FieldInfo> fieldInfos, IEnumerable<MethodInfo> methodInfos)
        {
            var sb = new StringBuilder();

            sb.AppendLine("// =================================================");
            sb.AppendLine("// This is an automatically generated file.");
            sb.AppendLine("// Unable to edit.");
            sb.AppendLine("// Create by class TypeToTextConvert.");
            sb.AppendLine("// =================================================");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine();

            sb.AppendLine("#nullable enable");
            sb.AppendLine();

            var headerText = sb.ToString();

            sb.Clear();
            sb.Append("public static partial class ").AppendLine(MakeClassName(target));
            sb.AppendLine("{");
            sb.AppendLine("\tstatic Type? _cacheType;");
            sb.Append("\tstatic Type? _type => _cacheType ??= Type.GetType(\"").Append(target.AssemblyQualifiedName).AppendLine("\");");

            var fieldText = GetFieldText(fieldInfos);
            if (!string.IsNullOrEmpty(fieldText))
            {
                sb.AppendLine();
                sb.Append(fieldText);
            }

            var methodText = GetMethodText(methodInfos);
            if (!string.IsNullOrEmpty(methodText))
            {
                sb.AppendLine();
                sb.Append(methodText);
            }

            sb.AppendLine("}");

            var body = sb.ToString();
            sb.Clear();

            sb.Append(headerText);

            if (!string.IsNullOrEmpty(target.Namespace))
            {
                sb.Append("namespace ").AppendLine(target.Namespace);
                sb.AppendLine("{");

                var rows = body.Replace("\r\n", "\n").Split('\n');
                for (var i = 0; i < rows.Length - 1; ++i)
                {
                    sb.Append("\t").Append(rows[i]);
                    if (rows.Length > 1)
                    {
                        sb.AppendLine();
                    }
                }

                sb.Append("}");
            }
            else
            {
                sb.Append(body);
            }

            return sb.ToString();
        }



        public static string GetFieldText(Type target, BindingFlags bindingFlags)
        {
            return GetFieldText(target.GetFields(bindingFlags));
        }

        public static string GetFieldText(IEnumerable<FieldInfo> fieldInfos)
        {
            var sb = new StringBuilder();

            foreach (var field in fieldInfos)
            {
                if (field.IsDefined(typeof(CompilerGeneratedAttribute), false))
                {
                    continue;
                }

                var bindingFlags = field.IsStatic ? BindingFlags.NonPublic | BindingFlags.Static : BindingFlags.NonPublic | BindingFlags.Instance;
                var typeName = TypeNameConvert.Convert(field.FieldType);
                var bindingFlagsText = BindingFlagsText(bindingFlags);

                sb.Append("\tpublic static void Set").Append(field.Name).Append("(");

                if (!field.IsStatic)
                {
                    sb.Append("object self, ");
                }

                sb.Append(typeName).AppendLine(" value)");
                sb.AppendLine("\t{");
                sb.Append("\t\tvar info = jwellone.ReflectionUtil.GetFieldInfo(_type, \"").Append(field.Name).Append("\", ").Append(bindingFlagsText).AppendLine(");");
                sb.Append("\t\tinfo?.SetValue(");
                sb.Append(field.IsStatic ? "null" : "self");
                sb.AppendLine(", value);");
                sb.AppendLine("\t}");

                sb.Append("\tpublic static ").Append(typeName).Append(" Get").Append(field.Name).Append("(");

                if (!field.IsStatic)
                {
                    sb.Append("object self");
                }

                sb.AppendLine(")");
                sb.AppendLine("\t{");
                sb.Append("\t\tvar info = jwellone.ReflectionUtil.GetFieldInfo(_type, \"").Append(field.Name).Append("\", ").Append(bindingFlagsText).AppendLine(");");
                sb.AppendLine("\t\tif(info == null)");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\t return default!;");
                sb.AppendLine("\t\t}");
                sb.Append("\t\treturn (").Append(typeName).Append(")info!.GetValue(");
                sb.Append(field.IsStatic ? "null" : "self");
                sb.AppendLine(");");
                sb.AppendLine("\t}");
            }

            return sb.ToString();
        }

        public static string GetMethodText(Type target, BindingFlags bindingFlags)
        {
            return GetMethodText(target.GetMethods(bindingFlags));
        }

        public static string GetMethodText(IEnumerable<MethodInfo> methodInfos)
        {
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            foreach (var method in methodInfos)
            {
                var isMember = !method.IsStatic;
                var bindingFlags = method.IsStatic ? BindingFlags.NonPublic | BindingFlags.Static : BindingFlags.NonPublic | BindingFlags.Instance;
                var returnTypeName = TypeNameConvert.Convert(method.ReturnType);
                sb.Append("\tpublic static ").Append(returnTypeName).Append(" ").Append(method.Name).Append("(");

                if (isMember)
                {
                    sb.Append("object self");
                }

                var parameters = method.GetParameters();
                var parameterTypeTexts = new string[parameters.Length];
                var parameterStrings = new string[parameters.Length];
                if (parameters.Length > 0)
                {
                    if (isMember)
                    {
                        sb.Append(", ");
                    }

                    for (var i = 0; i < parameters.Length; ++i)
                    {
                        sb2.Clear();

                        var parameter = parameters[i];
                        if (parameter.ParameterType.IsByRef)
                        {
                            if (parameter.IsOut)
                            {
                                sb2.Append("out ");
                            }
                            else
                            {
                                sb2.Append("ref ");
                            }
                        }

                        parameterTypeTexts[i] = TypeNameConvert.Convert(parameter.ParameterType);
                        sb2.Append(parameterTypeTexts[i]);
                        sb2.Append(" ");
                        sb2.Append(parameter.Name);
                        parameterStrings[i] = sb2.ToString();
                    }

                    sb.Append(string.Join(", ", parameterStrings));
                }

                sb.AppendLine(")");
                sb.AppendLine("\t{");

                var bindingFlagsText = BindingFlagsText(bindingFlags);

                if (isMember)
                {
                    sb.Append("\t\tvar methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, ").Append("\"").Append(method.Name).Append("\", ").Append(bindingFlagsText).Append(", ");
                    sb.Append("new Type[").Append(parameterStrings.Length).Append("] {");
                    for (var i = 0; i < parameterTypeTexts.Length; ++i)
                    {
                        if (i > 0)
                        {
                            sb.Append(", ");
                        }
                        sb.Append(" typeof(").Append(parameterTypeTexts[i]).Append(")");
                    }
                    sb.AppendLine(" });");
                }
                else
                {
                    sb.Append("\t\tvar methodInfo = jwellone.ReflectionUtil.GetMethodInfo(_type, ").Append("\"").Append(method.Name).Append("\", ").Append(bindingFlagsText).AppendLine(");");
                }

                var isReturnTypeVoid = method.ReturnType == typeof(void);

                sb.AppendLine("\t\tif (methodInfo == null)");
                sb.AppendLine("\t\t{");

                for (var i = 0; i < parameters.Length; ++i)
                {
                    var parameter = parameters[i];
                    if (!parameter.ParameterType.IsByRef)
                    {
                        continue;
                    }

                    sb.Append("\t\t\t");
                    sb.Append(parameter.Name).AppendLine(" = default!;");
                }

                sb.Append("\t\t\treturn").AppendLine(isReturnTypeVoid ? ";" : " default!;");
                sb.AppendLine("\t\t}");

                sb.Append("\t\tvar p = new object[] {");
                for (var i = 0; i < parameters.Length; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append(",");
                    }

                    var parameterName = parameters[i].IsOut ? "null!" : parameters[i].Name;
                    sb.Append(" ").Append(parameterName);
                }

                sb.AppendLine(" };");

                if (isReturnTypeVoid)
                {
                    sb.Append("\t\tmethodInfo!.Invoke(");
                    sb.Append(isMember ? "self" : "null");
                    sb.AppendLine(", p);");
                }
                else
                {
                    sb.Append("\t\tvar returnValue = (").Append(returnTypeName).Append(")methodInfo!.Invoke(");
                    sb.Append(isMember ? "self" : "null");
                    sb.AppendLine(", p);");
                }

                for (var i = 0; i < parameters.Length; ++i)
                {
                    var parameter = parameters[i];
                    if (!parameter.ParameterType.IsByRef)
                    {
                        continue;
                    }

                    sb.Append("\t\t");
                    sb.Append(parameter.Name).Append(" = (");
                    sb.Append(TypeNameConvert.Convert(parameter.ParameterType)).Append(")");
                    sb.Append("p[").Append(i.ToString()).AppendLine("];");
                }

                if (!isReturnTypeVoid)
                {
                    sb.AppendLine("\t\treturn returnValue;");
                }

                sb.AppendLine("\t}");
            }

            return sb.ToString();
        }

        public static string MthodInfoToFullName(MethodInfo info)
        {
            return TypeNameConvert.Convert(info.ReturnParameter.ParameterType) + " " + info.Name + "(" + string.Join(",", info.GetParameters().Select(p => TypeNameConvert.Convert(p.ParameterType) + " " + p.Name)) + ")";
        }

        static string BindingFlagsText(BindingFlags flags)
        {
            var split = flags.ToString().Replace(" ", "").Split(',');
            var str = $"BindingFlags.{split[0]}";
            for (var i = 1; i < split.Length; ++i)
            {
                str += $"|BindingFlags.{split[i]}";
            }

            return str;
        }
    }
}
