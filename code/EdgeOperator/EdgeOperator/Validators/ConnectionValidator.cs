using cz.dvojak.k8s.EdgeOperator.Entities;
using KubeOps.Operator.Webhooks;

namespace cz.dvojak.k8s.EdgeOperator.Validators;

//public class ConnectionValidator : IValidationWebhook<ConnectionEntity>
//{
//    public AdmissionOperations Operations => AdmissionOperations.Create | AdmissionOperations.Update;
//
//    public ValidationResult Create(ConnectionEntity newEntity,bool dryRun) => CheckName(newEntity)
//        ? ValidationResult.Success("Name is valid")
//        : ValidationResult.Fail(StatusCodes.Status400BadRequest,"Name is invalid");
//
//    public ValidationResult Update(ConnectionEntity oldEntity,ConnectionEntity newEntity,bool dryRun) =>
//        CheckName(newEntity)
//            ? ValidationResult.Success("Name is valid")
//            : ValidationResult.Fail(StatusCodes.Status400BadRequest,"Name is invalid");
//
//    private bool CheckName(ConnectionEntity entity) => entity.Metadata.Name != "test";
//}