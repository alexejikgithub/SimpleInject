using System.Reflection;

public static class ReflectionInfoTypeInfoConverter
{
	public static InjectTypeInfo.InjectMemberInfo ConvertField(ReflectionTypeInfo.InjectFieldInfo injectField)
	{
		return new InjectTypeInfo.InjectMemberInfo(GetSetter(injectField.FieldInfo), injectField.InjectableInfo);
	}

	static MemberSetterMethod GetSetter(MemberInfo memInfo)
	{
		var fieldInfo = memInfo as FieldInfo;

		return (injectable, value) => fieldInfo.SetValue(injectable, value);
	}

}
