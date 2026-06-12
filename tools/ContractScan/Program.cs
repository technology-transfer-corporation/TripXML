using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

// Replicates (approximately) the per-contract XmlSerializer import CoreWCF performs at
// host startup, so every XML type-name collision across all 104 service contracts is
// reported in ONE run instead of one boot-crash at a time.
internal static class Program
{
    private static int Main()
    {
        var failures = new List<string>();
        int scanned = 0;

        foreach (var entry in wsTripXML.Hosting.ServiceRoutes.All)
        {
            scanned++;
            var contract = entry.Contract;
            var ns = GetContractNamespace(contract);
            var importer = new XmlReflectionImporter(ns);
            var seen = new HashSet<Type>();

            foreach (var method in contract.GetMethods())
            {
                foreach (var t in ExpandTypes(method))
                {
                    if (t == typeof(string) || t == typeof(void) || t.IsPrimitive || !seen.Add(t))
                        continue;
                    try
                    {
                        importer.ImportTypeMapping(t, ns);
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"{contract.Name}: {t.FullName}: {ex.GetBaseException().Message}");
                    }
                }
            }
        }

        Console.WriteLine($"scanned contracts: {scanned}");
        Console.WriteLine($"failures: {failures.Count}");
        foreach (var f in failures.Distinct())
            Console.WriteLine(f);
        return failures.Count == 0 ? 0 : 1;
    }

    private static string GetContractNamespace(Type contract)
    {
        var attr = contract.GetCustomAttributes().FirstOrDefault(a => a.GetType().Name == "ServiceContractAttribute");
        var prop = attr?.GetType().GetProperty("Namespace");
        return prop?.GetValue(attr) as string ?? "http://tempuri.org/";
    }

    private static IEnumerable<Type> ExpandTypes(MethodInfo method)
    {
        foreach (var p in method.GetParameters())
            foreach (var t in ExpandMessageContract(p.ParameterType))
                yield return t;
        foreach (var t in ExpandMessageContract(method.ReturnType))
            yield return t;
    }

    // MessageContract wrappers are imported member-by-member by WCF; import their
    // field types (header + body members) to mirror that scope.
    private static IEnumerable<Type> ExpandMessageContract(Type type)
    {
        if (type == null || type == typeof(void)) yield break;
        bool isMessageContract = type.GetCustomAttributes().Any(a => a.GetType().Name == "MessageContractAttribute");
        if (!isMessageContract)
        {
            yield return type;
            yield break;
        }
        foreach (var f in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            yield return f.FieldType;
    }
}
