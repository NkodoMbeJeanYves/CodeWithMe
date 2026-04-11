namespace CodeWithMe.Core.Services
{
    public class FakeService : IFakeService
    {
        DateTime _serviceCreated;
        Guid _serviceId;
        public FakeService()
        {
            _serviceCreated = DateTime.Now;
            _serviceId = Guid.NewGuid();
        }
        public string GetMessage()
        {
            return $"Welcome to Contoso! The current time is {_serviceCreated}. This service instance has an ID of {_serviceId}"; ;
        }
    }


    public interface IFakeService
    {
        string GetMessage();
    }
}
