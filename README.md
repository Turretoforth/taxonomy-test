# Taxonomy
## Description
This API serves the purpose of representing the structure of a company with a tree structure composed of one root and child nodes.

It uses SQLite as storage and is made with .NET 8 using Entity Framework Core 8.
## How to use
On first launch, the database is automatically created (taxonomy.db) and the API is available on [https://localhost:7143]()

A swagger is available on [https://localhost:7143/swagger/index.html]()

The operations currently available are : 
#### Creation of an employee
POST [https://localhost:7143/Taxonomy]()

A JSON with this format must be sent:
```
{
  "name": "string",
  "role": "string",
  "manager": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "department": "string",
  "programmingLanguage": "string"
}
```
*name*: The name of the new employee.

*role*: The role of the employee. The current allowed values are: 'CEO', 'Developer' or 'Manager'.

*manager*: The GUID of the manager of the employee. Can be ommited if not necessary.

*department*: Required if the role is 'Manager', can be ommited otherwise. It represents the department they are managing.

*programmingLanguage*: Required if the role is 'Developer', can be ommited otherwise. It represents the programming language they are the strongest in.

If the creation is successful, the details of the newly created employee will be returned.
#### Getting the direct subordinates of an employee
GET [https://localhost:7143/Taxonomy/subordinates/\<GUID\>]()

A valid GUID of an existing employee must be given.

A list of the direct subordinates will be returned with all of their details.
#### Changing the manager of an employee
POST [https://localhost:7143/Taxonomy/\<GUID\>?newManager=\<GUID\>]()

The first and second GUID must be a valid GUID of an existing employee. 
The new manager cannot be the employee itself and **the operation cannot currently be done on an employee with subordinates**.

If successful, the API will return a 200 (OK) response.

## Things to add or improve
- Unit tests
- Better decoupling of the business from presentation and storage
- Better handling of the "Role" situation (Enum?)
- Handling of the manager change for employees with subordinates in a way that won't break the tree structure
- Add limits to the input (Notably the Name)
- Database connection string in the config
- More pertinent and useful logging
