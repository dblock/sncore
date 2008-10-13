using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

namespace SnCore.Data.Mapping
{
    class Program
    {
        struct MappingProperty
        {
            public string ClassName;
            public string PropertyName;

            public MappingProperty(string classname, string propertyname)
            {
                ClassName = classname;
                PropertyName = propertyname;
            }
        };

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("SnCore.Data.Mapping: fix hibernate mappings");

                if (args.Length == 0)
                {
                    Console.WriteLine("syntax: SnCore.Data.Mapping.exe [Project.csproj]");
                    throw new Exception("missing project name");
                }

                string projectFullPath = Path.GetFullPath(args[0]);
                string projectFileName = Path.GetFileName(projectFullPath);
                string projectDirectory = Path.GetDirectoryName(projectFullPath);

                XmlDocument projectXml = new XmlDocument();
                Console.WriteLine("Loading {0}", projectFullPath);
                projectXml.Load(projectFullPath);

                XmlNamespaceManager projectXmlNsMgr = new XmlNamespaceManager(projectXml.NameTable);
                string msbuildns = "http://schemas.microsoft.com/developer/msbuild/2003";
                projectXmlNsMgr.AddNamespace("msbuild", msbuildns);

                // add all interface files to the project, IDbObject.cs, IDbPictureObject.cs, etc.
                Console.WriteLine("Adding interface files ...");

                AdditionalProjectFilesConfigurationSection additionalProjectFiles = (AdditionalProjectFilesConfigurationSection) ConfigurationManager.GetSection("AdditionalProjectFiles");
                foreach (AdditionalProjectFileConfigurationElement interfaceFile in additionalProjectFiles.AdditionalProjectFiles)
                {
                    string interfaceFileName = Path.GetFileName(interfaceFile.Filename);
                    Console.Write(" {0}: ", interfaceFileName);

                    XmlNode compileNode = projectXml.SelectSingleNode(
                        "/msbuild:Project/msbuild:ItemGroup/msbuild:Compile", projectXmlNsMgr);
                    if (compileNode == null)
                    {
                        throw new Exception("Missing Compile ItemGroup");
                    }

                    XmlNode compileItemGroupNode = compileNode.ParentNode;
                    XmlNode compileIncludeNode = compileItemGroupNode.SelectSingleNode(string.Format(
                        "msbuild:Compile[@Include='{0}']", interfaceFileName), projectXmlNsMgr);

                    if (compileIncludeNode == null)
                    {
                        compileIncludeNode = projectXml.CreateElement("Compile", msbuildns);
                        XmlAttribute compileIncludeNodeIncludeAttribute = projectXml.CreateAttribute("Include");
                        compileIncludeNodeIncludeAttribute.Value = interfaceFileName;
                        compileIncludeNode.Attributes.Append(compileIncludeNodeIncludeAttribute);
                        XmlElement subtypeElement = projectXml.CreateElement("SubType", msbuildns);
                        subtypeElement.AppendChild(projectXml.CreateTextNode("Code"));
                        compileIncludeNode.AppendChild(subtypeElement);
                        compileItemGroupNode.AppendChild(compileIncludeNode);
                        Console.WriteLine("added");
                    }
                    else
                    {
                        Console.WriteLine("skipped");
                    }
                }

                projectXml.Save(projectFullPath);

                RemoveMappingConfigurationSection removeMappings = (RemoveMappingConfigurationSection)ConfigurationManager.GetSection("RemoveMappings");
                foreach (RemoveMappingConfigurationElement removeMapping in removeMappings.RemoveMappings)
                {
                    string mappingFileName = Path.Combine(projectDirectory, removeMapping.Class + ".hbm.xml");
                    Console.WriteLine(" {0}", Path.GetFileName(mappingFileName));
                    XmlDocument mappingXml = new XmlDocument();
                    mappingXml.Load(mappingFileName);
                    XmlNamespaceManager mappingXmlNsMgr = new XmlNamespaceManager(mappingXml.NameTable);
                    string hns = "urn:nhibernate-mapping-2.0";
                    mappingXmlNsMgr.AddNamespace("hns", hns);
                    XmlNode bagNode = mappingXml.SelectSingleNode(string.Format(
                        "//hns:bag[@name='{0}']", removeMapping.Bag), mappingXmlNsMgr);
                    if (bagNode != null)
                    {
                        Console.WriteLine("  Delete: {0}", removeMapping.Bag);
                        bagNode.ParentNode.RemoveChild(bagNode);
                        mappingXml.Save(mappingFileName);
                    }
                }

                foreach (string mappingFileName in Directory.GetFiles(projectDirectory, "*.hbm.xml"))
                {
                    Console.WriteLine(" {0}", Path.GetFileName(mappingFileName));
                    XmlDocument mappingXml = new XmlDocument();
                    mappingXml.Load(mappingFileName);
                    XmlNamespaceManager mappingXmlNsMgr = new XmlNamespaceManager(mappingXml.NameTable);
                    string hns = "urn:nhibernate-mapping-2.0";
                    mappingXmlNsMgr.AddNamespace("hns", hns);
                    // get the mapping node
                    XmlNode hibernateMappingNode = mappingXml.SelectSingleNode("/hns:hibernate-mapping", mappingXmlNsMgr);
                    // remove puzzle comment
                    if (hibernateMappingNode != null)
                    {
                        if (hibernateMappingNode.PreviousSibling.NodeType == XmlNodeType.Comment)
                            mappingXml.RemoveChild(hibernateMappingNode.PreviousSibling);

                        // make all bags lazy
                        XmlNodeList bags = hibernateMappingNode.SelectNodes("//hns:bag", mappingXmlNsMgr);
                        foreach (XmlNode bag in bags)
                        {
                            Console.WriteLine("  Bag: {0}", bag.Attributes["name"].Value);
                            XmlAttribute lazyAttribute = mappingXml.CreateAttribute("lazy");
                            lazyAttribute.Value = "true";
                            // bag.Attributes.SetNamedItem(lazyAttribute);
                            bag.Attributes.RemoveNamedItem("lazy");
                            bag.Attributes.Prepend(lazyAttribute);
                        }

                        ClobPropertiesConfigurationSection clobProperties = (ClobPropertiesConfigurationSection) ConfigurationManager.GetSection("ClobProperties");
                        foreach (ClobPropertyConfigurationElement clobProperty in clobProperties.ClobProperties)
                        {
                            XmlNode propertyNode = hibernateMappingNode.SelectSingleNode(string.Format(
                                "//hns:property[@name='{0}'][@type='String']", clobProperty.Property), mappingXmlNsMgr);
                            if (propertyNode != null)
                            {
                                Console.WriteLine("  Property: {0}", propertyNode.Attributes["name"].Value);
                                XmlAttribute typeAttribute = mappingXml.CreateAttribute("type");
                                typeAttribute.Value = "StringClob";
                                propertyNode.Attributes.SetNamedItem(typeAttribute);
                            }
                        }

                        XmlNodeList binaryNodes = hibernateMappingNode.SelectNodes("//hns:property[@type='Byte[]']", mappingXmlNsMgr);
                        foreach (XmlNode binaryNode in binaryNodes)
                        {
                            Console.WriteLine("  Property: {0}", binaryNode.Attributes["name"].Value);
                            XmlAttribute typeAttribute = mappingXml.CreateAttribute("type");
                            typeAttribute.Value = "BinaryBlob";
                            binaryNode.Attributes.SetNamedItem(typeAttribute);
                        }
                    }

                    // update namespace version
                    mappingXml.DocumentElement.SetAttribute("xmlns", 
                        (string)ConfigurationManager.AppSettings["nhibernateNamespace"]);
                    // save mapping file
                    mappingXml.Save(mappingFileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }
    }
}
