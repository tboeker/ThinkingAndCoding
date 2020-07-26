using Swashbuckle.AspNetCore;

namespace proj2
{
    public static class MySwaggerDocs
    {
        public static MySwaggerDoc[] GetSwaggerDocs()
        {
            return new[]
            {
                new MySwaggerDoc
                {
                    Name = "proj2",
                    Title = "Project 2"
                }
            };
        }
    }
}