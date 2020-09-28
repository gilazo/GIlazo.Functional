# Gilazo.Functional

The purpose of this library is to bring functional programming paradigms into the imperative csharp world.

It is not a comprehensive solution, but provides the types necessary to write code in csharp while using more
functional styles.

## NuGet

| Package | Version |
| --- | --- |
| [Gilazo.Functional](https://www.nuget.org/packages/Gilazo.Functional) | [![NuGet](https://img.shields.io/nuget/v/Gilazo.Functional.svg)](https://www.nuget.org/packages/Gilazo.Functional)

## Examples

### fsharp

```fsharp
type Person =
        { FirstName: string
          LastName: string }

module PersonModule =
    let validateFirstName person =
        match person.FirstName with
        | null -> Error "No first name found."
        | "" -> Error "First name cannot be empty."
        | _ -> Ok person

    let validateLastName person =
        match person.LastName with
        | null -> Error "No last name found."
        | "" -> Error "Last name cannot be empty."
        | _ -> Ok person

    let validatePerson personResult =
        personResult
        |> Result.bind validateFirstName
        |> Result.bind validateLastName

    let createPerson (firstName : string, lastName : string) =
        let person = { FirstName = firstName; LastName = lastName }
        let result = validatePerson (Ok person)
        match result with
        | Ok person -> printfn "Valid! FirstName: %s LastName: %s" person.FirstName person.LastName
        | Error e -> printfn "Error: %s" e
```

### csharp with Gilazo.Functional

```csharp
class Person
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}

private Either<string, Person> ValidateFirstName(Person person) =>
    person.FirstName switch
    {
        null => "No first name found.",
        "" => "First name cannot be empty.",
        _ => person
    };

private Either<string, Person> ValidateLastName(Person person) =>
    person.LastName switch
    {
        null => "No last name found.",
        "" => "Last name cannot be empty.",
        _ => person
    };

private Either<string, Person> ValidatePerson(Either<string, Person> person) =>
    person
        .Bind(ValidateFirstName)
        .Bind(ValidateLastName);
```

With native switch expressions from `csharp 8` and the `Bind` functions from this library, a few styles can be used to write `csharp` in a more functional way.

Here are a few styles that can be used.

#### Functional Fluent
```csharp
public void CreatePerson(string firstName, string lastName)
{
    ValidatePerson(new Person { FirstName = firstName, LastName= lastName })
        .Match(
            right: person => Console.WriteLine($"Valid! FirstName: {person.FirstName} LastName: {person.LastName}"),
            left: error => Console.WriteLine($"Error: {error}")
        );
}
```

#### Functional fsharp like with EitherFunctions
```csharp
public void CreatePerson(string firstName, string lastName)
{
    var person = new Person { FirstName = "Jon", LastName = "Doe" };
    var personEither = ValidatePerson(person);
    Match(personEither,
        right: person => Console.WriteLine($"Valid! FirstName: {person.FirstName} LastName: {person.LastName}"),
        left: error => Console.WriteLine($"Error: {error}")
    );
}
```

#### Functional fsharp like with switches
```csharp
public void CreatePerson(string firstName, string lastName)
{
    var person = new Person { FirstName = "Jon", LastName = "Doe" };
    var personEither = ValidatePerson(person);
    Console.WriteLine(
        personEither switch
        {
            Right<string, Person> (Person p) => $"Valid! FirstName: {p.FirstName} LastName: {p.LastName}",
            Left<string, Person> (string e) => $"Error {e}",
            _ => "Unknown Error"
        }
    );
}
```

## TODO

1. Maybe
2. Better async
3. More functiona apis
