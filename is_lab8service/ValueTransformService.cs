using System.ServiceModel;


namespace is_lab8service
{
    [ServiceContract]
    public interface IValueTransformService
    {
        [OperationContract]
        string TransformValue(string value, string fromUnit, string toUnit);
    }

    public class ValueTransformService : IValueTransformService
    {
        public string TransformValue(string value, string fromUnit, string toUnit)
        {
            // Плейсхолдер
            return $"Transformed {value} from {fromUnit} to {toUnit}";
        }
    }
}
