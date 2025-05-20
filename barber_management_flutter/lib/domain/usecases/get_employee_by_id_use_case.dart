import 'package:barbermanagemobile/domain/repositories/employee_repository.dart';

class GetEmployeeByIdUseCase {
  final EmployeeRepository repository;

  GetEmployeeByIdUseCase(this.repository);

  Future<Map<String, dynamic>> call(int empID) async {
    return await repository.getEmployeeById(empID);
  }
}