using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


    public delegate void ZenInjectMethod(object obj, object[] args);
    public delegate object ZenFactoryMethod(object[] args);
    public delegate void ZenMemberSetterMethod(object obj, object value);

    public class InjectTypeInfo
    {
        public readonly Type Type;
        public readonly InjectMemberInfo[] InjectMembers;

        public InjectTypeInfo(
            Type type,

            InjectMemberInfo[] injectMembers)
        {
            Type = type;
            InjectMembers = injectMembers;
        }

        // Filled in later
        public InjectTypeInfo BaseTypeInfo
        {
            get; set;
        }


        public class InjectMemberInfo
        {
            public readonly ZenMemberSetterMethod Setter;
            public readonly InjectableInfo Info;

            public InjectMemberInfo(
                ZenMemberSetterMethod setter,
                InjectableInfo info)
            {
                Setter = setter;
                Info = info;
            }
        }

    }
