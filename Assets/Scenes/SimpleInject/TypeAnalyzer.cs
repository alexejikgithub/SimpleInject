using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


    public delegate InjectTypeInfo ZenTypeInfoGetter();

    public enum ReflectionBakingCoverageModes
    {
        FallbackToDirectReflection,
        NoCheckAssumeFullCoverage,
        FallbackToDirectReflectionWithWarning
    }

    public static class TypeAnalyzer
    {
        static Dictionary<Type, InjectTypeInfo> _typeInfo = new Dictionary<Type, InjectTypeInfo>();

        // We store this separately from InjectTypeInfo because this flag is needed for contract
        // types whereas InjectTypeInfo is only needed for types that are instantiated, and
        // we want to minimize the types that generate InjectTypeInfo for
        static Dictionary<Type, bool> _allowDuringValidation = new Dictionary<Type, bool>();

        // Use double underscores for generated methods since this is also what the C# compiler does
        // for things like anonymous methods
        public const string ReflectionBakingGetInjectInfoMethodName = "__zenCreateInjectTypeInfo";
        public const string ReflectionBakingFactoryMethodName = "__zenCreate";
        public const string ReflectionBakingInjectMethodPrefix = "__zenInjectMethod";
        public const string ReflectionBakingFieldSetterPrefix = "__zenFieldSetter";
        public const string ReflectionBakingPropertySetterPrefix = "__zenPropertySetter";

        public static ReflectionBakingCoverageModes ReflectionBakingCoverageMode
        {
            get; set;
        }

        

        public static bool HasInfo<T>()
        {
            return HasInfo(typeof(T));
        }

        public static bool HasInfo(Type type)
        {
            return TryGetInfo(type) != null;
        }

        public static InjectTypeInfo GetInfo<T>()
        {
            return GetInfo(typeof(T));
        }

        public static InjectTypeInfo GetInfo(Type type)
        {
            var info = TryGetInfo(type);
            //Assert.IsNotNull(info, "Unable to get type info for type '{0}'", type);
            return info;
        }

        public static InjectTypeInfo TryGetInfo<T>()
        {
            return TryGetInfo(typeof(T));
        }

        public static InjectTypeInfo TryGetInfo(Type type) //!!!!!!!!!!
        {
            InjectTypeInfo info;


            {
                if (_typeInfo.TryGetValue(type, out info))
                {
                    return info;
                }
            }

                info = GetInfoInternal(type);

            if (info != null)
            {
                //Assert.IsEqual(info.Type, type);
                //Assert.IsNull(info.BaseTypeInfo);

                var baseType = type.BaseType();

                if (baseType != null && !ShouldSkipTypeAnalysis(baseType))
                {
                    info.BaseTypeInfo = TryGetInfo(baseType);
                }
            }

            {
                _typeInfo.Add(type, info);
            }

            return info;
        }

        static InjectTypeInfo GetInfoInternal(Type type) //!!!!!!!!!!!!!!!!
        {
            if (ShouldSkipTypeAnalysis(type))
            {
                return null;
            }


            {
                return CreateTypeInfoFromReflection(type);
            }
        }

        public static bool ShouldSkipTypeAnalysis(Type type)
        {
            return type == null || type.IsEnum() || type.IsArray || type.IsInterface()
                || type.ContainsGenericParameters() || IsStaticType(type)
                || type == typeof(object);
        }

        static bool IsStaticType(Type type)
        {
            // Apparently this is unique to static classes
            return type.IsAbstract() && type.IsSealed();
        }

        static InjectTypeInfo CreateTypeInfoFromReflection(Type type)
        {
            var reflectionInfo = ReflectionTypeAnalyzer.GetReflectionInfo(type);


            var memberInfos = reflectionInfo.InjectFields.Select(
                x => ReflectionInfoTypeInfoConverter.ConvertField(type, x)).ToArray();

            return new InjectTypeInfo(
                type, memberInfos);
        }
    }

