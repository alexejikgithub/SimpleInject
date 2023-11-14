using System;
using System.Collections.Generic;
using System.Reflection;


    public class ReflectionTypeInfo
    {
        public readonly Type Type;
        public readonly Type BaseType;
        public readonly List<InjectFieldInfo> InjectFields;

        public ReflectionTypeInfo(
            Type type,
            Type baseType,
            List<InjectFieldInfo> injectFields)

        {
            Type = type;
            BaseType = baseType;
            InjectFields = injectFields;

        }

        public class InjectFieldInfo
        {
            public readonly FieldInfo FieldInfo;
            public readonly InjectableInfo InjectableInfo;

            public InjectFieldInfo(
                FieldInfo fieldInfo,
                InjectableInfo injectableInfo)
            {
                InjectableInfo = injectableInfo;
                FieldInfo = fieldInfo;
            }
        }
    }


