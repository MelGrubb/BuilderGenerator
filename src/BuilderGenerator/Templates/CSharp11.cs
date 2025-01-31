//namespace BuilderGenerator.Templates;

///// <summary>Defines code generation templates compatible with C# 11 features.</summary>
//public class CSharp11 : CSharp
//{
//    public override string BuilderForAttribute
//    {
//        get
//        {
//            return """
//                   namespace BuilderGenerator
//                   {
//                       [System.AttributeUsage(System.AttributeTargets.Class)]
//                       public class BuilderForAttribute : System.Attribute
//                       {
//                           public bool IncludeInternals { get; }
//                           public System.Type Type { get; }


//                           public BuilderForAttribute(System.Type type, bool includeInternals = false)
//                           {
//                               IncludeInternals = includeInternals;
//                               Type = type;
//                           }
//                       }


//                       [System.AttributeUsage(System.AttributeTargets.Class)]
//                       public class BuilderForAttribute<T> : BuilderForAttribute
//                       {
//                           public BuilderForAttribute(bool includeInternals = false) : base(typeof(T), includeInternals)
//                           {
//                           }
//                       }
//                   }
//                   """;
//        }
//    }
//}

