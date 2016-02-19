# Object

[![][build-img]][build]
[![][nuget-img]][nuget]

[build]:     https://ci.appveyor.com/project/TallesL/net-object
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/net-object?svg=true
[nuget]:     https://www.nuget.org/packages/Object
[nuget-img]: https://badge.fury.io/nu/Object.svg

Automagically parses (with reflection) an AppSettings/DataRow/DataTable/Dictionary to a custom class of yours.

## [NameValueCollection]&nbsp;([&lt;appSettings&gt;])

```cs
using ObjectLibrary;

var configuration = ConfigurationManager.AppSettings.ToObject<SomeConfigurationClass>();
```

[NameValueCollection]: https://msdn.microsoft.com/library/System.Collections.Specialized.NameValueCollection
[&lt;appSettings&gt;]: https://msdn.microsoft.com/library/System.Configuration.ConfigurationManager.AppSettings

## [DataRow]/[DataTable]

```cs
using ObjectLibrary;

var users = dataTable.ToObject<User>();
```

[DataRow]:   https://msdn.microsoft.com/library/System.Data.DataRow
[DataTable]: https://msdn.microsoft.com/library/System.Data.DataTable

## [IDictionary]&lt;string, object&gt;

```cs
using ObjectLibrary;

var userData = new Dictionary<string, object> { { "Name", "John Smith" }, { "Birth", new DateTime(1970, 1, 1) } };
var userObject = userData.ToObject<User>();
```

[IDictionary]: https://msdn.microsoft.com/library/System.Collections.IDictionary

## [IDictionary]&lt;string, string&gt;

```cs
using ObjectLibrary;

var userData = new Dictionary<string, string> { { "Name", "John Smith" }, { "Birth", "1970-01-01" } };
var userObject = userData.ToObject<User>();

userData["Birth"] = "Not a date!";
var shitHappens = userData.ToObject<User>(); // throws CouldNotParseException
```
