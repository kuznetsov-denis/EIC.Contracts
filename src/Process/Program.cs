using System;

using EIC.Contracts.Container;

namespace Process
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var task = (new ContractsProcess()).RunAsync(args);
				task.Wait();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Console.ReadLine();
			}
		}
	}
}
