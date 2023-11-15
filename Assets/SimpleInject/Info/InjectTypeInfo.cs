
    public delegate void MemberSetterMethod(object obj, object value);

    public class InjectTypeInfo
    {
        public readonly InjectMemberInfo[] InjectMembers;

        public InjectTypeInfo(InjectMemberInfo[] injectMembers)
        {
            InjectMembers = injectMembers;
        }

        // Filled in later
        public InjectTypeInfo BaseTypeInfo
        {
            get; set;
        }

        public class InjectMemberInfo
        {
            public readonly MemberSetterMethod Setter;
            public readonly InjectableInfo Info;

            public InjectMemberInfo(MemberSetterMethod setter, InjectableInfo info)
            {
                Setter = setter;
                Info = info;
            }
        }
    }
