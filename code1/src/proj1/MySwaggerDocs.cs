using Swashbuckle.AspNetCore;

namespace proj1
{
    public static class MySwaggerDocs
    {
        public static MySwaggerDoc[] GetSwaggerDocs()
        {
            return new[]
            {
                new MySwaggerDoc
                {
                    Title = "Project 1 Test 1",
                    Name = "proj1-test1"
                },
                new MySwaggerDoc
                {
                    Title = "Project 1 Test 2",
                    Name = "proj1-test2"
                },
                new MySwaggerDoc
                {
                    Title = "Project 1 Lifetime",
                    Name = "proj1-lifetime"
                }
            };
        }
    }
}