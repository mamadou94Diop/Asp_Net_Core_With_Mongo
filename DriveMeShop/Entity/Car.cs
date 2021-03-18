using System;
namespace DriveMeShop.Entity
{
    public class Car
    {
        public string Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Mileage { get; set; }
        public int ReleasedYear { get; set; }
        public int LastRevisionYear { get; set; }
        public string TransmissionMode { get; set; }

        public static implicit operator Car(void v)
        {
            throw new NotImplementedException();
        }
    }
}
