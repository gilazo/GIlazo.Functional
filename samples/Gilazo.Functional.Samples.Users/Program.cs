using System;
using static Gilazo.Functional.EitherFunctions;

namespace Gilazo.Functional.User.CSharp
{
	class User
	{
		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;
	}

	class Program
	{
		private static Either<string, User> ValidateFirstName(User user) =>
			user.FirstName switch
			{
				null => "First name cannot be null.",
				"" => "First name cannot be empty.",
				_ => user
			};

		private static Either<string, User> ValidateLastName(User user) =>
			user.LastName switch
			{
				null => "Last name cannot be null.",
				"" => "Last name cannot be empty.",
				_ => user
			};

		private static Either<string, User> ValidateEmail(User user) =>
			user.Email switch
			{
				null => "Email cannot be null.",
				"" => "Email cannot be empty",
				string e when !e.Contains('@') => "Email must contain @.",
				_ => user
			};

		public static Either<string, User> ValidateUser(Either<string, User> user) =>
			user
				.Bind(ValidateFirstName)
				.Bind(ValidateLastName)
				.Bind(ValidateEmail);

		private static Either<string, string[]> ValidateArgs(string[] args) =>
			args switch
			{
				null => "Array is null.",
				string[] a when a.Length != 3 => "Array must contain 3 elements.",
				_ => args
			};

		static void Main(string[] args)
		{
			// Style: Functional Fluent
			ValidateArgs(args)
				.Match(
					right: args => ValidateUser(new User { FirstName = args[0], LastName = args[1], Email = args[2] })
						.Match(
							right: user => Console.WriteLine("Fluent: User is valid!"),
							left: e => Console.WriteLine($"Error: {e}")
						),
					left: e => Console.WriteLine($"Error: {e}")
				);

			// Style: Functional fsharp like with EitherFunctions
			var argsEither = ValidateArgs(args);
			Match(argsEither,
				right: args =>
				{
					var user = new User { FirstName = args[0], LastName = args[1], Email = args[2] };
					var userEither = ValidateUser(user);
					Match(userEither,
						right: user => Console.WriteLine("EitherFunctions: User is valid!"),
						left: e => Console.WriteLine($"Error: {e}")
					);
				},
				left: e => Console.WriteLine($"Error: {e}")
			);

			// Style: Functional fSharp like with switches
			argsEither = ValidateArgs(args);
			Console.WriteLine(
				argsEither switch
				{
					Right<string, string[]> (string[] a) => ValidateUser(new User { FirstName = a[0], LastName = a[1], Email = a[2] }) switch
					{
						Right<string, User> _ => "Switches: User is valid!",
						Left<string, User> (string e) => $"Error: {e}",
						_ => "Unknown error"
					},
					Left<string, string[]> (string e) => $"Error: {e}",
					_ => "Unknown error"
				}
			);
		}
	}
}
