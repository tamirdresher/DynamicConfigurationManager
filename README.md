Welcome to DynamicConfigurationManager!	
=====================

[![Build status](https://ci.appveyor.com/api/projects/status/327ypy8w0vtdqhwe)](https://ci.appveyor.com/project/tamirdresher/dynamicconfigurationmanager)

##Why another Configuration Manager for .NET?

###Code First
Each configuration is set by declaring classes and properties. providing type safety
###One Place Management
code is king, you dont need to mess with the configuration file at all
###Versioning 
Each Configuration property has version and the configuration system know how to handle new versions
###JSON Support
the configuration file is in json format
###Dynamic Loading
Configuration node could be discovered dynamically - better extendability and pluggability
###Editor 
WPF editor that can show the configuration (and dynamically find the Configuration Elements)
###Easier Configuration Reading 
using _dynamic_, indexer, regular properties
##Creating a Configuration
DynamicConfigurationManager is code first, meaning that you need to create a class that represent your configuration node. A configuration node is like a branch inside the configuration tree that can holds properties for different configuration values .
to create a configuration node you simply need to inherit from the class _ConfigurationNode_ and override the _CreateProperties_ 
and _DescribePath_ methods
Here is a simple example:

```cs
	public class ASimpleNode : ConfigurationNode
	{
	    public ASimpleNode()
	        : base("ASimpleNode")
	    {
	
	    }
	
	    public override IEnumerable<IConfigurationProperty> CreateProperties()
	    {
	        yield return new StringProperty("AStringProperty", "this is a property that can hold a string", "defaultValue");
	    }
		
		public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.Level1.Level2.Level3;
        }
	}
```
 inside the _CreateProperties_ you declare the properties that you ConfigurationNode provides. The out-of-the-box property types are 

 - StringProperty - holds a string value
 - NumericProperty< TNumeric > - Holds a numeric value of type TNumeric (int, double etc.)
 - EnumProperty - Holds a value of some enum type
 - ListProperty<TListItem> - holds a value from a set of items of type TListItem  

you can always create more types.

inside the _DescribePath_ you configure the location inside the configuration tree. 
```cs
public override object DescribePath(dynamic pathDescriber)
{
    return pathDescriber.Level1.Level2.Level3;
}
```
taken from the example above, the path will result in a tree like

 - Level1
	 - Level2
		 - Level3
			 - ASimpleNode
				 - AStringProperty
			
the path descriptor shown above support the dot operator or indexer [] operator
so the example could also be written as
```cs
public override object DescribePath(dynamic pathDescriber)
{
    return pathDescriber["Level1"]["Level2"]["Level3"];
}
```
the indexer operator enables to give name with spaces, which cannot be achieved with the dot operator.

##Working with the  ~~ConfigurationManager~~ ConfigurationReader
When creating the  ~~ConfigurationManager~~ you need to pass a collection of ConfigurationNodes.
```cs
public ConfigurationReader(IEnumerable<IConfigurationNode> configurationNodes)
```
the ConfigurationReader will read each of them and by using the _DescribePath_ method will build the tree of configuration.

To start using the ConfigurationReader, you need to call the _OpenConfiguration_ method.
you can open an existing configuration file by specifying the file path otherwise ConfigurationReader will create a file with a default name.

The configuration file has a version embedded into it which will be used by the ConfigurationReader and ConfigurationNode to validate it. 
you can specify inside your nodes if a new version will override the value for a property.

##Reading from the configuration
ConfigurationReader is code first so static typing is the prefered way:
```cs
SomeTestNodeDummy dynSomeConfig = configurationReader.GetConfigNode<SomeTestNodeDummy>();
var resolution = dynSomeConfig.Resolution;
```
the ConfigurationReader will use the path that is defined inside the Node to pull its data.

if you prefer a more verbose way to get the node you can use the _dynamic_ capabilities of the ConfigurationReader:

```cs
var theValue = configurationReader.AsDynamic().Level1..Level2.Level3.ASimpleNode.AStringProperty
string stringValue = theValue.ToString();
```

you can also use the indexer option if you prefer working with strings or in the case that some branch or property name contains spaces:
```cs
//option 1
var theValue = configurationReader.AsDynamic()["Level1"]["Level2"]["Level3"]["ASimpleNode"]["AStringProperty"];

//option 2
var theValue = configurationReader["Level1"]["Level2"]["Level3"]["ASimpleNode"]["AStringProperty"];

string stringValue = theValue.ToString();
```

You can even use a combination of all the methods
```cs

var theValue = configurationReader["Level1"].Level2["Level3"].ASimpleNode["AStringProperty"];

string stringValue = theValue.ToString();
```

But my advice is using the type-safe option.
> Written with [StackEdit](https://stackedit.io/).
