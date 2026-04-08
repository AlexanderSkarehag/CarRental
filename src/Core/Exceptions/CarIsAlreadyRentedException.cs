namespace Core.Exceptions
{
    internal class CarIsAlreadyRentedException : Exception
    {
        public CarIsAlreadyRentedException()
        {
            
        }
        public CarIsAlreadyRentedException(string message)
        {
            
        }
        public CarIsAlreadyRentedException(string message, Exception innerException)
            :base (message, innerException) 
        {
            
        }
    }
}
