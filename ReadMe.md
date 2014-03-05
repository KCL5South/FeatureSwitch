**Feature Switch**

_Usage_:

`IFeatureSwitch` is the interface used to determine which features are on or off.

	using FS;

	...

	public void SomeFunction(IFeatureSwitch switch)
	{
		if(switch["Feature1"])
		{
			//... Feature 1 logic ...
		}
		else if(switch["Feature2"])
		{
			//... Feature 2 logic ...
		}
	}

_Configuration_:

*FeatureSwitch* uses [Data Contract Serialization](http://msdn.microsoft.com/en-us/library/system.runtime.serialization.datacontractserializer.aspx).  The schema of the configuration file is as follows:

	<?xml version="1.0" encoding="utf-8"?>
	<xs:schema targetNamespace="https://www.kcl-data.com"
		elementFormDefault="qualified"
		xmlns="https://www.kcl-data.com"
		xmlns:xs="http://www.w3.org/2001/XMLSchema">
		<xs:element name="Features" nillable="true">
			<xs:complexType>
				<xs:sequence>
					<xs:element name="Feature" maxOccurs="unbounded" minOccurs="0" nillable="false">
						<xs:complexType>
							<xs:sequence>
								<xs:element name="Key" type="xs:string" maxOccurs="1" minOccurs="1" nillable="false"/>
								<xs:element name="Enabled" type="xs:boolean" maxOccurs="1" minOccurs="1" nillable="false"/>
							</xs:sequence>
						</xs:complexType>
					</xs:element>
				</xs:sequence>
			</xs:complexType>
		</xs:element>
	</xs:schema>

So, given the following configuration file:

	<?xml version="1.0" encoding="utf-8"?>
	<Features xmlns="https://www.kcl-data.com">
		<Feature>
			<Key>Feature1</Key>
			<Enabled>false</Enabled>
		</Feature>
		<Feature>
			<Key>Feature1.SubFeature</Key>
			<Enabled>true</Enabled>
		</Feature>
	</Features>

The following code would produce `False`:

	string configurationFilePath = <some configuration file path>;
	IFeatureSwitch features = null;
	
	if(System.IO.File.Exists(configurationFilePath))
		features = FeatureSwitch.Create(System.IO.File.OpenRead(configurationFilePath));
	else
		throw new System.InvalidOperation("Unable to locate feature configuration file.");

	System.Console.WriteLine(features["Feature1"]);

The features work like namespaces.  If a parent namespace is turned off, then all of it's children are also off.  Given the above scenario, the following bit of code will produce `False` as well.

	System.Console.WriteLine(features["SubFeature"]);

_Future Plans_:

Since the features are organized similarly to namespaces, things such as whole class types and urls could be translated into feature keys.