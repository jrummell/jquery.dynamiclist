using System;

namespace Minify
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args == null)
                {
                    throw GetInvalidArgumentsException();
                }

                if (args.Length < 3)
                {
                    throw GetInvalidArgumentsException();
                }

                string destination = null;
                string[] sourcePaths = null;
                BundleType type = BundleType.None;

                try
                {
                    foreach (string arg in args)
                    {
                        if (arg.StartsWith("-out") || arg.StartsWith("-output"))
                        {
                            destination = arg.Split(':')[1];
                        }
                        else if (arg.StartsWith("-source"))
                        {
                            sourcePaths = arg.Split(':')[1].Split(';');
                        }
                        else if (arg.StartsWith("-type"))
                        {
                            type = (BundleType) Enum.Parse(typeof (BundleType), arg.Split(':')[1], ignoreCase: true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw GetInvalidArgumentsException(null, ex);
                }

                try
                {
                    BundleService service = new BundleService(new FileService());
                    service.Bundle(destination, sourcePaths, type);
                }
                catch (ArgumentException ex)
                {
                    throw GetInvalidArgumentsException(ex.ParamName, ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("StackTrace:");
                Console.WriteLine(ex);
            }
        }

        private static ArgumentException GetInvalidArgumentsException(string argumentName = null,
                                                                      Exception innerException = null)
        {
            return new ArgumentException(
                "Required parameters: -out:<output file path> -source:<semi-colon separated list of source file paths> -type:<js|Script|css|Style>",
                argumentName, innerException);
        }
    }
}