# ToObject

[![][build-img]][build]
[![][nuget-img]][nuget]

[build]:     https://ci.appveyor.com/project/TallesL/DataTableToObject
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/DataTableToObject

[nuget]:     http://badge.fury.io/nu/DataTableToObject
[nuget-img]: https://badge.fury.io/nu/DataTableToObject.png

Automagically parses (with reflection) a Dictionary/DataRow/DataTable to a custom class of yours.

## IDictionary<string, object>

```cs
using ToObject;

var user = new Dictionary<string, object> { { "Name", "John Smith" }, { "Birth", new DateTime(1970, 1, 1) } };
var actualUser = user.ToObject<User>();
```

## DataRow/DataTable

```cs
using ToObject;

var users = dataTable.ToObject<User>();
```
