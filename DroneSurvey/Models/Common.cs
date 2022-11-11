namespace DroneSurvey.Models
{
    /// <summary>
    /// EnuExecutionResponsemeration used to define exicution status 
    /// </summary>
    public enum ExecutionStatus
    {
        Fail = 0,
        Success = 1
    }

    /// <summary>
    /// Class used to define exicution status of a operation and message of the operation
    /// </summary>
    public class ExecutionResponse
    {
        public ExecutionStatus ExecutionStatus { get; set; }
        public string? Message { get; set; }
    }
}
