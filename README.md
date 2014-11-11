Welcome to DynamicConfigurationManager!	{#welcome}
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
to create a configuration node you simply need to inherit from the class _ConfigurationNode_ and override the _CreateProperties_ and _DescribePath_ methods
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
	}
```
	
 inside the _CreateProperties_ you declare the properties that you ConfigurationNode provides. The out-of-the-box property types are 

 - StringProperty - holds a string value
 - NumericProperty< TNumeric > - Holds a numeric value of type TNumeric (int, double etc.)
 - EnumProperty - Holds a value of some enum type
 - ListProperty<TListItem> - holds a value from a set of items of type TListItem  

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
the indexer operator enables to give name with spaces, which cannot be achieved with the dot operator
> Written with [StackEdit](https://stackedit.io/).
