using Mono.Cecil;
using System.Text.Json;
using System.Xml.Linq;

namespace DllMetadataExtractor;

public class Extractor
{
    public string Extract(string assemblyPath)
    {
        if (!File.Exists(assemblyPath))
        {
            return JsonSerializer.Serialize(new { error = "Assembly file not found." });
        }

        var xmlDocPath = Path.ChangeExtension(assemblyPath, ".xml");
        var comments = LoadComments(xmlDocPath);

        var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);
        var metadata = new AssemblyMetadata
        {
            Name = assembly.Name.Name,
            Types = assembly.MainModule.Types
                .Where(t => t.IsPublic && (t.IsClass || t.IsInterface))
                .Select(type => new TypeMetadata
                {
                    Name = type.Name,
                    Namespace = type.Namespace,
                    FullName = type.FullName,
                    Documentation = comments.GetValueOrDefault(GetXmlDocKey(type)),
                    Methods = type.Methods
                        .Where(m => m.IsPublic && !m.IsConstructor && !m.IsGetter && !m.IsSetter)
                        .Select(method => new MethodMetadata
                        {
                            Name = method.Name,
                            ReturnType = method.ReturnType.FullName,
                            Documentation = comments.GetValueOrDefault(GetXmlDocKey(method)),
                            Parameters = method.Parameters.Select(p => new ParameterMetadata
                            {
                                Name = p.Name,
                                Type = p.ParameterType.FullName
                            }).ToList()
                        }).ToList(),
                    Properties = type.Properties
                        .Where(p => p.GetMethod?.IsPublic ?? false)
                        .Select(prop => new PropertyMetadata
                        {
                            Name = prop.Name,
                            Type = prop.PropertyType.FullName,
                            Documentation = comments.GetValueOrDefault(GetXmlDocKey(prop))
                        }).ToList()
                }).ToList()
        };

        return JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
    }

    private Dictionary<string, string> LoadComments(string xmlPath)
    {
        if (!File.Exists(xmlPath))
        {
            return new Dictionary<string, string>();
        }

        try
        {
            var xdoc = XDocument.Load(xmlPath);
            return xdoc.Descendants("member")
                .ToDictionary(
                    member => member.Attribute("name")!.Value,
                    member => string.Concat(member.DescendantNodes().OfType<XText>()).Trim()
                );
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    private string GetXmlDocKey(TypeDefinition type) => $"T:{type.FullName}";

    private string GetXmlDocKey(MethodDefinition method)
    {
        var parameters = string.Join(",", method.Parameters.Select(p => p.ParameterType.FullName));
        return $"M:{method.DeclaringType.FullName}.{method.Name}{(string.IsNullOrEmpty(parameters) ? "" : $"({parameters})")}";
    }

    private string GetXmlDocKey(PropertyDefinition prop) => $"P:{prop.DeclaringType.FullName}.{prop.Name}";
}

// Data model for serialization
public class AssemblyMetadata
{
    public required string Name { get; init; }
    public required List<TypeMetadata> Types { get; init; }
}

public class TypeMetadata
{
    public required string Name { get; init; }
    public required string Namespace { get; init; }
    public required string FullName { get; init; }
    public string? Documentation { get; init; }
    public required List<MethodMetadata> Methods { get; init; }
    public required List<PropertyMetadata> Properties { get; init; }
}

public class MethodMetadata
{
    public required string Name { get; init; }
    public required string ReturnType { get; init; }
    public string? Documentation { get; init; }
    public required List<ParameterMetadata> Parameters { get; init; }
}

public class PropertyMetadata
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public string? Documentation { get; init; }
}

public class ParameterMetadata
{
    public required string Name { get; init; }
    public required string Type { get; init; }
} 