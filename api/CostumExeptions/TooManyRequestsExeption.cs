namespace api.CostumExeptions;

public class TooManyRequestsExeption(string message) : Exception(message);