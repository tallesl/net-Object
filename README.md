# DataTable to Object

[![][build-img]][build]
[![][nuget-img]][nuget]

[build]:     https://ci.appveyor.com/project/TallesL/DataTableToObject
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/DataTableToObject

[nuget]:     http://badge.fury.io/nu/DataTableToObject
[nuget-img]: https://badge.fury.io/nu/DataTableToObject.png

Automagically parses (with reflection) a [DataTable][DataTable] to a custom class of yours.

[DataTable]: https://msdn.microsoft.com/library/system.data.datatable.aspx

```cs
using DataTableToObject;

var users = dataTable.ToObject<User>(); // parses each row as an User and returns an IEnumerable<User>
                                        // the column names on the DataTable must match the property names on the class
```
