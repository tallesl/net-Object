# ToObject

[![][build-img]][build]
[![][nuget-img]][nuget]

[build]:     https://ci.appveyor.com/project/TallesL/ToObject
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/ToObject

[nuget]:     http://badge.fury.io/nu/ToObject
[nuget-img]: https://badge.fury.io/nu/ToObject.png

Automagically parses (with reflection) an AppSettings/Dictionary/DataRow/DataTable to a custom class of yours.

## IDictionary&lt;string, object&gt;

```cs
using ToObject;

var userData = new Dictionary<string, object> { { "Name", "John Smith" }, { "Birth", new DateTime(1970, 1, 1) } };
var userObject = userData.ToObject<User>();
```

## IDictionary&lt;string, string&gt;

```cs
using ToObject;

var userData = new Dictionary<string, string> { { "Name", "John Smith" }, { "Birth", "1970-01-01" } };
var userObject = userData.ToObject<User>();

userData["Birth"] = "Not a date!";
var shitHappens = userData.ToObject<User>(); // throws CouldntParseException
```

## DataRow/DataTable

```cs
using ToObject;

var users = dataTable.ToObject<User>();
```

## NameValueCollection (&lt;appSettings&gt;)

```cs
using ToObject;

var configuration = ConfigurationManager.AppSettings.ToObject<SomeConfigurationClass>();
```
