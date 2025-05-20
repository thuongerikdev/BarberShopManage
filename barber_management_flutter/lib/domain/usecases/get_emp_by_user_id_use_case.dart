import 'package:barbermanagemobile/domain/repositories/employee_repository.dart';

class GetEmpByUserIDUseCase {
  final EmployeeRepository repository;

  GetEmpByUserIDUseCase(this.repository);

  Future<Map<String, dynamic>> call(int userID) async {
    return await repository.getEmpByUserID(userID);
  }
}