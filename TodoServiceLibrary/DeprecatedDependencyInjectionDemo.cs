using System;

namespace TodoServiceLibrary
{
    //  This class is ONLY for demonstrating Dependency Injection migration challenges.
    //  It should NOT be used in production code.
    public class DeprecatedDependencyInjectionDemo
    {
        //  LegacyLogger - Represents an older component with no interface
        public class LegacyLogger
        {
            public void Log(string message)
            {
                Console.WriteLine($"[Legacy Logger] {message}");
            }
        }

        //  LegacyDataAccessLayer - Represents a component that is tightly coupled
        public class LegacyDataAccessLayer
        {
            public string GetData()
            {
                return "Data from LegacyDataAccessLayer";
            }
        }

        private readonly LegacyLogger _logger = new LegacyLogger();
        private readonly LegacyDataAccessLayer _dataAccess = new LegacyDataAccessLayer();

        public void DoSomethingWithData()
        {
            _logger.Log("DeprecatedDependencyInjectionDemo.DoSomethingWithData called.");
            string data = _dataAccess.GetData();
            Console.WriteLine($"  Data: {data}");
        }

        public DeprecatedDependencyInjectionDemo()
        {
            Console.WriteLine("DeprecatedDependencyInjectionDemo created.");
        }
    }
}