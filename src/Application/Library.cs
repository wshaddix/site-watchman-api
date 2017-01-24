namespace Application
{
    public class Healthcheck
    {
        public object GetMe()
        {
            return new {
                FirstName = "Wes",
                LastName = "Shaddix"
            };
        }
    }
}
