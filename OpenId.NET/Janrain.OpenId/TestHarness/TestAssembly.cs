using System;
using System.Reflection;
using Janrain.TestHarness;


class TestAssembly
{
    public static void Main ( string[] args )
    {
        Assembly assembly = Assembly.LoadFrom (args[0]);

        BindingFlags flags = (BindingFlags.NonPublic | BindingFlags.Public |
                BindingFlags.Static | BindingFlags.Instance | 
                BindingFlags.DeclaredOnly);

        object[] typeAttrs, methodAttrs;
        string msg;
        object inst;
        foreach (Type type in assembly.GetTypes()) {
            typeAttrs = type.GetCustomAttributes(
                    typeof(TestSuiteAttribute), true);
            if (typeAttrs.Length == 0)
                continue;

        msg = String.Format("TestSuite: {0} in {1}", 
                type.Name, type.Namespace);
        Console.WriteLine(msg);
        Console.WriteLine("".PadLeft(msg.Length, '='));

        inst = Activator.CreateInstance(type);
        foreach (MethodInfo method in type.GetMethods())
        {
        methodAttrs = method.GetCustomAttributes(
            typeof(TestAttribute), true);
        if (methodAttrs.Length == 0)
            continue;

        Console.WriteLine("Running {0} Test:", method.Name);
        try
        {
            method.Invoke(inst, new object[0]);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Running Test: {0} in {1}", 
                      method.Name, type.FullName);
            if (e.InnerException == null)
            throw e;
            throw e.InnerException;
        }
        Console.WriteLine("Succeeded!");
        }
        Console.WriteLine();
        }

        //MethodInfo[] mi = t.GetMethods(flags);

        //





        /*      // Print the Class Name
        
        // Print the name space
        Console.WriteLine ("Type Namespace : {0}", type.Namespace); 
        // Print the Base Class Name
        Console.WriteLine ("Type Base Class : {0}",  
                   (type.BaseType != null) ?  
                   type.BaseType.FullName :  
                   "No Base Class Found...");
                   Console.WriteLine("");*/
    }
}
