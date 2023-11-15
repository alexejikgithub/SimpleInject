using System;

// An injectable is a field or property with [Inject] attribute
// Or a constructor parameter
public class InjectableInfo
{
	public readonly string MemberName;
	public readonly Type MemberType;

	public InjectableInfo(string memberName, Type memberType)
	{
		MemberType = memberType;
		MemberName = memberName;
	}
}

