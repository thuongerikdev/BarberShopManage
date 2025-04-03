// lib/domain/repositories/employee_repository.dart
abstract class EmployeeRepository {
  Future<List<Map<String, String>>> getEmployees();
}