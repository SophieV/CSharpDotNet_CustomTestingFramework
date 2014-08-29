
namespace TestMVC4App.Models
{
    public abstract class TestSuite
    {
        public abstract string newServiceURLBase { get; }

        public abstract string oldServiceURLBase { get; }

        public abstract void RunAllTests();

        public string BuildOldServiceFullURL(int oldUserUPI)
        {
            return this.oldServiceURLBase + oldUserUPI;
        }
    }
}