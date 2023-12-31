using System;
using System.Collections.Generic;
using System.Reflection;


    public class ReflectionTypeInfo
    {
        public readonly List<InjectFieldInfo> InjectFields;

        public ReflectionTypeInfo( List<InjectFieldInfo> injectFields)
        {
            InjectFields = injectFields;
        }

        public class InjectFieldInfo
        {
            public readonly FieldInfo FieldInfo;
            public readonly InjectableInfo InjectableInfo;

            public InjectFieldInfo(FieldInfo fieldInfo, InjectableInfo injectableInfo)
            {
                InjectableInfo = injectableInfo;
                FieldInfo = fieldInfo;
            }
        }
    }


