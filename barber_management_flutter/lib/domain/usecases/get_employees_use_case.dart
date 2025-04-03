// lib/domain/usecases/get_employees_use_case.dart
import 'package:barbermanagemobile/domain/repositories/employee_repository.dart';

class GetEmployeesUseCase {
  final EmployeeRepository repository;

  GetEmployeesUseCase(this.repository);

  Future<List<Map<String, String>>> call() async {
    return await repository.getEmployees();
  }
}